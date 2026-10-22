# PCbooster Store

Tienda online de componentes de PC, hecha como Trabajo Práctico Integrador de Programación III (UTN).
ASP.NET WebForms sobre .NET Framework 4.7.2, con arquitectura en capas:

| Proyecto    | Responsabilidad                                   |
|-------------|---------------------------------------------------|
| `Vistas`    | Sitio WebForms (páginas de usuario y de admin)    |
| `Negocio`   | Reglas de negocio y validaciones                  |
| `Dao`       | Acceso a datos (SQL parametrizado sobre SQLite)   |
| `Entidades` | Clases del dominio                                |

## Cómo correrlo

1. Abrí `TP_Integrador.sln` en Visual Studio 2022 (workload *ASP.NET and web development*).
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

Tests de integración de la capa de datos contra un SQLite temporal. Corren sin Visual Studio, con el .NET SDK:

```
dotnet test Tests/Dao.Tests
```

## Base de datos

- Schema y datos de prueba: [`Dao/Sql/schema.sql`](Dao/Sql/schema.sql) y [`Dao/Sql/seed.sql`](Dao/Sql/seed.sql),
  embebidos en `Dao.dll`.
- El proyecto se desarrolló originalmente sobre SQL Server, con stored procedures y triggers, y después
  se portó a MySQL. Esos scripts quedan como referencia en [`Scripts/legacy`](Scripts/legacy). En la
  versión actual, el SQL de cada stored procedure vive en su DAO y los triggers están en el schema de SQLite.
