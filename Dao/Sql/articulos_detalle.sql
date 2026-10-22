-- Marca, descripción, especificaciones y garantía de los artículos de prueba.
-- Datos inventados (verosímiles) para mostrar el detalle del producto en el catálogo.
-- Especificaciones_ar: una por línea, con el formato "Clave: Valor".
-- Solo completa los artículos que todavía no tienen descripción.

update Articulos set Marca_ar = 'Deepcool', Garantia_ar = 36, DescripcionLarga_ar = 'Fuente de alimentación Deepcool de 700 W con certificación 80 Plus Bronze. Ideal para equipos gamer de gama media con una placa de video dedicada.',
	Especificaciones_ar = 'Potencia: 700 W
Certificación: 80 Plus Bronze
Modularidad: No modular
Ventilador: 120 mm hidráulico
PFC: Activo
Conectores PCIe: 2 x 6+2 pines'
	where CodArticulo_ar = 'fnt1' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Thermaltake', Garantia_ar = 36, DescripcionLarga_ar = 'Fuente de alimentación Thermaltake de 750 W con certificación 80 Plus Gold. Iluminación RGB direccionable y cables planos semi modulares para un armado prolijo.',
	Especificaciones_ar = 'Potencia: 750 W
Certificación: 80 Plus Gold
Modularidad: Semi modular
Ventilador: 120 mm RGB
PFC: Activo
Conectores PCIe: 4 x 6+2 pines'
	where CodArticulo_ar = 'fnt2' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Segotep', Garantia_ar = 24, DescripcionLarga_ar = 'Fuente de alimentación Segotep de 500 W con certificación 80 Plus White. Opción económica y confiable para equipos de oficina o gaming liviano.',
	Especificaciones_ar = 'Potencia: 500 W
Certificación: 80 Plus White
Modularidad: No modular
Ventilador: 120 mm
PFC: Activo
Conectores PCIe: 1 x 6+2 pines'
	where CodArticulo_ar = 'fnt3' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Seasonic', Garantia_ar = 36, DescripcionLarga_ar = 'Fuente de alimentación Seasonic de 500 W con certificación 80 Plus Bronze. La calidad de componentes de Seasonic en un modelo de entrada muy silencioso.',
	Especificaciones_ar = 'Potencia: 500 W
Certificación: 80 Plus Bronze
Modularidad: No modular
Ventilador: 120 mm silencioso
PFC: Activo
Conectores PCIe: 2 x 6+2 pines'
	where CodArticulo_ar = 'fnt4' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASUS', Garantia_ar = 36, DescripcionLarga_ar = 'Fuente de alimentación ASUS de 750 W con certificación 80 Plus Gold. Línea ROG Strix con disipadores ROG Heatsink y modo 0 dB en baja carga.',
	Especificaciones_ar = 'Potencia: 750 W
Certificación: 80 Plus Gold
Modularidad: Full modular
Ventilador: 135 mm Axial-tech
PFC: Activo
Conectores PCIe: 4 x 6+2 pines'
	where CodArticulo_ar = 'fnt5' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASUS', Garantia_ar = 36, DescripcionLarga_ar = 'Fuente de alimentación ASUS de 550 W con certificación 80 Plus Bronze. Línea TUF Gaming con capacitores de grado militar y recubrimiento PCB.',
	Especificaciones_ar = 'Potencia: 550 W
Certificación: 80 Plus Bronze
Modularidad: No modular
Ventilador: 135 mm doble rodamiento
PFC: Activo
Conectores PCIe: 2 x 6+2 pines'
	where CodArticulo_ar = 'fnt6' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Gigabyte', Garantia_ar = 36, DescripcionLarga_ar = 'Fuente de alimentación Gigabyte de 550 W con certificación 80 Plus Bronze. Protecciones OVP, OPP, SCP y UVP para cuidar todos tus componentes.',
	Especificaciones_ar = 'Potencia: 550 W
Certificación: 80 Plus Bronze
Modularidad: No modular
Ventilador: 120 mm
PFC: Activo
Conectores PCIe: 2 x 6+2 pines'
	where CodArticulo_ar = 'fnt7' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'be quiet!', Garantia_ar = 36, DescripcionLarga_ar = 'Fuente de alimentación be quiet! de 600 W con certificación 80 Plus Gold. Pensada para equipos silenciosos: funcionamiento casi inaudible incluso a plena carga.',
	Especificaciones_ar = 'Potencia: 600 W
