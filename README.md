# Inmobiliaria TPI

Proyecto de gestión inmobiliaria desarrollado en ASP.NET Core.

## Desarrollada por

D'Isidoro Fabrizio Andres

## Estructura

El código fuente está en la carpeta `InmobiliariaTPI/`

## Clonar el proyecto

git clone https://github.com/Fabrizioandres1998/sistema_inmobiliaria.git

## Modelado de datos

En docs/ se encuentra el UML llamado UML inmobiliaria.draw.io.svg y el diagrama entidad-relacion llamado DER inmobiliaria.mwb

## Base de datos

La copia de la base de datos se encuentra en la carpeta docs/:

docs/sistema_inmobiliaria backup.sql

Abrí MySQL Workbench y conectate a tu servidor.

Andá a Server → Data Import.

Seleccioná "Import from Self-Contained File" y elegí el archivo sistema_inmobiliaria backup.sql

## Configurar la conexion

El proyecto incluye un archivo de ejemplo appsettings.Example.json con la estructura necesaria.

Pasos:

Copiá el archivo appsettings.Example.json y renombralo como appsettings.json

Abrí appsettings.json y completá tu usuario y contraseña de MySQL:

## Cómo ejecutar

```bash
cd InmobiliariaTPI
dotnet restore
dotnet run
```
