# PCbooster Store

Tienda online de componentes de PC, hecha como Trabajo Práctico Integrador de Programación III (UTN).
ASP.NET WebForms sobre .NET Framework 4.7.2, con interfaz en Bootstrap 5 y arquitectura en capas:

| Proyecto    | Responsabilidad                                   |
|-------------|---------------------------------------------------|
| `Vistas`    | Sitio WebForms (tienda y panel de administración) |
| `Negocio`   | Reglas de negocio y validaciones                  |
| `Dao`       | Acceso a datos (SQL parametrizado sobre SQLite)   |
| `Entidades` | Clases del dominio                                |

## Cómo correrlo

1. Abrí `TP_Integrador.sln` en Visual Studio 2022 o posterior (workload *ASP.NET and web development*).
2. Elegí `Vistas` como proyecto de inicio y presioná F5. Visual Studio restaura los paquetes NuGet.

**No hace falta instalar ningún motor de base de datos.** La app usa SQLite embebido: la primera vez
que arranca crea `Vistas/App_Data/PCbooster.db` con el schema y los datos de prueba. Para volver
a empezar de cero, borrá ese archivo.

### Usuarios de prueba

| Usuario     | Contraseña | Rol           |
|-------------|------------|---------------|
| `admAmador` | `admpsw2`  | Administrador |
| `Nom1`      | `ctr1`     | Cliente       |

## Tests

| Proyecto              | Qué prueba                                                             | Requisitos               |
|-----------------------|------------------------------------------------------------------------|--------------------------|
| `Tests/Dao.Tests`     | Acceso a datos contra un SQLite temporal                               | .NET SDK                 |
| `Tests/Negocio.Tests` | Carrito, login, compras y validaciones de la capa de negocio           | .NET SDK                 |
| `Tests/Vistas.E2E`    | El sitio real en IIS Express: login, catálogo, compra completa y admin | Compilar `Vistas` antes  |

Los dos primeros corren sin Visual Studio:

```
dotnet test Tests/Dao.Tests
dotnet test Tests/Negocio.Tests
```

Los E2E levantan `Vistas` en IIS Express (viene con Visual Studio), en un puerto libre y con una base
temporal, así que no tocan `App_Data`. Compilá la solución primero y después corré todo desde el
Explorador de pruebas de Visual Studio, o con `dotnet test Tests/Vistas.E2E`.

## Base de datos

- Schema y datos de prueba: [`Dao/Sql/schema.sql`](Dao/Sql/schema.sql) y [`Dao/Sql/seed.sql`](Dao/Sql/seed.sql),
  embebidos en `Dao.dll`.
- El proyecto se desarrolló originalmente sobre SQL Server, con stored procedures y triggers, y después
  se portó a MySQL. Esos scripts quedan como referencia en [`Scripts/legacy`](Scripts/legacy). En la
  versión actual, el SQL de cada stored procedure vive en su DAO y los triggers están en el schema de SQLite.