Certificación: 80 Plus Gold
Modularidad: Semi modular
Ventilador: 135 mm Silent Wings
PFC: Activo
Conectores PCIe: 2 x 6+2 pines'
	where CodArticulo_ar = 'fnt8' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Redragon', Garantia_ar = 36, DescripcionLarga_ar = 'Fuente de alimentación Redragon de 800 W con certificación 80 Plus Gold. Potencia de sobra para placas de video de alta gama y overclock.',
	Especificaciones_ar = 'Potencia: 800 W
Certificación: 80 Plus Gold
Modularidad: Full modular
Ventilador: 140 mm
PFC: Activo
Conectores PCIe: 4 x 6+2 pines'
	where CodArticulo_ar = 'fnt9' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Gamemax', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Mid Tower de Gamemax. Buena ventilación y un diseño gamer a un precio accesible.',
	Especificaciones_ar = 'Formato: Mid Tower
Placas compatibles: ATX, mATX, ITX
Ventiladores incluidos: 1 x 120 mm RGB
Panel lateral: Acrílico
Largo máx. de placa de video: 320 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb1' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Aerocool', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Mid Tower de Aerocool. Frente con iluminación RGB y filtro antipolvo removible.',
	Especificaciones_ar = 'Formato: Mid Tower
Placas compatibles: ATX, mATX, ITX
Ventiladores incluidos: 1 x 120 mm
Panel lateral: Acrílico
Largo máx. de placa de video: 330 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb2' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Deepcool', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Mid Tower de Deepcool. Frente de malla para máximo flujo de aire.',
	Especificaciones_ar = 'Formato: Mid Tower
Placas compatibles: ATX, mATX, ITX
Ventiladores incluidos: 3 x 120 mm ARGB
Panel lateral: Vidrio templado
Largo máx. de placa de video: 370 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb3' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Deepcool', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Mid Tower de Deepcool. Línea MATREXX: interior amplio con gestión de cables detrás de la bandeja.',
	Especificaciones_ar = 'Formato: Mid Tower
Placas compatibles: E-ATX, ATX, mATX
Ventiladores incluidos: 1 x 120 mm
Panel lateral: Vidrio templado
Largo máx. de placa de video: 370 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb4' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Kolink', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Mid Tower de Kolink. Bastion: cuatro ventiladores ARGB y controlador incluidos.',
	Especificaciones_ar = 'Formato: Mid Tower
Placas compatibles: ATX, mATX, ITX
Ventiladores incluidos: 4 x 120 mm ARGB
Panel lateral: Vidrio templado
Largo máx. de placa de video: 350 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb5' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Cooler Master', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Mid Tower de Cooler Master. Línea MasterBox con frente de malla y soporte para radiador de 360 mm.',
	Especificaciones_ar = 'Formato: Mid Tower
Placas compatibles: ATX, mATX, ITX
Ventiladores incluidos: 2 x 120 mm ARGB
Panel lateral: Vidrio templado
Largo máx. de placa de video: 360 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb6' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Cougar', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Mid Tower de Cougar. Diseño angular característico de Cougar con iluminación ARGB.',
	Especificaciones_ar = 'Formato: Mid Tower
Placas compatibles: ATX, mATX, ITX
Ventiladores incluidos: 3 x 120 mm ARGB
Panel lateral: Vidrio templado
Largo máx. de placa de video: 350 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb7' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASUS', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Full Tower de ASUS. Línea ROG: el gabinete premium para builds de máxima gama.',
	Especificaciones_ar = 'Formato: Full Tower
Placas compatibles: E-ATX, ATX, mATX
Ventiladores incluidos: 4 x 140 mm
Panel lateral: Vidrio templado
Largo máx. de placa de video: 450 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb8' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASUS', Garantia_ar = 12, DescripcionLarga_ar = 'Gabinete Mid Tower de ASUS. TUF Gaming: estructura reforzada y frente con detalles en malla.',
	Especificaciones_ar = 'Formato: Mid Tower
Placas compatibles: ATX, mATX, ITX
Ventiladores incluidos: 1 x 120 mm
Panel lateral: Vidrio templado
Largo máx. de placa de video: 365 mm
Puertos frontales: 2 x USB 3.0, audio'
	where CodArticulo_ar = 'gb9' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Seagate', Garantia_ar = 24, DescripcionLarga_ar = 'Disco rígido Seagate de 8 TB pensado para almacenamiento masivo. Ideal para guardar juegos, fotos y backups.',
	Especificaciones_ar = 'Capacidad: 8 TB
