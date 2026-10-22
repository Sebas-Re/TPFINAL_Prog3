-- Migración 1: para bases creadas antes del versionado (user_version = 0).
-- Una base nueva ya nace con estos cambios (schema.sql + seed.sql).

-- Fecha de alta de los usuarios, para medir clientes nuevos. SQLite no permite
-- agregar una columna con default no constante, así que los inserts la cargan.
alter table Usuarios add column FechaAlta_us datetime null;
update Usuarios set FechaAlta_us = datetime('now', 'localtime') where FechaAlta_us is null;

-- En los datos de prueba faltaba el punto de la extensión.
update Articulos set Imagen = '~/Imagenes_ar/Fuente/fnt8.png' where Imagen = '~/Imagenes_ar/Fuente/fnt8png';
