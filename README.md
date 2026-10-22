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
que arranca crea `Vistas/App_Data/PCbooster.db` con el schema, los datos de prueba y un historial de
demo (20 clientes y unas 180 ventas de los últimos 6 meses, con fechas relativas al día en que se crea),
para que el panel y los reportes tengan qué mostrar. Para volver a empezar de cero, borrá ese archivo.

### Usuarios de prueba

| Usuario     | Contraseña | Rol           |
|-------------|------------|---------------|
| `admAmador` | `admpsw2`  | Administrador |
| `Nom1`      | `ctr1`     | Cliente       |
| `lucia.g`   | `demo123`  | Cliente (historial de demo, igual que el resto de los `*.x`) |

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

- Schema y datos: [`Dao/Sql/schema.sql`](Dao/Sql/schema.sql), [`seed.sql`](Dao/Sql/seed.sql) (datos de prueba)
  y [`demo.sql`](Dao/Sql/demo.sql) (historial), embebidos en `Dao.dll`.
- La versión del schema se guarda en `PRAGMA user_version`. Si la app encuentra una base anterior, le aplica
  los `migracion_N.sql` pendientes sin tocar los datos existentes (el historial de demo solo se carga en bases nuevas).
- Las contraseñas se guardan como hash PBKDF2-SHA256 con salt ([`Contrasenas.cs`](Dao/Contrasenas.cs)). Los scripts
  de datos las traen en texto plano para que se puedan leer, y se hashean al crear la base; la migración a la
  versión 4 hashea las de una base existente.
- El proyecto se desarrolló originalmente sobre SQL Server, con stored procedures y triggers, y después
  se portó a MySQL. Esos scripts, junto con páginas viejas del sitio, quedan como referencia en [`Legacy`](Legacy). En la
  versión actual, el SQL de cada stored procedure vive en su DAO y los triggers están en el schema de SQLite.