Velocidad: 5400 RPM
Caché: 256 MB
Interfaz: SATA III 6 Gb/s
Formato: 3,5"
Uso: Almacenamiento masivo'
	where CodArticulo_ar = 'hdd1' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Seagate', Garantia_ar = 12, DescripcionLarga_ar = 'Disco rígido Seagate de 3 TB pensado para escritorio. Ideal para guardar juegos, fotos y backups.',
	Especificaciones_ar = 'Capacidad: 3 TB
Velocidad: 5400 RPM
Caché: 256 MB
Interfaz: SATA III 6 Gb/s
Formato: 3,5"
Uso: Escritorio'
	where CodArticulo_ar = 'hdd2' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Toshiba', Garantia_ar = 12, DescripcionLarga_ar = 'Disco rígido Toshiba de 4 TB pensado para escritorio. Ideal para guardar juegos, fotos y backups.',
	Especificaciones_ar = 'Capacidad: 4 TB
Velocidad: 7200 RPM
Caché: 128 MB
Interfaz: SATA III 6 Gb/s
Formato: 3,5"
Uso: Escritorio'
	where CodArticulo_ar = 'hdd3' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Western Digital', Garantia_ar = 24, DescripcionLarga_ar = 'Disco rígido Western Digital de 8 TB pensado para nas. Ideal para guardar juegos, fotos y backups.',
	Especificaciones_ar = 'Capacidad: 8 TB
Velocidad: 5640 RPM
Caché: 256 MB
Interfaz: SATA III 6 Gb/s
Formato: 3,5"
Uso: NAS'
	where CodArticulo_ar = 'hdd4' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Western Digital', Garantia_ar = 12, DescripcionLarga_ar = 'Disco rígido Western Digital de 3 TB pensado para escritorio. Ideal para guardar juegos, fotos y backups.',
	Especificaciones_ar = 'Capacidad: 3 TB
Velocidad: 5400 RPM
Caché: 256 MB
Interfaz: SATA III 6 Gb/s
Formato: 3,5"
Uso: Escritorio'
	where CodArticulo_ar = 'hdd5' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Seagate', Garantia_ar = 24, DescripcionLarga_ar = 'Disco rígido Seagate de 14 TB pensado para nas. Ideal para guardar juegos, fotos y backups.',
	Especificaciones_ar = 'Capacidad: 14 TB
Velocidad: 7200 RPM
Caché: 256 MB
Interfaz: SATA III 6 Gb/s
Formato: 3,5"
Uso: NAS'
	where CodArticulo_ar = 'hdd6' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Western Digital', Garantia_ar = 12, DescripcionLarga_ar = 'Disco rígido Western Digital de 6 TB pensado para escritorio. Ideal para guardar juegos, fotos y backups.',
	Especificaciones_ar = 'Capacidad: 6 TB
Velocidad: 5400 RPM
Caché: 256 MB
Interfaz: SATA III 6 Gb/s
Formato: 3,5"
Uso: Escritorio'
	where CodArticulo_ar = 'hdd8' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Western Digital', Garantia_ar = 24, DescripcionLarga_ar = 'Disco rígido Western Digital de 6 TB pensado para videovigilancia. Ideal para guardar juegos, fotos y backups.',
	Especificaciones_ar = 'Capacidad: 6 TB
Velocidad: 5400 RPM
Caché: 256 MB
Interfaz: SATA III 6 Gb/s
Formato: 3,5"
Uso: Videovigilancia'
	where CodArticulo_ar = 'hdd9' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'OLOy', Garantia_ar = 36, DescripcionLarga_ar = 'Memoria RAM OLOy DDR4 de 64 GB (2 x 32 GB) a 3200 MHz. Mejora la fluidez del sistema y de los juegos.',
	Especificaciones_ar = 'Capacidad: 64 GB (2 x 32 GB)
Tipo: DDR4 UDIMM
Frecuencia: 3200 MHz
Latencia: CL16
Voltaje: 1,35 V
Disipador: Aluminio'
	where CodArticulo_ar = 'mr1' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Geil', Garantia_ar = 120, DescripcionLarga_ar = 'Memoria RAM Geil DDR4 de 8 GB a 3200 MHz. Mejora la fluidez del sistema y de los juegos.',
	Especificaciones_ar = 'Capacidad: 8 GB
