using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            //arrange

            //act

            //assert
            Assert.ThrowsException<FileManagerException>(() =>
            {
                throw new FileManagerException("Esto paso");
            });
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("Juan");

            //act
            int resultado = cocinero.CantPedidosFinalizados;

            //assert
            Assert.AreEqual(0, resultado);
        }
    }
}