using Entidades;
using Xunit;

namespace Negocio.Tests
{
    public class CarritoTests
    {
        [Fact]
        public void ArrancaVacio()
        {
            var carrito = new Carrito();

            Assert.True(carrito.Vacio);
            Assert.Equal(0m, carrito.Total);
            Assert.Equal(0, carrito.CantidadUnidades);
        }

        [Fact]
        public void AgregarElMismoArticuloSumaCantidad()
        {
            var carrito = new Carrito();

            Assert.True(carrito.Agregar("gb1", "Gabinete Gamemax", 3500m, stock: 10));
            Assert.True(carrito.Agregar("GB1", "Gabinete Gamemax", 3500m, stock: 10));
            Assert.True(carrito.Agregar("fnt1", "Fuente Deepcool 700W", 7300m, stock: 10));

            Assert.Equal(2, carrito.Items.Count);
            Assert.Equal(2, carrito.Items[0].Cantidad);
            Assert.Equal(3, carrito.CantidadUnidades);
            Assert.Equal(14300m, carrito.Total);
        }

        [Fact]
        public void NoPermiteSuperarElStock()
        {
            var carrito = new Carrito();

            Assert.True(carrito.Agregar("gb8", "Gabinete ASUS", 55190m, stock: 1));
            Assert.False(carrito.Agregar("gb8", "Gabinete ASUS", 55190m, stock: 1));
            Assert.False(carrito.Agregar("sin", "Sin stock", 10m, stock: 0));

            Assert.Equal(1, carrito.CantidadUnidades);
        }

        [Fact]
        public void QuitarYVaciar()
        {
            var carrito = new Carrito();
            carrito.Agregar("gb1", "Gabinete Gamemax", 3500m, 10);
            carrito.Agregar("fnt1", "Fuente Deepcool 700W", 7300m, 10);

            Assert.True(carrito.Quitar("gb1"));
            Assert.False(carrito.Quitar("gb1"));
            Assert.Equal(7300m, carrito.Total);

            carrito.Vaciar();
            Assert.True(carrito.Vacio);
        }
    }
}
