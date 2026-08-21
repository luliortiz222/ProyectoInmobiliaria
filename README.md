# ProyectoInmobiliaria
# Sistema de Gestión Inmobiliaria

> Aplicación web desarrollada en ASP.NET Core MVC para la administración integral de propiedades, propietarios e inquilinos con persistencia en MySQL.

---

## Integrantes del Grupo

* **Ortiz Paez Lourdes**
* **Silva Fabricio**

---

## Tecnologías Utilizadas

* **Lenguaje:** C# (.NET 10.0)
* **Framework Web:** ASP.NET Core MVC
* **Base de Datos:** MySQL (`MySql.Data`)
* **Front-end:** Razor Views, HTML5, CSS3, Bootstrap 5
* **Pruebas de API:** Postman

---

## Estructura del Proyecto

```text
ProyectoInmobiliaria/
├── Controllers/
│   ├── InquilinosController.cs
│   └── PropietariosController.cs
├── Models/
│   ├── Inquilino.cs
│   └── Propietario.cs
├── Repository/
│   ├── InquilinoRepository.cs
│   └── PropietarioRepository.cs
├── Views/
│   ├── Inquilinos/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   ├── Propietarios/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   └── Shared/
│       └── _Layout.cshtml
└── Program.cs


 Diagrama de Clases UML
Fragmento de código
classDiagram
    class Propietario {
        +int IdPropietario
        +String Dni
        +String Nombre
        +String Apellido
        +String Telefono
        +String Email
        +Propietario()
        +Propietario(int id, string dni, string nombre, string apellido, string telefono, string email)
    }

    class Inquilino {
        +int IdInquilino
        +String Dni
        +String Nombre
        +String Apellido
        +String Telefono
        +String Email
        +Inquilino()
        +Inquilino(int id, string dni, string nombre, string apellido, string telefono, string email)
    }

    class PropietarioRepository {
        -_cadenaConexion: string
        +Guardar(p: Propietario) void
        +ObtenerTodos() List~Propietario~
        +ObtenerPorId(id: int) Propietario
        +Actualizar(p: Propietario) void
        +Eliminar(id: int) void
        +PropietarioRepository(cadena: string)
    }

    class InquilinoRepository {
        -_cadenaConexion: string
        +Guardar(i: Inquilino) void
        +ObtenerTodos() List~Inquilino~
        +ObtenerPorId(id: int) Inquilino
        +Modificar(i: Inquilino) void
        +Eliminar(id: int) void
        +InquilinoRepository(cadena: string)
    }

    PropietarioRepository ..> Propietario : usa
    InquilinoRepository ..> Inquilino : usa



Instrucciones para Levantar la Base de Datos

Para inicializar la base de datos en tu entorno local de MySQL, ejecutá el script script_inmobiliaria.sql incluido en este repositorio siguiendo estos pasos:

Opción A: Desde MySQL Workbench / DBeaver

Abrí tu gestor de base de datos (MySQL Workbench, DBeaver, HeidiSQL, etc.).

Conéctate a tu servidor local de MySQL.

Abrí el archivo script_inmobiliaria.sql (File -> Open Script).

Ejecutá todo el script para crear la base de datos inmobiliaria y sus tablas correspondientes.


Opción B: Desde la Terminal (CMD / PowerShell)

Abrí la terminal en la carpeta donde tenés el archivo .sql.

Ejecutá el siguiente comando reemplazando root por tu usuario de MySQL:

Bash
mysql -u root -p < script_inmobiliaria.sql
Ingresá tu contraseña de MySQL cuando la consola lo solicite.


Configuración y Ejecución (.NET Core)

Abrí la solución ProyectoInmobiliaria.sln en Visual Studio.

Verificá que el archivo appsettings.json tenga la cadena de conexión correspondiente a tu MySQL local:

JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=admin;"
  }
}
Presioná F5 para ejecutar la aplicación en el servidor Kestrel local.

Endpoints (API REST)

Inquilinos
GET /api/inquilinos

GET /api/inquilinos/{id}

POST /api/inquilinos

PUT /api/inquilinos/{id}

DELETE /api/inquilinos/{id}

Propietarios
GET /api/propietarios

GET /api/propietarios/{id}

POST /api/propietarios

PUT /api/propietarios/{id}

DELETE /api/propietarios/{id}