# ProyectoInmobiliaria
# Sistema de Gestión Inmobiliaria

> Aplicación web desarrollada en ASP.NET Core MVC para la administración integral de propiedades, propietarios, inquilinos, inmuebles, reservas, pagos y usuarios con persistencia en MySQL.

---

## Integrantes del Grupo

* **Ortiz Paez Lourdes**
* **Silva Fabricio Daniel**

---

## Tecnologías Utilizadas

* **Lenguaje:** C# (.NET 10.0)
* **Framework Web:** ASP.NET Core MVC
* **Base de Datos:** MySQL (`MySql.Data`)
* **Front-end:** Razor Views, HTML5, CSS3, Bootstrap 5

---

## Instrucciones para Levantar la Base de Datos

Para inicializar la base de datos en tu entorno local de MySQL, ejecutá el script DB_inmobiliaria.sql incluido en este repositorio siguiendo estos pasos:

## Desde MySQL Workbench / DBeaver: 

Abrí tu gestor de base de datos (MySQL Workbench, DBeaver, HeidiSQL, etc.).

Conéctate a tu servidor local de MySQL.

Abrí el archivo DB_inmobiliaria.sql (File -> Open Script).

Ejecutá todo el script para crear la base de datos inmobiliaria y sus tablas correspondientes.


## Desde la Terminal (CMD / PowerShell): 

Abrí la terminal en la carpeta donde tenés el archivo .sql.

Ejecutá el siguiente comando reemplazando root por tu usuario de MySQL:

Bash
mysql -u root -p < DB_inmobiliaria.sql
Ingresá tu contraseña de MySQL cuando la consola lo solicite.

---

## Configuración y Ejecución (.NET Core)

1. Abrí la solución `ProyectoInmobiliaria.sln` en Visual Studio o Visual Studio Code
2. Si las credenciales de tu servidor MySQL local son distintas, actualizá la variable `cadenaConexion` dentro del archivo `Program.cs`:
```csharp
string cadenaConexion = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=admin;";
```
---

## Estructura del Proyecto
```text
ProyectoInmobiliaria
├── Controllers/
│   ├── HomeController.cs
│   ├── InquilinosController.cs
│   ├── PropietariosController.cs
│   ├── InmuebleController.cs
│   ├── TipoInmuebleController.cs
│   ├── UsuarioController.cs
│   ├── PagoController.cs
│   └── ReservasController.cs
├── models/
│   ├── Inquilino.cs
│   ├── Propietario.cs
│   ├── Inmueble.cs
│   ├── TipoInmueble.cs
│   ├── Usuario.cs
│   ├── Pago.cs
│   └── Reserva.cs
├── Repository/
│   ├── InquilinoRepository.cs
│   ├── PropietarioRepository.cs
│   ├── InmuebleRepository.cs
│   ├── TipoInmuebleRepository.cs
│   ├── UsuarioRepository.cs
│   ├── PagoRepository.cs
│   └── ReservaRepository.cs
├── Views/
│   ├── Home/
│   ├── Inquilinos/
│   ├── Propietarios/
│   ├── Inmueble/
│   ├── TipoInmueble/
│   ├── Pago/
│   ├── Reservas/
│   └── Shared/
│       └── _Layout.cshtml
├── wwwroot/
│   └── avatars/
└── Program.cs
```

## Usuarios de Prueba

El script de base de datos incluye la creación de los siguientes usuarios por defecto para pruebas de acceso al sistema:

| Rol | Correo Electrónico | Contraseña | Nombre Completo |
| :--- | :--- | :--- | :--- |
| **Administrador** | `admin@inmobiliaria.com` | `123456` | Admin Sistema |
| **Empleado** | `empleado@inmobiliaria.com` | `123456` | Empleado Sistema |