Tipo: DDR4 UDIMM
Frecuencia: 3200 MHz
Latencia: CL16
Voltaje: 1,35 V
Disipador: Aluminio'
	where CodArticulo_ar = 'mr2' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Team', Garantia_ar = 120, DescripcionLarga_ar = 'Memoria RAM Team DDR4 de 8 GB a 3200 MHz. Mejora la fluidez del sistema y de los juegos.',
	Especificaciones_ar = 'Capacidad: 8 GB
Tipo: DDR4 UDIMM
Frecuencia: 3200 MHz
Latencia: CL16
Voltaje: 1,35 V
Disipador: T-Force Vulcan Z'
	where CodArticulo_ar = 'mr3' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'LeiL', Garantia_ar = 36, DescripcionLarga_ar = 'Memoria RAM LeiL DDR4 de 8 GB a 2666 MHz. Mejora la fluidez del sistema y de los juegos.',
	Especificaciones_ar = 'Capacidad: 8 GB
Tipo: DDR4 UDIMM
Frecuencia: 2666 MHz
Latencia: CL19
Voltaje: 1,35 V
Disipador: Sin disipador'
	where CodArticulo_ar = 'mr6' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Adata', Garantia_ar = 120, DescripcionLarga_ar = 'Memoria RAM Adata DDR4 de 16 GB a 3200 MHz. Mejora la fluidez del sistema y de los juegos.',
	Especificaciones_ar = 'Capacidad: 16 GB
Tipo: DDR4 UDIMM
Frecuencia: 3200 MHz
Latencia: CL16
Voltaje: 1,35 V
Disipador: XPG Gammix'
	where CodArticulo_ar = 'mr7' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'PNY', Garantia_ar = 36, DescripcionLarga_ar = 'Memoria RAM PNY DDR4 de 8 GB a 2666 MHz. Mejora la fluidez del sistema y de los juegos.',
	Especificaciones_ar = 'Capacidad: 8 GB
Tipo: DDR4 UDIMM
Frecuencia: 2666 MHz
Latencia: CL19
Voltaje: 1,35 V
Disipador: Sin disipador'
	where CodArticulo_ar = 'mr8' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Team', Garantia_ar = 120, DescripcionLarga_ar = 'Memoria RAM Team DDR4 de 16 GB a 3600 MHz. Mejora la fluidez del sistema y de los juegos.',
	Especificaciones_ar = 'Capacidad: 16 GB
Tipo: DDR4 UDIMM
Frecuencia: 3600 MHz
Latencia: CL18
Voltaje: 1,35 V
Disipador: T-Force Delta RGB'
	where CodArticulo_ar = 'mr9' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Nvidia', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce GTX 1050 Ti con 4 GB GDDR5. Para jugar en 1080p títulos competitivos sin necesidad de conector de alimentación extra.',
	Especificaciones_ar = 'GPU: GeForce GTX 1050 Ti
Memoria: 4 GB GDDR5
Interfaz: PCI Express 3.0 x16
Salidas: HDMI, DisplayPort, DVI
Consumo: 75 W
Fuente recomendada: 300 W'
	where CodArticulo_ar = 'pdv1' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Nvidia', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce GT 710 con 1 GB DDR3. Placa de bajo perfil para sumar salidas de video a equipos de oficina.',
	Especificaciones_ar = 'GPU: GeForce GT 710
Memoria: 1 GB DDR3
Interfaz: PCI Express 3.0 x16
Salidas: HDMI, VGA, DVI
Consumo: 19 W
Fuente recomendada: 300 W'
	where CodArticulo_ar = 'pdv2' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Nvidia', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce GT 730 con 2 GB GDDR5. Solución económica para multimedia y reproducción 4K.',
	Especificaciones_ar = 'GPU: GeForce GT 730
Memoria: 2 GB GDDR5
Interfaz: PCI Express 3.0 x16
Salidas: HDMI, VGA, DVI
Consumo: 38 W
Fuente recomendada: 300 W'
	where CodArticulo_ar = 'pdv3' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Nvidia', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce RTX 3060 con 8 GB GDDR6. Ray tracing y DLSS para jugar en 1080p y 1440p con todo al máximo.',
	Especificaciones_ar = 'GPU: GeForce RTX 3060
