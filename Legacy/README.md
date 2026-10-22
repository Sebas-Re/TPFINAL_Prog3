# Legacy

Material histórico del proyecto. Se conserva como referencia; la aplicación no lo usa ni lo compila.

| Carpeta                 | Qué es                                                                                   |
|-------------------------|------------------------------------------------------------------------------------------|
| `Scripts/sqlserver`     | Versión original de la base, en SQL Server: tablas, stored procedures, triggers y datos. |
| `Scripts/mysql`         | Port posterior de esos mismos scripts a MySQL.                                           |
| `Vistas-Administrador`  | Primeras páginas del panel de administración, reemplazadas por `Vistas/AdminForms`.      |

La base actual es SQLite: [`Dao/Sql/schema.sql`](../Dao/Sql/schema.sql) y [`Dao/Sql/seed.sql`](../Dao/Sql/seed.sql).
