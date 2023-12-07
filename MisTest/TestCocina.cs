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

            string testText = "textoGuardado";
            string testPath = ".txt. Invalide path";

            FileManager.Guardar(testText, testPath, false);
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("Juan");

            //act
            int resultado = cocinero.CantPedidosFinalizados;

            //assert
            Assert.AreEqual(resultado,0);
        }
    }
}