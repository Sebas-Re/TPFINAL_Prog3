-- Migración 3: tr_SumaTotal sumaba el monto de cada detalle a todas las ventas
-- del usuario (filtraba por IdUsuario_ve). Se reemplaza por uno que suma solo a
-- la venta del detalle y se recalculan los totales que quedaron inflados.

drop trigger if exists tr_SumaTotal;

create trigger tr_SumaTotal
after insert on Detalle_Ventas
begin
	update Ventas set TotalVenta_ve = TotalVenta_ve + new.Cantidad_dv * new.Precio_Unitario_dv
	where IdVentas_ve = new.IdDetVenta_dv;
end;

update Ventas set TotalVenta_ve =
	(select coalesce(sum(Cantidad_dv * Precio_Unitario_dv), 0) from Detalle_Ventas where IdDetVenta_dv = IdVentas_ve);
