CREATE DATABASE BD_Pcbooster_store;
USE BD_Pcbooster_store;

CREATE TABLE Usuarios(
	IdUsuario_us BIGINT NOT NULL AUTO_INCREMENT,
	Nombre_us VARCHAR(10) NOT NULL UNIQUE,
	Contrasenia_us VARCHAR(100) NOT NULL,
	Email_us VARCHAR(30) NOT NULL UNIQUE,
	Telefono_us VARCHAR(15) NULL DEFAULT 'Sin telefono',
	Tipo_us TINYINT(1) NOT NULL DEFAULT 0, -- Si es 1, es admin. Si es 0, es usuario
	Estado_us TINYINT(1) NOT NULL DEFAULT 1,
	CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario_us)
);

CREATE TABLE Categorias(
	IdCategoria_ca BIGINT NOT NULL AUTO_INCREMENT,
	Descripcion_ca VARCHAR(30) NOT NULL UNIQUE,
	Estado_ca TINYINT(1) NOT NULL DEFAULT 1,
	CONSTRAINT PK_Categorias_ca PRIMARY KEY (IdCategoria_ca)
);

CREATE TABLE Articulos(
	CodArticulo_ar CHAR(4) NOT NULL,
	Categoria_ar BIGINT NOT NULL,
	Descripcion_ar VARCHAR(300) NULL UNIQUE,
	Stock_ar BIGINT NULL DEFAULT 0,
	PrecioUnitario_ar DECIMAL(10,2) NOT NULL,
	Imagen VARCHAR(255) NULL DEFAULT 'Sin imagen',
	Estado_ar TINYINT(1) NOT NULL DEFAULT 1,
	CONSTRAINT CK_Articulos_Stock_ar CHECK (Stock_ar >= 0),
	CONSTRAINT CK_Articulos_PrecioUnitario_Ar CHECK (PrecioUnitario_ar > 0),
	CONSTRAINT PK_Articulos PRIMARY KEY (CodArticulo_ar),
	CONSTRAINT Fk_Articulos_Categoria_ar FOREIGN KEY (Categoria_ar) REFERENCES Categorias(IdCategoria_ca)
);

CREATE TABLE Ventas(
	IdVentas_ve BIGINT NOT NULL AUTO_INCREMENT,
	IdUsuario_ve BIGINT NOT NULL,
	TotalVenta_ve DECIMAL(10,2) NOT NULL DEFAULT 0,
	FechaVenta_ve DATETIME NOT NULL DEFAULT NOW(),
	Metodo_de_Pago_ve TINYINT(1) NOT NULL,
	Codigo_Seguridad_ve INT NOT NULL,
	CONSTRAINT CK_Ventas_TotalVentas CHECK (TotalVenta_ve >= 0),
	CONSTRAINT PK_Ventas PRIMARY KEY (IdVentas_ve, IdUsuario_ve),
	CONSTRAINT FK_Ventas_Cuentas_Cod FOREIGN KEY (IdUsuario_ve) REFERENCES Usuarios(IdUsuario_us)
);

CREATE TABLE Detalle_Ventas(
	IdDetVenta_dv BIGINT NOT NULL,
	IdUsuario_dv BIGINT NOT NULL,
	CodArticulo_dv CHAR(4) NOT NULL,
	Cantidad_dv BIGINT NOT NULL,
	Precio_Unitario_dv DECIMAL(10,2) NOT NULL,
	Direccion_de_Entrega TEXT NOT NULL,
	Contacto VARCHAR(20) NOT NULL,
	CONSTRAINT CK_Detalle_Ventas_Cantidad CHECK (Cantidad_dv >= 0),
	CONSTRAINT CK_Detalle_Ventas_Precio_Unitario CHECK (Precio_Unitario_dv >= 0),
	CONSTRAINT PK_Detalle_Ventas PRIMARY KEY (IdDetVenta_dv, CodArticulo_dv),
	CONSTRAINT FK_DetalleVentas_Ventas FOREIGN KEY (IdDetVenta_dv, IdUsuario_dv) REFERENCES Ventas(IdVentas_ve, IdUsuario_ve),
	CONSTRAINT FK_DetalleVentas_Articulos FOREIGN KEY (CodArticulo_dv) REFERENCES Articulos(CodArticulo_ar)
);

CREATE TABLE Tarjetas(
	Id_TarjetaUsuario_t BIGINT NOT NULL,
	Titular_Tarjeta_t VARCHAR(50) NOT NULL,
	Nombre_Tarjeta_t VARCHAR(20) NOT NULL,
	Tipo_Tarjeta_t TINYINT(1) NOT NULL,
	Num_Tarjeta_t BIGINT NOT NULL,
	Fecha_Vencimiento_t DATE NOT NULL,
	Codigo_Seguridad_t INT NOT NULL,
	CONSTRAINT PK_Tarjetas PRIMARY KEY (Id_TarjetaUsuario_t, Codigo_Seguridad_t),
	CONSTRAINT FK_Tarjetas_Usuarios FOREIGN KEY (Id_TarjetaUsuario_t) REFERENCES Usuarios(IdUsuario_us)
);

DELIMITER $$

-- Ventas: la fecha de venta no puede ser futura
CREATE TRIGGER TRG_Ventas_FechaVenta_Insert
BEFORE INSERT ON Ventas
FOR EACH ROW
BEGIN
	IF NEW.FechaVenta_ve > NOW() THEN
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'La fecha de venta no puede ser futura';
	END IF;
END$$

CREATE TRIGGER TRG_Ventas_FechaVenta_Update
BEFORE UPDATE ON Ventas
FOR EACH ROW
BEGIN
	IF NEW.FechaVenta_ve > NOW() THEN
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'La fecha de venta no puede ser futura';
	END IF;
END$$

-- Tarjetas: la fecha de vencimiento debe ser futura
CREATE TRIGGER TRG_Tarjetas_FechaVencimiento_Insert
BEFORE INSERT ON Tarjetas
FOR EACH ROW
BEGIN
	IF NEW.Fecha_Vencimiento_t <= NOW() THEN
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'La fecha de vencimiento debe ser una fecha futura';
	END IF;
END$$

CREATE TRIGGER TRG_Tarjetas_FechaVencimiento_Update
BEFORE UPDATE ON Tarjetas
FOR EACH ROW
BEGIN
	IF NEW.Fecha_Vencimiento_t <= NOW() THEN
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'La fecha de vencimiento debe ser una fecha futura';
	END IF;
END$$

DELIMITER ;