Memoria: 8 GB GDDR6
Interfaz: PCI Express 4.0 x16
Salidas: 3 x DisplayPort, HDMI
Consumo: 170 W
Fuente recomendada: 550 W'
	where CodArticulo_ar = 'pdv4' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Nvidia', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce RTX 3060 Ti con 8 GB GDDR6. Rendimiento de gama alta para 1440p con ray tracing.',
	Especificaciones_ar = 'GPU: GeForce RTX 3060 Ti
Memoria: 8 GB GDDR6
Interfaz: PCI Express 4.0 x16
Salidas: 3 x DisplayPort, HDMI
Consumo: 200 W
Fuente recomendada: 600 W'
	where CodArticulo_ar = 'pdv5' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Nvidia', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce GTX 1660 Ti con 6 GB GDDR6. Excelente relación precio rendimiento para 1080p a altos FPS.',
	Especificaciones_ar = 'GPU: GeForce GTX 1660 Ti
Memoria: 6 GB GDDR6
Interfaz: PCI Express 3.0 x16
Salidas: DisplayPort, HDMI, DVI
Consumo: 120 W
Fuente recomendada: 450 W'
	where CodArticulo_ar = 'pdv6' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASUS', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce RTX 3070 con 8 GB GDDR6. Línea TUF Gaming con triple ventilador Axial-tech y backplate metálico.',
	Especificaciones_ar = 'GPU: GeForce RTX 3070
Memoria: 8 GB GDDR6
Interfaz: PCI Express 4.0 x16
Salidas: 3 x DisplayPort, 2 x HDMI
Consumo: 220 W
Fuente recomendada: 650 W'
	where CodArticulo_ar = 'pdv7' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Nvidia', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce RTX 3060 con 12 GB GDDR6. 12 GB de memoria para juegos exigentes y edición de video.',
	Especificaciones_ar = 'GPU: GeForce RTX 3060
Memoria: 12 GB GDDR6
Interfaz: PCI Express 4.0 x16
Salidas: 3 x DisplayPort, HDMI
Consumo: 170 W
Fuente recomendada: 550 W'
	where CodArticulo_ar = 'pdv8' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Nvidia', Garantia_ar = 36, DescripcionLarga_ar = 'Placa de video GeForce GT 710 con 1 GB DDR3. Pasiva y silenciosa: ideal para HTPC y puestos de trabajo.',
	Especificaciones_ar = 'GPU: GeForce GT 710
Memoria: 1 GB DDR3
Interfaz: PCI Express 3.0 x16
Salidas: HDMI, VGA, DVI
Consumo: 19 W
Fuente recomendada: 300 W'
	where CodArticulo_ar = 'pdv9' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'MSI', Garantia_ar = 36, DescripcionLarga_ar = 'Placa madre MSI B450 para AM4. Línea PRO con BIOS Flash Back para actualizar sin procesador.',
	Especificaciones_ar = 'Socket: AM4
Chipset: B450
Formato: Micro ATX
Memoria: 4 x DDR4 hasta 64 GB
Slots M.2: 1
Red: Gigabit Ethernet'
	where CodArticulo_ar = 'pm1' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASUS', Garantia_ar = 36, DescripcionLarga_ar = 'Placa madre ASUS H410 para LGA 1200. Base sólida para procesadores Intel de 10.ª generación.',
	Especificaciones_ar = 'Socket: LGA 1200
Chipset: H410
Formato: Micro ATX
Memoria: 2 x DDR4 hasta 64 GB
Slots M.2: 1
Red: Gigabit Ethernet'
	where CodArticulo_ar = 'pm2' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'AMD', Garantia_ar = 36, DescripcionLarga_ar = 'Placa madre AMD A68H para FM2+. Combo con procesador AMD A6 incluido: listo para armar un equipo de oficina.',
	Especificaciones_ar = 'Socket: FM2+
Chipset: A68H
Formato: Micro ATX
Memoria: 2 x DDR3 hasta 32 GB
Slots M.2: 0
Red: Gigabit Ethernet'
	where CodArticulo_ar = 'pm3' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASUS', Garantia_ar = 36, DescripcionLarga_ar = 'Placa madre ASUS A320 para AM4. Línea Prime: la opción de entrada para Ryzen.',
	Especificaciones_ar = 'Socket: AM4
