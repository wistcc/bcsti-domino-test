using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csharp.Tests
{
    //Este proyecto usa MsTest para los unit tests. Más información aqui:
    //http://msdn.microsoft.com/en-us/library/ms182532.aspx

    //MsTest es el framework de Testing que viene predeterminado en Visual Studio, 
    //si quieres una alternativa también puedes ver nUnit: http://www.nunit.org/

    [TestClass]
    public class DominoUnitTests
    {
        [TestMethod]
        public void DebePoderComenzarUnJuego()
        {
            var juego = new Csharp.JuegoDomino();
            juego.JugarFicha(Jugadores.Primero, new Ficha(6, 6));

            Assert.IsTrue(juego != null);
        }
    }
}
