CREATE DATABASE IF NOT EXISTS sistema_alquileres;
USE sistema_alquileres;

-- 1. Tabla Usuario (para administradores y empleados)
CREATE TABLE usuario (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    rol ENUM('ADMINISTRADOR', 'EMPLEADO') NOT NULL,
    nombre_completo VARCHAR(150) NOT NULL,
    avatar VARCHAR(255),
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    fecha_ultima_modificacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 2. Tabla Propietario
CREATE TABLE propietario (
    id_propietario INT AUTO_INCREMENT PRIMARY KEY,
    nombre_completo VARCHAR(150) NOT NULL,
    dni VARCHAR(20) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL,
    telefono VARCHAR(50),
    direccion VARCHAR(200),
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- 3. Tabla TipoInmueble
CREATE TABLE tipo_inmueble (
    id_tipo_inmueble INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT
);

-- 4. Tabla Inquilino
CREATE TABLE inquilino (
    id_inquilino INT AUTO_INCREMENT PRIMARY KEY,
    dni VARCHAR(20) NOT NULL UNIQUE,
    nombre_completo VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL,
    telefono VARCHAR(50),
    direccion VARCHAR(200),
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- 5. Tabla Inmueble
CREATE TABLE inmueble (
    id_inmueble INT AUTO_INCREMENT PRIMARY KEY,
    direccion VARCHAR(200) NOT NULL,
    cupo_maximo INT NOT NULL,
    coordenadas VARCHAR(100),
    precio_por_dia DECIMAL(10, 2) NOT NULL,
    imagen_portada VARCHAR(255),
    disponible BOOLEAN DEFAULT TRUE,
    porcentaje_reserva DECIMAL(5, 2) DEFAULT 0.00,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    id_propietario INT NOT NULL,
    id_tipo_inmueble INT NOT NULL,
    
    CONSTRAINT fk_inmueble_propietario 
        FOREIGN KEY (id_propietario) REFERENCES propietario(id_propietario) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_inmueble_tipo 
        FOREIGN KEY (id_tipo_inmueble) REFERENCES tipo_inmueble(id_tipo_inmueble) 
        ON DELETE RESTRICT ON UPDATE CASCADE
);

-- 6. Tabla Reserva
CREATE TABLE reserva (
    id_reserva INT AUTO_INCREMENT PRIMARY KEY,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    fecha_fin_original DATE NOT NULL,
    monto_por_dia DECIMAL(10, 2) NOT NULL,
    estado ENUM('ACTIVA', 'FINALIZADA', 'CANCELADA') DEFAULT 'ACTIVA',
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    fecha_terminacion DATETIME NULL,
    multa_aplicada DECIMAL(10, 2) DEFAULT 0.00,
    
    -- Foreign Keys
    id_inquilino INT NOT NULL,
    id_inmueble INT NOT NULL,
    id_usuario_creador INT NOT NULL,
    id_usuario_terminacion INT NULL,
    
    CONSTRAINT fk_reserva_inquilino 
        FOREIGN KEY (id_inquilino) REFERENCES inquilino(id_inquilino) 
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_reserva_inmueble 
        FOREIGN KEY (id_inmueble) REFERENCES inmueble(id_inmueble) 
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_reserva_usuario_creador 
        FOREIGN KEY (id_usuario_creador) REFERENCES usuario(id_usuario) 
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_reserva_usuario_terminacion 
        FOREIGN KEY (id_usuario_terminacion) REFERENCES usuario(id_usuario) 
        ON DELETE SET NULL ON UPDATE CASCADE
);

-- 7. Tabla Pago
CREATE TABLE pago (
    id_pago INT AUTO_INCREMENT PRIMARY KEY,
    concepto VARCHAR(200) NOT NULL,
    fecha_pago DATE NOT NULL,
    importe DECIMAL(10, 2) NOT NULL,
    estado ENUM('ACTIVO', 'ANULADO') DEFAULT 'ACTIVO',
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    fecha_anulacion DATETIME NULL,
    
    -- Foreign Keys
    id_reserva INT NOT NULL,
    id_usuario_creador INT NOT NULL,
    id_usuario_anulacion INT NULL,
    
    CONSTRAINT fk_pago_reserva 
        FOREIGN KEY (id_reserva) REFERENCES reserva(id_reserva) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_pago_usuario_creador 
        FOREIGN KEY (id_usuario_creador) REFERENCES usuario(id_usuario) 
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT fk_pago_usuario_anulacion 
        FOREIGN KEY (id_usuario_anulacion) REFERENCES usuario(id_usuario) 
        ON DELETE SET NULL ON UPDATE CASCADE
);