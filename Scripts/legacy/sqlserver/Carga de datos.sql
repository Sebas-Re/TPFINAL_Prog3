use BD_Pcbooster_store
go


--------------DATOS-------------------
---------------------------------------------------------------------------
--Carga de datos prueba usuario--
---------------------------------------------------------------------------
insert into Usuarios (Nombre_us,Contraseña_us,Email_us,Telefono_us,Tipo_us)
select 'Nom1','ctr1','ema1','12345','0' union
select 'Nom2','ctr2','ema2','23456','0' union
select 'Nom3','ctr3','ema3','67891','0' union
select 'Nom4','ctr4','ema4','23456','0' union
select 'Nom5','ctr5','ema5','67849','0' union
select 'admDiamela','admpsw1','Diamela@adm.com','111222333','1' union
select 'admSebasti','admpsw2','Sebastian@adm.com','444555666','1' union
select 'admNicolas','admpsw2','Nicolas@adm.com','777888999','1' union
select 'admAmador','admpsw2','Amador@adm.com','777444111','1'
go
---------------------------------------------------------------------------
--Carga de datos prueba Categorias--
---------------------------------------------------------------------------

insert into Categorias (Descripcion_ca)
select 'Placa de video' union
select 'Memoria ram' union
select 'Procesador' union
select 'Hdd' union
select 'Ssd' union
select 'Fuente' union
select 'Placa madre' union
select 'Gabinete'
go
---------------------------------------------------------------------------
--Carga de datos prueba Articulos--
---------------------------------------------------------------------------
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
--Fuentes
select 'fnt1','1','Fuente Deepcool 700W','100','7300','~/Imagenes_ar/Fuente/fnt1.png' union
select 'fnt2','1','Fuente Thermaltake 750W','150','14890','~/Imagenes_ar/Fuente/fnt2.png' union
select 'fnt3','1','Fuente Segotep 500W','60','1600','~/Imagenes_ar/Fuente/fnt3.png' union
select 'fnt4','1','Fuente Seasonic 500W','70','8100','~/Imagenes_ar/Fuente/fnt4.png' union
select 'fnt5','1','Fuente ASUS 750W','100','27970','~/Imagenes_ar/Fuente/fnt5.png' union
select 'fnt6','1','Fuente ASUSF 550W','65','10690','~/Imagenes_ar/Fuente/fnt6.png' union
select 'fnt7','1','Fuente Gigabyte 550W','90','6660','~/Imagenes_ar/Fuente/fnt7.png' union
select 'fnt8','1','Fuente Be Quiet! 600W','150','8590','~/Imagenes_ar/Fuente/fnt8png' union
select 'fnt9','1','Fuente Redragon 800W','105','13790','~/Imagenes_ar/Fuente/fnt9.png'
go
--Gabinetes
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
select 'gb1','2','Gabinete Gamemax','10','3500','~/Imagenes_ar/Gabinete/gb1.png' union
select 'gb2','2','Gabinete Aerocool','100','4750','~/Imagenes_ar/Gabinete/gb2.png' union
select 'gb3','2','Gabinete Deepcool','50','10310','~/Imagenes_ar/Gabinete/gb3.png' union
select 'gb4','2','Gabinete Deepcool MATREXX','80','9870','~/Imagenes_ar/Gabinete/gb4.png' union
select 'gb5','2','Gabinete Kolink Bastion','45','7850','~/Imagenes_ar/Gabinete/gb5.png' union
select 'gb6','2','Gabinete Cooler Master','90','11650','~/Imagenes_ar/Gabinete/gb6.png' union
select 'gb7','2','Gabinete Cougar','30','11780','~/Imagenes_ar/Gabinete/gb7.png' union
select 'gb8','2','Gabinete ASUS','5','55190','~/Imagenes_ar/Gabinete/gb8.png' union
select 'gb9','2','Gabinete ASUS TUF','50','13270','~/Imagenes_ar/Gabinete/gb9.png'
go
--HDD
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
select 'hdd1','3','Disco Rigido Seagate 8TB','30','62200','~/Imagenes_ar/HDD/hdd1.png' union
select 'hdd2','3','Disco Rigido Seagate 3TB','50','11470','~/Imagenes_ar/HDD/hdd2.png' union
select 'hdd3','3','Disco Rigido Toshiba 4TB','100','12000','~/Imagenes_ar/HDD/hdd3.png' union
select 'hdd4','3','Disco Rigido WD 8TB','40','41940','~/Imagenes_ar/HDD/hdd4.png' union
select 'hdd5','3','Disco Rigido WD 3TB','110','11150','~/Imagenes_ar/HDD/hdd5.png' union
select 'hdd6','3','Disco Rigido Seagate 14TB','12','52390','~/Imagenes_ar/HDD/hdd6.png' union
select 'hdd8','3','Disco Rigido WD 6TB','60','20980','~/Imagenes_ar/HDD/hdd8.png' union
select 'hdd9','3','Disco Rigido WD 6TB Purple','90','21220','~/Imagenes_ar/HDD/hdd9.png'
go
--Memorias ram
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
select 'mr1','4','Memoria OLOy DDR4 64GB','20','34730','~/Imagenes_ar/Memoria/mr1.png' union
select 'mr2','4','Memoria Geil DDR4 8GB','100','5900','~/Imagenes_ar/Memoria/mr2.png'  union
select 'mr3','4','Memoria Team DDR4 8GB','200','4973','~/Imagenes_ar/Memoria/mr3.png'  union
select 'mr6','4','Memoria LeiL DDR4 8GB','150','5100','~/Imagenes_ar/Memoria/mr6.png'  union
select 'mr7','4','Memoria Adata DDR4 16GB','70','9930','~/Imagenes_ar/Memoria/mr7.png'  union
select 'mr8','4','Memoria PNY DDR4 8GB','80','5488','~/Imagenes_ar/Memoria/mr8.png'  union
select 'mr9','4','Memoria Team DDR4 16GB','15','19990','~/Imagenes_ar/Memoria/mr9.png'
go
--Placas de video
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
select 'pdv1','5','Placa de video Nvidia GTX 1050 4GB','50','49499','~/Imagenes_ar/Placa de video/pdv1.png' union
select 'pdv2','5','Placa de video Nvidia GeForce 1GB','100','25000','~/Imagenes_ar/Placa de video/pdv2.png' union
select 'pdv3','5','Placa de video GeForce 2gb','100','18499','~/Imagenes_ar/Placa de video/pdv3.png' union
select 'pdv4','5','Placa de video GeForce RTX 3060 8GB','5','189000','~/Imagenes_ar/Placa de video/pdv4.png' union
select 'pdv5','5','Placa De Video Nvidia Rtx 3060ti','5','159000','~/Imagenes_ar/Placa de video/pdv5.png' union
select 'pdv6','5','Placa De Video GTX 1660 Ti 6GB','10','120000','~/Imagenes_ar/Placa de video/pdv6.png' union
select 'pdv7','5','Placa De Video ASUS RTX 3010 8GB','10','345000','~/Imagenes_ar/Placa de video/pdv7.png' union
select 'pdv8','5','Placa De Video RTX 3060 12GB','15','190000','~/Imagenes_ar/Placa de video/pdv8.png' union
select 'pdv9','5','Placa De Video GeForce 1GB','50','5930','~/Imagenes_ar/Placa de video/pdv9.png' 
go
--Placas madre
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
select 'pm1','6','Placa Madre Msi','70','14500','~/Imagenes_ar/Placa madre/pm1.png' union
select 'pm2','6','Placa Madre Asus','120','7900','~/Imagenes_ar/Placa madre/pm2.png' union
select 'pm3','6','Placa Madre Y Procesador Amd A6','200','23555','~/Imagenes_ar/Placa madre/pm3.png' union
select 'pm4','6','Placa Madre Asus Prime','200','6900','~/Imagenes_ar/Placa madre/pm4.png' union
select 'pm5','6','Placa Madre Asus H410m-e','50','17500','~/Imagenes_ar/Placa madre/pm5.png' union
select 'pm6','6','Placa Madre Asrock','100','6390','~/Imagenes_ar/Placa madre/pm6.png' union
select 'pm8','6','Placa Madre Asrockk','100','6450','~/Imagenes_ar/Placa madre/pm8.png' union
select 'pm9','6','Placa Madre Gigabyte','40','21440','~/Imagenes_ar/Placa madre/pm9.png'
go
--Procesadores
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
select 'ps1','7','Procesador Intel Core i5','73','32100','~/Imagenes_ar/Procesador/ps1.png' union
select 'ps2','7','Procesador gamer AMD Ryzen 7','50','61700','~/Imagenes_ar/Procesador/ps2.png' union
select 'ps3','7','Procesador AMD Athlon','150','15900','~/Imagenes_ar/Procesador/ps3.png' union
select 'ps4','7','Procesador gamer Intel Core i9','40','60899','~/Imagenes_ar/Procesador/ps4.png' union
select 'ps5','7','Procesador Intel Core i3','90','89000','~/Imagenes_ar/Procesador/ps5.png' union 
select 'ps6','7','Procesador Intel Core i9 10900','50','58980','~/Imagenes_ar/Procesador/ps6.png' union
select 'ps7','7','Procesador AMD Ryzen 7 3800XT','15','51860','~/Imagenes_ar/Procesador/ps7.png' union
select 'ps8','7','Procesador Intel Core i9 10900 KA 5.3GHz','5','71638','~/Imagenes_ar/Procesador/ps8.png' union
select 'ps9','7','Procesador Intel Core i3 9100','150','19500','~/Imagenes_ar/Procesador/ps9.png'
go
--SSD
insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen)
select 'sdd1','8','Disco Solido SSD M.2 1TB','150','28405','~/Imagenes_ar/SDD/sdd1.png' union
select 'sdd2','8','Disco Solido SSD Team 1TB','100','15910','~/Imagenes_ar/SDD/sdd1.png' union
select 'sdd3','8','Disco Solido SSD Team 128GB','220','2630','~/Imagenes_ar/SDD/sdd1.png' union
select 'sdd4','8','Disco Solido SSD Adata 1.92TB','105','22674','~/Imagenes_ar/SDD/sdd1.png' union
select 'sdd5','8','Disco Solidos SSD M.2 ADATA 1TB','45','14100','~/Imagenes_ar/SDD/sdd1.png' union
select 'sdd6','8','Disco Solido SSD M.2 Gigabyte 1TB','80','17451','~/Imagenes_ar/SDD/sdd1.png' union
select 'sdd7','8','Disco Solido SSD Team 500GB','100','11849','~/Imagenes_ar/SDD/sdd1.png' union
select 'sdd8','8','Disco Solido SSD M.2 WD 2TB','20','43510','~/Imagenes_ar/SDD/sdd1.png' union
select 'sdd9','8','Disco Solido SSD M.2 ADATA 1TB','150','20140','~/Imagenes_ar/SDD/sdd1.png' 
go




