use BD_Pcbooster_store
go
-------------------------------------------------
--PROCEDIMIENTOS ALMACENADOS TP INTEGRADOR PROG 3
-------------------------------------------------
-------------------------------------------------
--PROCEDIMIENTOS USUARIOS
-------------------------------------------------
--agregar usuario
create or alter procedure sp_AgregarUsuario 
(
@NOMBRE_US varchar(10),
@CONTRASEÑA_US varchar(7),
@EMAIL_US varchar(30),
@TELEFONO_US varchar(15),
@TIPO_US bit 
)
as 
insert into Usuarios(Nombre_us,Contraseña_us,Email_us,Telefono_us,Tipo_us,Estado_us)
values (@NOMBRE_US,@CONTRASEÑA_US,@EMAIL_US,@TELEFONO_US,@TIPO_US,'true')
return 
go
-------------------------------------------------
--eliminar usuario

create or alter procedure [dbo].[sp_EliminarUsuariosAdmin]
(

@IdUsuario_us int
)
as
update Usuarios
set
Estado_us = 'false'
where IdUsuario_us = @IdUsuario_us
return  
go
-------------------------------------------------
--actualizar usuario
create or alter procedure sp_ModificarUsuario
(
@Id_Usuario_us bigint,
@NOMBRE_US varchar(10),
@CONTRASEÑA_US varchar(7),
@EMAIL_US varchar(30),
@TELEFONO_US varchar(15),
@TIPO_US bit,
@Estado_us bit
)
as
update Usuarios
set
Nombre_us = @NOMBRE_US,
Contraseña_us = @CONTRASEÑA_US,
Email_us = @EMAIL_US,
Telefono_us = @TELEFONO_US,
Tipo_us = @TIPO_US,
Estado_us = @Estado_us
where IdUsuario_us = @Id_Usuario_us
return
go
-------------------------------------------------
--PROCEDIMIENTOS ARTICULOS	
-------------------------------------------------
--agregar articulo
-------------------------------------------------
create or alter  procedure sp_AgregarArticulo
(
@CodArticulo char(4),
@Categoria bigint,
@Descripcion varchar(300),
@Stock bigint,
@PrecioUnitario MONEY,
@ImagenURL varchar(255)
)
as
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
values (@CodArticulo,@Categoria,@Descripcion,@Stock,@PrecioUnitario,@ImagenURL)
return
go
-------------------------------------------------
--eliminar articulo

create or alter procedure sp_EliminarArticulo
(
@CODARTICULO_AR char(4)
)
as
update Articulos
set
Estado_ar = 'false'
where CodArticulo_ar = @CODARTICULO_AR
return 
go
-------------------------------------------------
--eliminar modificar articulo
create or alter procedure [dbo].[sp_ModificarArticulo]
(
@CodArticulo char(4),
@Categoria bigint,
@Descripcion varchar(300),
@Stock bigint,
@PrecioUnitario money,
@ImagenURL varchar(255)
)
as 
update Articulos
set
Categoria_ar = @CodArticulo,
Descripcion_ar = @Descripcion,
Stock_ar = @Stock,
PrecioUnitario_ar = @PrecioUnitario,
Imagen = @ImagenURL
where CodArticulo_ar = @CodArticulo
return  
go
-------------------------------------------------
--PROCEDIMIENTOS CATEGORIAS	
-------------------------------------------------
--agregar categoria
-------------------------------------------------
create or alter procedure sp_AgregarCategoria
(
@DESCRIPCION_CA varchar(30)
)
as
insert into Categorias(Descripcion_ca,Estado_ca)
values (@DESCRIPCION_CA,'true')
return
go
-------------------------------------------------
--eliminar eliminar categoria
create or alter procedure sp_EliminarCategoria
(
@IDCATEGORIA_CA bigint
)
as
update Categorias
set
Estado_ca = 'false'
where IdCategoria_ca = @IDCATEGORIA_CA
return 
go


-------------------------------------------------
--modificar categoria
create or alter procedure sp_ModificarCategoria
(
@IDCATEGORIA_CA bigint,

@DESCRIPCION_CA varchar(30),

@ESTADO_CA bit

)
as 
update Categorias
set

Descripcion_ca = @DESCRIPCION_CA,

Estado_ca = @ESTADO_CA


where IdCategoria_ca = @IDCATEGORIA_CA
return 
go



-------------------------------------------------
--PROCEDIMIENTOS VENTAS	
-------------------------------------------------
--agregar venta
-------------------------------------------------

create or alter procedure sp_AgregarVenta
(
@IDUSUARIO_VE bigint,
@METODODEPAGO_VE bit,
@CODIGODESEGURIDAD_VE int
)
as
insert into Ventas(IdUsuario_ve,Metodo_de_Pago_ve,Codigo_Seguridad_ve, FechaVenta_ve)
values (@IDUSUARIO_VE,@METODODEPAGO_VE,@CODIGODESEGURIDAD_VE, getdate())
return
go

