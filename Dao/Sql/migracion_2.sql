-- Migración 2: detalle de producto para el modal del catálogo.
-- Después de este script se ejecuta articulos_detalle.sql, que completa los datos
-- de los artículos de prueba (los artículos cargados por el admin quedan sin detalle).

alter table Articulos add column Marca_ar varchar(50) null;
alter table Articulos add column DescripcionLarga_ar text null;
alter table Articulos add column Especificaciones_ar text null;
alter table Articulos add column Garantia_ar int null;
