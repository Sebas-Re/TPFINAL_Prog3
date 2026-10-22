USE BD_Pcbooster_store;

-- ---------------------------------------------------------------------
-- PROCEDIMIENTOS USUARIOS
-- ---------------------------------------------------------------------

-- Agregar usuario
DROP PROCEDURE IF EXISTS sp_AgregarUsuario;
DELIMITER //
CREATE PROCEDURE sp_AgregarUsuario 
(
    IN p_NOMBRE_US VARCHAR(10),
    IN p_CONTRASENIA_US VARCHAR(100),
    IN p_EMAIL_US VARCHAR(30),
    IN p_TELEFONO_US VARCHAR(15),
    IN p_TIPO_US BIT 
)
BEGIN
    INSERT INTO Usuarios(Nombre_us, Contrasenia_us, Email_us, Telefono_us, Tipo_us, Estado_us)
    VALUES (p_NOMBRE_US, p_CONTRASENIA_US, p_EMAIL_US, p_TELEFONO_US, p_TIPO_US, 'true');
END //
DELIMITER ;

-- Eliminar usuario
DROP PROCEDURE IF EXISTS sp_EliminarUsuariosAdmin;
DELIMITER //
CREATE PROCEDURE sp_EliminarUsuariosAdmin
(
    IN p_IdUsuario_us INT
)
BEGIN
    UPDATE Usuarios
    SET Estado_us = 'false'
    WHERE IdUsuario_us = p_IdUsuario_us;
END //
DELIMITER ;

-- Actualizar usuario
DROP PROCEDURE IF EXISTS sp_ModificarUsuario;
DELIMITER //
CREATE PROCEDURE sp_ModificarUsuario
(
    IN p_Id_Usuario_us BIGINT,
    IN p_NOMBRE_US VARCHAR(10),
    IN p_CONTRASENIA_US VARCHAR(100),
    IN p_EMAIL_US VARCHAR(30),
    IN p_TELEFONO_US VARCHAR(15),
    IN p_TIPO_US BIT,
    IN p_Estado_us BIT
)
BEGIN
    UPDATE Usuarios
    SET
        Nombre_us = p_NOMBRE_US,
        Contrasenia_us = p_CONTRASENIA_US,
        Email_us = p_EMAIL_US,
        Telefono_us = p_TELEFONO_US,
        Tipo_us = p_TIPO_US,
        Estado_us = p_Estado_us
    WHERE IdUsuario_us = p_Id_Usuario_us;
END //
DELIMITER ;

-- ---------------------------------------------------------------------
-- PROCEDIMIENTOS ARTICULOS	
-- ---------------------------------------------------------------------

-- Agregar articulo
DROP PROCEDURE IF EXISTS sp_AgregarArticulo;
DELIMITER //
CREATE PROCEDURE sp_AgregarArticulo
(
    IN p_CodArticulo CHAR(4),
    IN p_Categoria BIGINT,
    IN p_Descripcion VARCHAR(300),
    IN p_Stock BIGINT,
    IN p_PrecioUnitario DECIMAL(10,2),
    IN p_ImagenURL VARCHAR(255)
)
BEGIN
    INSERT INTO Articulos(CodArticulo_ar, Categoria_ar, Descripcion_ar, Stock_ar, PrecioUnitario_ar, Imagen)
    VALUES (p_CodArticulo, p_Categoria, p_Descripcion, p_Stock, p_PrecioUnitario, p_ImagenURL);
END //
DELIMITER ;

-- Eliminar articulo
DROP PROCEDURE IF EXISTS sp_EliminarArticulo;
DELIMITER //
CREATE PROCEDURE sp_EliminarArticulo
(
    IN p_CODARTICULO_AR CHAR(4)
)
BEGIN
    UPDATE Articulos
    SET Estado_ar = 0
    WHERE CodArticulo_ar = p_CODARTICULO_AR;
END //
DELIMITER ;

-- Modificar articulo
DROP PROCEDURE IF EXISTS sp_ModificarArticulo;
DELIMITER //
CREATE PROCEDURE sp_ModificarArticulo
(
    IN p_CodArticulo CHAR(4),
    IN p_Categoria BIGINT,
    IN p_Descripcion VARCHAR(300),
    IN p_Stock BIGINT,
    IN p_PrecioUnitario DECIMAL(10,2),
    IN p_ImagenURL VARCHAR(255)
)
BEGIN 
    UPDATE Articulos
    SET
        Categoria_ar = p_CodArticulo,
        Descripcion_ar = p_Descripcion,
        Stock_ar = p_Stock,
        PrecioUnitario_ar = p_PrecioUnitario,
        Imagen = p_ImagenURL
    WHERE CodArticulo_ar = p_CodArticulo;
