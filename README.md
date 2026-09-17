# Inmobiliaria TPI

Sistema web para la gestión de alquileres temporarios de propiedades desarrollado con **ASP.NET Core MVC**.

El proyecto permite gestionar propietarios, inmuebles, inquilinos, reservas, pagos y usuarios, incorporando autenticación, autorización por roles, búsquedas, filtros, paginación y diferentes reglas de negocio relacionadas con los alquileres temporarios.

## Funcionalidades

### Gestión de propietarios

* Alta, baja y modificación de propietarios.
* Asociación de uno o varios inmuebles a cada propietario.
* Consulta de los inmuebles pertenecientes a un propietario.

### Gestión de inmuebles

* Alta, baja y modificación de inmuebles.
* Asociación de cada inmueble con un único propietario.
* Registro de:

  * Dirección.
  * Cupo máximo de personas.
  * Tipo de inmueble.
  * Coordenadas.
  * Precio por día.
  * Porcentaje de alquiler a pagar al realizar una reserva.
* Imagen de portada y múltiples imágenes adicionales.
* Posibilidad de suspender temporalmente la oferta de un inmueble.
* Filtrado de inmuebles según su estado de disponibilidad.

### Gestión de tipos de inmueble

* ABM de tipos de inmueble.
* Los tipos pueden ser, por ejemplo, casa, departamento, monoambiente, loft, etc.

### Gestión de inquilinos

* ABM de inquilinos.
* Registro de:

  * DNI.
  * Nombre completo.
  * Datos de contacto.

### Gestión de reservas

* Creación de reservas asociando:

  * Inquilino.
  * Inmueble.
  * Fecha de inicio.
  * Fecha de finalización.
  * Precio diario.
* Validación de fechas.
* Control para evitar reservas superpuestas sobre un mismo inmueble.
* Búsqueda de inmuebles disponibles entre dos fechas.
* Registro del usuario que creó la reserva.
* Renovación o extensión mediante la creación de una nueva reserva, conservando la reserva original.

### Finalización anticipada

El sistema permite finalizar una reserva antes de la fecha originalmente establecida.

* Se conserva la fecha de finalización original.
* Se registra la fecha efectiva de terminación.
* Se calcula automáticamente la multa correspondiente.
* Si se cumplió menos de la mitad del período original, la multa corresponde al 50% restante del alquiler.
* Si se cumplió la mitad o más del período original, la multa corresponde al 25%.
* La multa debe ser abonada para poder finalizar la reserva.
* El importe de la multa se registra como un pago asociado a la reserva.
* No se realizan devoluciones de dinero.

### Gestión de pagos

* Registro de pagos asociados a una reserva.
* Registro del concepto, fecha e importe.
* Edición únicamente del concepto de un pago.
* La fecha y el importe no pueden modificarse.
* Los pagos no se eliminan físicamente: al eliminarlos pasan a estado **anulado**.
* Registro del usuario que creó el pago.
* Registro del usuario que realizó la anulación.

### Usuarios, autenticación y autorización

El sistema cuenta con autenticación mediante **email y contraseña** y autorización basada en roles.

Existen dos roles:

**Administrador**

* Puede gestionar usuarios.
* Puede eliminar entidades.
* Puede consultar información de auditoría.

**Empleado**

* Puede gestionar las entidades correspondientes a su trabajo.
* Puede modificar su propio perfil.
* Puede cambiar sus datos personales, contraseña y avatar.
* No puede gestionar otros usuarios.
* No puede eliminar entidades.

La información de auditoría es visible únicamente para administradores y permite identificar qué usuario creó una reserva o pago y, cuando corresponde, quién realizó su terminación o anulación.

## Informes y búsquedas

El sistema incluye diferentes consultas e informes:

* Listado de todos los inmuebles junto con su propietario.
* Filtrado de inmuebles por disponibilidad.
* Inmuebles correspondientes a un propietario específico.
* Inmuebles más reservados durante los últimos 365 días.
* Inmuebles sin reservas durante una cantidad determinada de días.
* Reservas actualmente vigentes.
* Reservas que finalizan dentro de un período determinado.
* Pagos asociados a una reserva.
* Carga de nuevos pagos desde el listado de pagos de una reserva.
* Búsqueda de inmuebles disponibles entre dos fechas determinadas.

Los listados utilizan **paginación del lado del servidor** y las búsquedas son procesadas en el servidor.

Los valores utilizados en campos desplegables cuentan con mecanismos de búsqueda y filtrado para evitar cargar innecesariamente todos los registros disponibles.

## Tecnologías utilizadas

* **ASP.NET Core MVC**
* **C#**
* **MySQL**
* **Bootstrap**
* **Bootstrap Icons**
* **Vue.js**
* **HTML / CSS**
* **JavaScript**
* **Razor**
* **MySQL Workbench**

## Arquitectura y organización

El proyecto utiliza una arquitectura basada en:

* **Models:** representación de las entidades del sistema.
* **Controllers:** manejo de las solicitudes HTTP y lógica de interacción.
* **Views:** interfaces desarrolladas con Razor y Bootstrap.
* **Repositories:** acceso y operaciones sobre la base de datos.
* **DatabaseHelper:** gestión de la conexión y operaciones con MySQL.
* **wwwroot:** archivos estáticos como CSS, JavaScript e imágenes.

## Modelado de datos

Dentro de la carpeta `docs/` se encuentran los archivos correspondientes al modelado de la aplicación:

* `UML inmobiliaria.draw.io.svg`
* `DER inmobiliaria.mwb`

El sistema contempla las siguientes entidades principales:

* Propietario
* Inmueble
* TipoInmueble
* Inquilino
* Reserva
* Pago
* Usuario

## Requisitos previos

Para ejecutar el proyecto se necesita tener instalado:

* .NET SDK
* MySQL Server
* MySQL Workbench
* Git

## Clonar el proyecto

```bash
git clone https://github.com/Fabrizioandres1998/sistema_inmobiliaria.git
cd sistema_inmobiliaria
```

## Configuración de la base de datos

La copia de la base de datos se encuentra en:

```text
docs/sistema_inmobiliaria backup.sql
```

### Importar la base de datos

1. Abrir **MySQL Workbench**.
2. Conectarse al servidor MySQL.
3. Ir a **Server → Data Import**.
4. Seleccionar **Import from Self-Contained File**.
5. Elegir:

```text
docs/sistema_inmobiliaria backup.sql
```

6. Realizar la importación.

## Configuración de la conexión

El proyecto incluye un archivo de ejemplo:

```text
appsettings.Example.json
```

Copiarlo y renombrarlo como:

```text
appsettings.json
```

Luego completar los datos correspondientes a la conexión de MySQL.

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sistema_inmobiliaria;User=root;Password=TU_CONTRASEÑA;"
  }
}
```

## Usuarios de prueba

El proyecto incluye usuarios de prueba para acceder al sistema.

### Administrador

```text
Email: admin@inmobiliaria.com
Contraseña: admin123
Rol: Administrador
```

### Empleado

```text
Email: empleadouno@inmobiliaria.com
Contraseña: empleadouno123
Rol: Empleado
```

## Ejecutar el proyecto

Restaurar las dependencias:

```bash
dotnet restore
```

Ejecutar la aplicación:

```bash
dotnet run
```

La aplicación quedará disponible en la dirección indicada por ASP.NET Core al iniciar el proyecto.

## Autor

**D'Isidoro Fabrizio Andres**

Proyecto desarrollado como parte de la formación académica en desarrollo de software.
