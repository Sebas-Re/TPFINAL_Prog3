use master
go


create database BD_Pcbooster_store
GO
use BD_Pcbooster_store
go

----CREATE TRIGGER TR_SEGURIDAD
----	ON DATABASE FOR DROP_TABLE, ALTER_TABLE
----	AS BEGIN
----	RAISERROR ('No está permitido borrar ni
----	modificar tablas !' , 16, 1) 
----	ROLLBACK TRANSACTION 
----	ENd
---------------------------------------------------------------------------
--Tabla usuarios-- 
---------------------------------------------------------------------------
create table Usuarios(
	IdUsuario_us bigint not null identity(1,1),
	Nombre_us varchar(10) not null unique,
	Contraseña_us varchar(7) not null,
	Email_us varchar(30) not null unique,
	Telefono_us varchar(15) null default 'Sin telefono',
	Tipo_us bit not null DEFAULT 0, --Si es 1, es admin. Si es 0, es usuario
	Estado_us bit not null default '1',
	constraint PK_Usuarios primary key (IdUsuario_us)
	)
go
---------------------------------------------------------------------------
--Tabla Categorias--
---------------------------------------------------------------------------
create table Categorias(
	IdCategoria_ca bigint not null identity(1,1),
	Descripcion_ca varchar(30) not null unique,
	Estado_ca bit not null default '1',
	constraint PK_Categorias_ca primary key (IdCategoria_ca)
	)
go
---------------------------------------------------------------------------
--Tabla articulos--
---------------------------------------------------------------------------

create table Articulos(
	CodArticulo_ar char(4) not null,
	Categoria_ar bigint not null,
	Descripcion_ar varchar(300) null unique,
	Stock_ar bigint null default '0',
	PrecioUnitario_ar money not null,
	Imagen varchar(100) null default 'Sin imagen',
	Estado_ar bit not null default '1',
	constraint CK_Articulos_Stock_ar check (Stock_ar >=0),
	constraint CK_Articulos_PrecioUnitario_Ar check (PrecioUnitario_ar >0),
	constraint PK_Articulos primary key(CodArticulo_ar),
	constraint Fk_Articulos_Categoria_ar foreign key (Categoria_ar) references Categorias(IdCategoria_ca) 
)
go
---------------------------------------------------------------------------
--Tabla Ventas--
---------------------------------------------------------------------------
 	create table Ventas(
	IdVentas_ve bigint not null identity (1,1),
	IdUsuario_ve bigint not null,
	TotalVenta_ve money not null default '0',
	FechaVenta_ve smalldatetime not null default getdate(),
	Metodo_de_Pago_ve bit not null,
	Codigo_Seguridad_ve int not null, 
	constraint CK_Ventas_TotalVentas check (TotalVenta_ve >= 0 ),
	constraint CK_Ventas_FechaVentas check (FechaVenta_ve <= getdate()),
	constraint PK_Ventas primary key(IdVentas_ve,IdUsuario_ve),
	constraint FK_Ventas_Cuentas_Cod foreign key(IdUsuario_ve) references Usuarios(IdUsuario_us)
)
go


---------------------------------------------------------------------------
--Tabla Detalle Ventas--
---------------------------------------------------------------------------
create  table Detalle_Ventas(
	IdDetVenta_dv bigint not null,
	IdUsuario_dv bigint not null,
	CodArticulo_dv char(4) not null,
	Cantidad_dv bigint not null,
	Precio_Unitario_dv money not null,
	Direccion_de_Entrega text not null,
	Contacto varchar(20) not null,
	constraint CK_Detalle_Ventas_Cantidad check (Cantidad_dv>=0),
	constraint CK_Detalle_Ventas_Precio_Unitario check (Precio_Unitario_dv >=0),
	constraint PK_Detalle_Ventas primary key (IdDetVenta_dv, CodArticulo_dv),
	constraint FK_DetalleVentas_Ventas foreign key (IdDetVenta_dv,IdUsuario_dv) references Ventas(IdVentas_ve,IdUsuario_ve),
	constraint FK_DetalleVentas_Articulos foreign key (CodArticulo_dv) references Articulos(CodArticulo_ar)
)
go
---------------------------------------------------------------------------
--Tabla Tarjeta de Credito--
---------------------------------------------------------------------------

create table  Tarjetas(
	Id_TarjetaUsuario_t bigint not null,
	Titular_Tarjeta_t varchar(50) not null,
	Nombre_Tarjeta_t varchar(20) not null,
	Tipo_Tarjeta_t bit not null,
	Num_Tarjeta_t bigint not null,
	Fecha_Vencimiento_t date not null,
	Codigo_Seguridad_t int not null,
	constraint Ck_Tarjetas_Fecha_Vencimiento check (Fecha_Vencimiento_t > getdate()),
	constraint PK_Tarjetas primary key (Id_tarjetaUsuario_t,Codigo_Seguridad_t),
	constraint FK_Tarjetas_Usuarios foreign key (Id_tarjetaUsuario_t) references Usuarios (IdUsuario_us)
	)
go


	