Chipset: A320
Formato: Micro ATX
Memoria: 2 x DDR4 hasta 32 GB
Slots M.2: 1
Red: Gigabit Ethernet'
	where CodArticulo_ar = 'pm4' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASUS', Garantia_ar = 36, DescripcionLarga_ar = 'Placa madre ASUS H410 para LGA 1200. Protección contra sobretensiones y componentes de larga duración.',
	Especificaciones_ar = 'Socket: LGA 1200
Chipset: H410
Formato: Micro ATX
Memoria: 2 x DDR4 hasta 64 GB
Slots M.2: 1
Red: Gigabit Ethernet'
	where CodArticulo_ar = 'pm5' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASRock', Garantia_ar = 36, DescripcionLarga_ar = 'Placa madre ASRock A320 para AM4. Compacta y económica, compatible con Ryzen 1000 a 3000.',
	Especificaciones_ar = 'Socket: AM4
Chipset: A320
Formato: Micro ATX
Memoria: 2 x DDR4 hasta 32 GB
Slots M.2: 1
Red: Gigabit Ethernet'
	where CodArticulo_ar = 'pm6' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'ASRock', Garantia_ar = 36, DescripcionLarga_ar = 'Placa madre ASRock B450 para AM4. Soporte para memorias rápidas y overclock en procesadores Ryzen.',
	Especificaciones_ar = 'Socket: AM4
Chipset: B450
Formato: Micro ATX
Memoria: 4 x DDR4 hasta 64 GB
Slots M.2: 1
Red: Gigabit Ethernet'
	where CodArticulo_ar = 'pm8' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Gigabyte', Garantia_ar = 36, DescripcionLarga_ar = 'Placa madre Gigabyte B550 para AM4. Línea AORUS con PCIe 4.0 y VRM reforzado para Ryzen 5000.',
	Especificaciones_ar = 'Socket: AM4
Chipset: B550
Formato: ATX
Memoria: 4 x DDR4 hasta 128 GB
Slots M.2: 2
Red: Gigabit Ethernet'
	where CodArticulo_ar = 'pm9' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Intel', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador Intel Core i5-10400. Gran equilibrio para gaming y productividad. Incluye cooler.',
	Especificaciones_ar = 'Modelo: Core i5-10400
Núcleos / hilos: 6 / 12
Frecuencia: 2,9 GHz (turbo 4,3 GHz)
Socket: LGA 1200
Caché: 12 MB
TDP: 65 W'
	where CodArticulo_ar = 'ps1' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'AMD', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador AMD Ryzen 7 5700X. Ocho núcleos Zen 3 para jugar, transmitir y editar al mismo tiempo.',
	Especificaciones_ar = 'Modelo: Ryzen 7 5700X
Núcleos / hilos: 8 / 16
Frecuencia: 3,4 GHz (turbo 4,6 GHz)
Socket: AM4
Caché: 32 MB
TDP: 65 W'
	where CodArticulo_ar = 'ps2' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'AMD', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador AMD Athlon 3000G. Con gráficos Radeon Vega integrados: no necesita placa de video.',
	Especificaciones_ar = 'Modelo: Athlon 3000G
Núcleos / hilos: 2 / 4
Frecuencia: 3,5 GHz
Socket: AM4
Caché: 4 MB
TDP: 35 W'
	where CodArticulo_ar = 'ps3' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Intel', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador Intel Core i9-10900. Diez núcleos para tareas pesadas de render y gaming de alto rendimiento.',
	Especificaciones_ar = 'Modelo: Core i9-10900
Núcleos / hilos: 10 / 20
Frecuencia: 2,8 GHz (turbo 5,2 GHz)
Socket: LGA 1200
Caché: 20 MB
TDP: 65 W'
	where CodArticulo_ar = 'ps4' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Intel', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador Intel Core i3-10100. Ideal para equipos de oficina y gaming de entrada.',
	Especificaciones_ar = 'Modelo: Core i3-10100
Núcleos / hilos: 4 / 8
Frecuencia: 3,6 GHz (turbo 4,3 GHz)
Socket: LGA 1200
Caché: 6 MB
TDP: 65 W'
	where CodArticulo_ar = 'ps5' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Intel', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador Intel Core i9-10900. Versión box con disipador Intel incluido.',
	Especificaciones_ar = 'Modelo: Core i9-10900
Núcleos / hilos: 10 / 20
Frecuencia: 2,8 GHz (turbo 5,2 GHz)
Socket: LGA 1200
Caché: 20 MB
TDP: 65 W'
	where CodArticulo_ar = 'ps6' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'AMD', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador AMD Ryzen 7 3800XT. Frecuencias más altas de la serie 3000. No incluye cooler.',
	Especificaciones_ar = 'Modelo: Ryzen 7 3800XT