END //
DELIMITER ;

-- ---------------------------------------------------------------------
-- PROCEDIMIENTOS CATEGORIAS	
-- ---------------------------------------------------------------------

-- Agregar categoria
DROP PROCEDURE IF EXISTS sp_AgregarCategoria;
DELIMITER //
CREATE PROCEDURE sp_AgregarCategoria
(
    IN p_DESCRIPCION_CA VARCHAR(30)
)
BEGIN
    INSERT INTO Categorias(Descripcion_ca, Estado_ca)
    VALUES (p_DESCRIPCION_CA, 1);
END //
DELIMITER ;

-- Eliminar categoria
DROP PROCEDURE IF EXISTS sp_EliminarCategoria;
DELIMITER //
CREATE PROCEDURE sp_EliminarCategoria
(
    IN p_IDCATEGORIA_CA BIGINT
)
BEGIN
    UPDATE Categorias
    SET Estado_ca = 0
    WHERE IdCategoria_ca = p_IDCATEGORIA_CA;
END //
DELIMITER ;

-- Modificar categoria
DROP PROCEDURE IF EXISTS sp_ModificarCategoria;
DELIMITER //
CREATE PROCEDURE sp_ModificarCategoria
(
    IN p_IDCATEGORIA_CA BIGINT,
    IN p_DESCRIPCION_CA VARCHAR(30),
    IN p_ESTADO_CA BIT
)
BEGIN 
    UPDATE Categorias
    SET
        Descripcion_ca = p_DESCRIPCION_CA,
        Estado_ca = p_ESTADO_CA
    WHERE IdCategoria_ca = p_IDCATEGORIA_CA;
END //
DELIMITER ;

-- ---------------------------------------------------------------------
-- PROCEDIMIENTOS VENTAS	
-- ---------------------------------------------------------------------

-- Agregar venta
DROP PROCEDURE IF EXISTS sp_AgregarVenta;
DELIMITER //
CREATE PROCEDURE sp_AgregarVenta
(
    IN p_IDUSUARIO_VE BIGINT,
    IN p_METODODEPAGO_VE BIT,
    IN p_CODIGODESEGURIDAD_VE INT
)
BEGIN
    INSERT INTO Ventas(IdUsuario_ve, Metodo_de_Pago_ve, Codigo_Seguridad_ve, FechaVenta_ve)
    VALUES (p_IDUSUARIO_VE, p_METODODEPAGO_VE, p_CODIGODESEGURIDAD_VE, NOW()); -- Reemplazado getdate() por NOW()
END //
DELIMITER ;

-- ---------------------------------------------------------------------
-- PROCEDIMIENTOS DETALLE VENTAS
-- ---------------------------------------------------------------------

-- Agregar detalle ventas
DROP PROCEDURE IF EXISTS sp_AgregarDetalleVenta;
DELIMITER //
CREATE PROCEDURE sp_AgregarDetalleVenta
(
    IN p_IDDETVENTA_DV BIGINT,
    IN p_IDUSUARIO_DV BIGINT,
    IN p_CODARTICULO_DV CHAR(4),
    IN p_CANTIDAD_DV BIGINT,
    IN p_PRECIOUNITARIO_DV DECIMAL(10,2),
    IN p_DIRECCIONENTREGA_DV TEXT,
    IN p_CONTACTO VARCHAR(20)
)
BEGIN
    INSERT INTO Detalle_Ventas(IdDetVenta_dv, IdUsuario_dv, CodArticulo_dv, Cantidad_dv, Precio_Unitario_dv, Direccion_de_Entrega, Contacto)
    VALUES (p_IDDETVENTA_DV, p_IDUSUARIO_DV, p_CODARTICULO_DV, p_CANTIDAD_DV, p_PRECIOUNITARIO_DV, p_DIRECCIONENTREGA_DV, p_CONTACTO);
END //
DELIMITER ;

-- ---------------------------------------------------------------------
-- PROCEDIMIENTOS REPORTES
-- ---------------------------------------------------------------------