--------------------------------------------------------------



-------------------------------------------------
--PROCEDIMIENTOS DETALLE VENTAS
-------------------------------------------------
--agregar detalle ventas
-------------------------------------------------

create or alter procedure sp_AgregarDetalleVenta
(
@IDDETVENTA_DV bigint,
@IDUSUARIO_DV bigint,
@CODARTICULO_DV char(4),
@CANTIDAD_DV bigint,
@PRECIOUNITARIO_DV money,
@DIRECCIONENTREGA_DV text,
@CONTACTO varchar(20)
)
as
insert into Detalle_Ventas(IdDetVenta_dv,IdUsuario_dv,CodArticulo_dv,Cantidad_dv,Precio_Unitario_dv,Direccion_de_Entrega,Contacto)
values (@IDDETVENTA_DV,@IDUSUARIO_DV,@CODARTICULO_DV,@CANTIDAD_DV,@PRECIOUNITARIO_DV,@DIRECCIONENTREGA_DV,@CONTACTO)
return
GO


-------------------------------------------------
--PROCEDIMIENTOS REPORTES
-------------------------------------------------
--COMPRAS POR DETERMINADO CLIENTE
---------------------------------------------------

create or alter procedure sp_ComprasCliente
(
@IdCliente bigint
)
AS
SELECT * from Ventas
WHERE IdUsuario_ve = @IdCliente
go
RETURN
GO

create or alter procedure sp_MaxArticulo
(
@CodArt char(4)
)
AS
--Ventas totales, cant total vendida y stock actual de un determinado articulo
SELECT sum(Precio_Unitario_dv) AS [VENTAS TOTALES], sum(Cantidad_dv) AS [UNIDADES VENDIDAS], Stock_ar from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv
GROUP BY Precio_Unitario_dv, Cantidad_dv, Stock_ar, CodArticulo_dv
HAVING CodArticulo_dv = @CodArt
RETURN
GO

-------------------------------------------------
--PROCEDIMIENTOS TARJETAS
-------------------------------------------------
--agregar tarjeta
use BD_Pcbooster_store
go
create or alter procedure sp_AgregarTarjeta 
(
	@IDTARJETA_T bigint, 
	@TITULARTARJETA_T varchar(50),
	@NOMBRETARJETA_T varchar(20),
	@TIPODETARJETA_T bit,
	@NUMEROTARJETA_T bigint,
	@FECHAVENCIMIENTO_T date,
	@CODIGODESEGURIDAD_T int
)
as 
insert into Tarjetas(Id_TarjetaUsuario_t,Titular_Tarjeta_t,Nombre_Tarjeta_t,Tipo_Tarjeta_t,Num_Tarjeta_t,Fecha_Vencimiento_t,Codigo_Seguridad_t)
values (@IDTARJETA_T,@TITULARTARJETA_T,@NOMBRETARJETA_T,@TIPODETARJETA_T,@NUMEROTARJETA_T,@FECHAVENCIMIENTO_T,@CODIGODESEGURIDAD_T)
return 
go
-------------------------------------------------
--eliminar tarjeta
use BD_Pcbooster_store
go
create or alter procedure sp_EliminarTarjeta
(
	@NUMEROTARJETA_T bigint,
	@CODIGODESEGURIDAD_T int,
	@TITULARTARJETA_T varchar
)
as
delete from Tarjetas where right(Num_Tarjeta_t,4) = @NUMEROTARJETA_T and Codigo_Seguridad_t = @CODIGODESEGURIDAD_T and Titular_Tarjeta_t = @TITULARTARJETA_T
return
go


-------------------------------------------------
--TRIGGERS
-------------------------------------------------
--TRIGGERS DETALLE VENTAS
---------------------------------------------------


/* Calcula el total por compra*/

create or alter trigger tr_SumaTotal
on Detalle_Ventas
after insert
as
	begin
	update Ventas set TotalVenta_ve = TotalVenta_ve +  Cantidad_dv*Precio_Unitario_dv from inserted
	where IdUsuario_ve = (select IdUsuario_dv from inserted)
	end
go

-----------------------------------------------------------------------------

/* para habilitar o desabilitar el trigger*/

--DISABLE TRIGGER tr_SumaTotal
--	ON Detalle_Ventas
--	GO

--	ENABLE TRIGGER tr_SumaTotal
--	ON Detalle_Ventas
--	GO

------------------------------------------------------------------------------
/*calcula el stock despues de una compra*/

create or alter trigger tr_ControlStock
on Detalle_Ventas
after insert
as 
	begin 
	update Articulos set Stock_ar = Stock_ar - Cantidad_dv from inserted
	where CodArticulo_ar = (select CodArticulo_dv from inserted)
	end
go