Núcleos / hilos: 8 / 16
Frecuencia: 3,9 GHz (turbo 4,7 GHz)
Socket: AM4
Caché: 32 MB
TDP: 105 W'
	where CodArticulo_ar = 'ps7' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Intel', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador Intel Core i9-10900KA. Edición especial desbloqueada para overclock. No incluye cooler.',
	Especificaciones_ar = 'Modelo: Core i9-10900KA
Núcleos / hilos: 10 / 20
Frecuencia: 3,7 GHz (turbo 5,3 GHz)
Socket: LGA 1200
Caché: 20 MB
TDP: 125 W'
	where CodArticulo_ar = 'ps8' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Intel', Garantia_ar = 36, DescripcionLarga_ar = 'Procesador Intel Core i3-9100. Procesador de 9.ª generación con gráficos Intel UHD 630.',
	Especificaciones_ar = 'Modelo: Core i3-9100
Núcleos / hilos: 4 / 4
Frecuencia: 3,6 GHz (turbo 4,2 GHz)
Socket: LGA 1151
Caché: 6 MB
TDP: 65 W'
	where CodArticulo_ar = 'ps9' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Kingston', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Kingston de 1 TB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 1 TB
Formato: M.2 2280
Interfaz: PCIe 3.0 x4 NVMe
Lectura secuencial: 3500 MB/s
Escritura secuencial: 2100 MB/s
Durabilidad: 600 TBW'
	where CodArticulo_ar = 'sdd1' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Team', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Team de 1 TB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 1 TB
Formato: 2,5"
Interfaz: SATA III
Lectura secuencial: 560 MB/s
Escritura secuencial: 500 MB/s
Durabilidad: 560 TBW'
	where CodArticulo_ar = 'sdd2' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Team', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Team de 128 GB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 128 GB
Formato: 2,5"
Interfaz: SATA III
Lectura secuencial: 500 MB/s
Escritura secuencial: 400 MB/s
Durabilidad: 60 TBW'
	where CodArticulo_ar = 'sdd3' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Adata', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Adata de 1,92 TB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 1,92 TB
Formato: 2,5"
Interfaz: SATA III
Lectura secuencial: 560 MB/s
Escritura secuencial: 520 MB/s
Durabilidad: 1000 TBW'
	where CodArticulo_ar = 'sdd4' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Adata', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Adata de 1 TB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 1 TB
Formato: M.2 2280
Interfaz: PCIe 3.0 x4 NVMe
Lectura secuencial: 2100 MB/s
Escritura secuencial: 1500 MB/s
Durabilidad: 640 TBW'
	where CodArticulo_ar = 'sdd5' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Gigabyte', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Gigabyte de 1 TB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 1 TB
Formato: M.2 2280
Interfaz: PCIe 3.0 x4 NVMe
Lectura secuencial: 2500 MB/s
Escritura secuencial: 2100 MB/s
Durabilidad: 600 TBW'
	where CodArticulo_ar = 'sdd6' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Team', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Team de 500 GB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 500 GB
Formato: 2,5"
Interfaz: SATA III
Lectura secuencial: 550 MB/s
Escritura secuencial: 500 MB/s
Durabilidad: 300 TBW'
	where CodArticulo_ar = 'sdd7' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Western Digital', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Western Digital de 2 TB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 2 TB
Formato: M.2 2280
Interfaz: PCIe 4.0 x4 NVMe
Lectura secuencial: 5150 MB/s
Escritura secuencial: 4850 MB/s
Durabilidad: 1200 TBW'
	where CodArticulo_ar = 'sdd8' and DescripcionLarga_ar is null;
update Articulos set Marca_ar = 'Adata', Garantia_ar = 36, DescripcionLarga_ar = 'Disco sólido Adata de 1 TB. Reduce los tiempos de arranque y de carga de forma notable frente a un disco rígido.',
	Especificaciones_ar = 'Capacidad: 1 TB
Formato: M.2 2280
Interfaz: PCIe 4.0 x4 NVMe
Lectura secuencial: 5000 MB/s
Escritura secuencial: 4500 MB/s
Durabilidad: 740 TBW'
	where CodArticulo_ar = 'sdd9' and DescripcionLarga_ar is null;