-- Compras por determinado cliente
DROP PROCEDURE IF EXISTS sp_ComprasCliente;
DELIMITER //
CREATE PROCEDURE sp_ComprasCliente
(
    IN p_IdCliente BIGINT
)
BEGIN
    SELECT * FROM Ventas
    WHERE IdUsuario_ve = p_IdCliente;
END //
DELIMITER ;

-- Ventas totales, cant total vendida y stock actual de un determinado articulo
DROP PROCEDURE IF EXISTS sp_MaxArticulo;
DELIMITER //
CREATE PROCEDURE sp_MaxArticulo
(
    IN p_CodArt CHAR(4)
)
BEGIN
    SELECT 
        SUM(Precio_Unitario_dv) AS `VENTAS TOTALES`, 
        SUM(Cantidad_dv) AS `UNIDADES VENDIDAS`, 
        Stock_ar 
    FROM Detalle_Ventas 
    INNER JOIN Articulos ON CodArticulo_ar = CodArticulo_dv
    WHERE CodArticulo_dv = p_CodArt
    GROUP BY Precio_Unitario_dv, Cantidad_dv, Stock_ar, CodArticulo_dv;
END //
DELIMITER ;

-- ---------------------------------------------------------------------
-- PROCEDIMIENTOS TARJETAS
-- ---------------------------------------------------------------------

-- Agregar tarjeta
DROP PROCEDURE IF EXISTS sp_AgregarTarjeta;
DELIMITER //
CREATE PROCEDURE sp_AgregarTarjeta 
(
    IN p_IDTARJETA_T BIGINT, 
    IN p_TITULARTARJETA_T VARCHAR(50),
    IN p_NOMBRETARJETA_T VARCHAR(20),
    IN p_TIPODETARJETA_T BIT,
    IN p_NUMEROTARJETA_T BIGINT,
    IN p_FECHAVENCIMIENTO_T DATE,
    IN p_CODIGODESEGURIDAD_T INT
)
BEGIN 
    INSERT INTO Tarjetas(Id_TarjetaUsuario_t, Titular_Tarjeta_t, Nombre_Tarjeta_t, Tipo_Tarjeta_t, Num_Tarjeta_t, Fecha_Vencimiento_t, Codigo_Seguridad_t)
    VALUES (p_IDTARJETA_T, p_TITULARTARJETA_T, p_NOMBRETARJETA_T, p_TIPODETARJETA_T, p_NUMEROTARJETA_T, p_FECHAVENCIMIENTO_T, p_CODIGODESEGURIDAD_T);
END //
DELIMITER ;

-- Eliminar tarjeta
DROP PROCEDURE IF EXISTS sp_EliminarTarjeta;
DELIMITER //
CREATE PROCEDURE sp_EliminarTarjeta
(
    IN p_NUMEROTARJETA_T BIGINT,
    IN p_CODIGODESEGURIDAD_T INT,
    IN p_TITULARTARJETA_T VARCHAR(50)
)
BEGIN
    DELETE FROM Tarjetas 
    WHERE RIGHT(Num_Tarjeta_t, 4) = p_NUMEROTARJETA_T 
      AND Codigo_Seguridad_t = p_CODIGODESEGURIDAD_T 
      AND Titular_Tarjeta_t = p_TITULARTARJETA_T;
END //
DELIMITER ;


-- ---------------------------------------------------------------------
-- TRIGGERS
-- ---------------------------------------------------------------------

-- Calcula el total por compra
DROP TRIGGER IF EXISTS tr_SumaTotal;
DELIMITER //
CREATE TRIGGER tr_SumaTotal
AFTER INSERT ON Detalle_Ventas
FOR EACH ROW
BEGIN
    UPDATE Ventas 
    SET TotalVenta_ve = TotalVenta_ve + (NEW.Cantidad_dv * NEW.Precio_Unitario_dv)
    WHERE IdUsuario_ve = NEW.IdUsuario_dv; 
END //
DELIMITER ;

-- Calcula el stock despues de una compra
DROP TRIGGER IF EXISTS tr_ControlStock;
DELIMITER //
CREATE TRIGGER tr_ControlStock
AFTER INSERT ON Detalle_Ventas
FOR EACH ROW
BEGIN 
    UPDATE Articulos 
    SET Stock_ar = Stock_ar - NEW.Cantidad_dv
    WHERE CodArticulo_ar = NEW.CodArticulo_dv;
END //
DELIMITER ;
