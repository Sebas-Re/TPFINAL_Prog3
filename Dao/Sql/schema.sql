-- Schema SQLite de BD_Pcbooster_store.
-- Port de Legacy/Scripts/sqlserver/Script DB_PCbooster_Store.sql.
-- Los tipos bit/money/date se declaran así a propósito: System.Data.SQLite los
-- mapea a Boolean/Decimal/DateTime, igual que SqlClient con SQL Server.
-- COLLATE NOCASE replica la collation case-insensitive de SQL Server en las
-- columnas que se comparan por igualdad desde la app.

create table Usuarios(
	IdUsuario_us integer primary key autoincrement,
	Nombre_us varchar(10) not null unique collate nocase,
	Contraseña_us varchar(7) not null,
	Email_us varchar(30) not null unique collate nocase,
	Telefono_us varchar(15) null default 'Sin telefono',
	Tipo_us bit not null default 0, -- 1 = admin, 0 = usuario
	Estado_us bit not null default 1,
	FechaAlta_us datetime null default (datetime('now', 'localtime'))
);

create table Categorias(
	IdCategoria_ca integer primary key autoincrement,
	Descripcion_ca varchar(30) not null unique collate nocase,
	Estado_ca bit not null default 1
);

create table Articulos(
	CodArticulo_ar char(4) not null collate nocase,
	Categoria_ar bigint not null,
	Descripcion_ar varchar(300) null unique collate nocase,
	Stock_ar bigint null default 0,
	PrecioUnitario_ar money not null,
	Imagen varchar(100) null default 'Sin imagen',
	Estado_ar bit not null default 1,
	-- Detalle del producto (se muestra en el catálogo)
	Marca_ar varchar(50) null,
	DescripcionLarga_ar text null,
	Especificaciones_ar text null, -- una por línea: "Clave: Valor"
	Garantia_ar int null, -- meses
	constraint CK_Articulos_Stock_ar check (Stock_ar >= 0),
	constraint CK_Articulos_PrecioUnitario_Ar check (PrecioUnitario_ar > 0),
	constraint PK_Articulos primary key (CodArticulo_ar),
	constraint Fk_Articulos_Categoria_ar foreign key (Categoria_ar) references Categorias(IdCategoria_ca)
);

-- En SQL Server la PK era (IdVentas_ve, IdUsuario_ve) con identity en IdVentas_ve.
-- SQLite solo autoincrementa una "integer primary key", así que el par queda
-- como unique para que la FK compuesta de Detalle_Ventas siga siendo válida.
create table Ventas(
	IdVentas_ve integer primary key autoincrement,
	IdUsuario_ve bigint not null,
	TotalVenta_ve money not null default 0,
	FechaVenta_ve datetime not null default (datetime('now', 'localtime')),
	Metodo_de_Pago_ve bit not null,
	Codigo_Seguridad_ve int not null,
	constraint CK_Ventas_TotalVentas check (TotalVenta_ve >= 0),
	constraint UQ_Ventas_Usuario unique (IdVentas_ve, IdUsuario_ve),
	constraint FK_Ventas_Cuentas_Cod foreign key (IdUsuario_ve) references Usuarios(IdUsuario_us)
);

create table Detalle_Ventas(
	IdDetVenta_dv bigint not null,
	IdUsuario_dv bigint not null,
	CodArticulo_dv char(4) not null collate nocase,
	Cantidad_dv bigint not null,
	Precio_Unitario_dv money not null,
	Direccion_de_Entrega text not null,
	Contacto varchar(20) not null,
	constraint CK_Detalle_Ventas_Cantidad check (Cantidad_dv >= 0),
	constraint CK_Detalle_Ventas_Precio_Unitario check (Precio_Unitario_dv >= 0),
	constraint PK_Detalle_Ventas primary key (IdDetVenta_dv, CodArticulo_dv),
	constraint FK_DetalleVentas_Ventas foreign key (IdDetVenta_dv, IdUsuario_dv) references Ventas(IdVentas_ve, IdUsuario_ve),
	constraint FK_DetalleVentas_Articulos foreign key (CodArticulo_dv) references Articulos(CodArticulo_ar)
);

create table Tarjetas(
	Id_TarjetaUsuario_t bigint not null,
	Titular_Tarjeta_t varchar(50) not null,
	Nombre_Tarjeta_t varchar(20) not null,
	Tipo_Tarjeta_t bit not null,
	Num_Tarjeta_t bigint not null,
	Fecha_Vencimiento_t date not null,
	Codigo_Seguridad_t int not null,
	constraint PK_Tarjetas primary key (Id_TarjetaUsuario_t, Codigo_Seguridad_t),
	constraint FK_Tarjetas_Usuarios foreign key (Id_TarjetaUsuario_t) references Usuarios(IdUsuario_us)
);

-- SQLite no admite funciones no deterministas (date('now')) en un CHECK,
-- así que Ck_Tarjetas_Fecha_Vencimiento pasa a ser un trigger.
create trigger tr_Tarjetas_Fecha_Vencimiento
before insert on Tarjetas
when new.Fecha_Vencimiento_t <= date('now', 'localtime')
begin
	select raise(abort, 'Ck_Tarjetas_Fecha_Vencimiento: la tarjeta está vencida');
end;

-- Calcula el total por compra
create trigger tr_SumaTotal
after insert on Detalle_Ventas
begin
	update Ventas set TotalVenta_ve = TotalVenta_ve + new.Cantidad_dv * new.Precio_Unitario_dv
	where IdUsuario_ve = new.IdUsuario_dv;
end;

-- Descuenta el stock después de una compra
create trigger tr_ControlStock
after insert on Detalle_Ventas
begin
	update Articulos set Stock_ar = Stock_ar - new.Cantidad_dv
	where CodArticulo_ar = new.CodArticulo_dv;
end;
