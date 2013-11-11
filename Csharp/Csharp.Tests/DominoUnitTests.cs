using System;
using System.Runtime;
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
            var juego = new JuegoDomino();
            juego.JugarFicha(Jugadores.Primero, new Ficha(6, 6));

            Assert.IsNotNull(juego);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ElJuegoDebeComenzarConUnDoble()
        {
            var juego = new JuegoDomino();
            juego.JugarFicha(Jugadores.Primero, new Ficha(3, 4));

            Assert.IsNull(juego.Fichas);
        }

        [TestMethod]
        public void SePuedeJugarUnaFichaNueva()
        {
            var juego = new JuegoDomino();
            juego.JugarFicha(Jugadores.Primero, new Ficha(6,6)); //Primera
            juego.JugarFicha(Jugadores.Segundo, new Ficha(6,0)); //Segunda

            Assert.IsTrue(juego.Fichas.Count > 1);
        }

        [TestMethod]
        public void SoloSePuedeJugarUnNumeroComunEnTablero()
        {
            var juego = new JuegoDomino();
            juego.JugarFicha(Jugadores.Primero, new Ficha(5,5));
            juego.JugarFicha(Jugadores.Segundo, new Ficha(4,3));
            juego.JugarFicha(Jugadores.Tercero, new Ficha(3,2));

            Assert.IsTrue(juego.Fichas.Count == 1);
        }

        [TestMethod]
        public void SeJuegaUnaRondaExitosamente()
        {
            var juego = new JuegoDomino();

            juego.JugarFicha(Jugadores.Primero, new Ficha(6, 6));
            juego.JugarFicha(Jugadores.Segundo, new Ficha(6, 0));   
            juego.JugarFicha(Jugadores.Tercero, new Ficha(0,2)); 
            juego.JugarFicha(Jugadores.Cuarto, new Ficha(5,2 ));

            Assert.AreEqual(4, juego.Fichas.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NoSePuedeRepetirFichaYaJugada()
        {
            var juego = new JuegoDomino();
            juego.JugarFicha(Jugadores.Primero, new Ficha(6, 6));
            juego.JugarFicha(Jugadores.Segundo, new Ficha(6, 0));
            juego.JugarFicha(Jugadores.Tercero, new Ficha(0, 2));
            juego.JugarFicha(Jugadores.Cuarto, new Ficha(5, 2));
            
            juego.JugarFicha(Jugadores.Primero, new Ficha(6,0));
            Assert.AreEqual(4, juego.Fichas.Count);
        }

        [TestMethod]
        public void DebenHaberCuatroJugadores()
        {
            var juego = new JuegoDomino();

            Assert.AreEqual(4, juego.Jugadores.Count, "No hay cuatro jugadores");
        }

        [TestMethod]
        public void LosJugadoresTienenFichasAsignadasAlComenzarElJuego()
        {
            var juego = new JuegoDomino();

            Assert.AreEqual(7, juego.Jugadores[0].Fichas.Count);
            Assert.AreEqual(7, juego.Jugadores[1].Fichas.Count);
            Assert.AreEqual(7, juego.Jugadores[2].Fichas.Count);
            Assert.AreEqual(7, juego.Jugadores[3].Fichas.Count);
        }

        public void JugadorSoloPuedeJugarFichasQueTengaAsignadas()
        {}

        public void JugadorSoloPuedeJugarSiEsSuTurno()
        {}

        public void JugadorPuedePasar()
        {}

        public void JugadorNoPuedePasarConFichasDisponibles()
        {}

        public void SiUnJugadorSeQuedaSinFichasGanaLaPartida()
        {}

        public void ElJuegoCalculaElScoreDeQuienGano()
        {}

        public void ElJuegoDetectaCuandoHayUnTranque()
        {}

        public void SiHayUnTranqueElJuegoDecideElGanador()
        {}

        public void SePuedeJugarMasDeUnaPartida()
        {}

        public void ElJugadorQueDominoLaPartidaAnteriorDebeComenzarLaSiguiente()
        {}

        public void LaPrimeraPartidaDebeComenzarConDobleSeis()
        {}

        public void ElEquipoCon200PuntosGanaElJuego()
        {}

        public void PenalidadSiElSegundoJugadorNoTieneFichas()
        {}

        public void PenalidadSegundoJugadorNoAplicaSiTerceroNoTiene()
        {}

        public void PenalidadSiTresJugadoresPasanConsecutivamente()
        {}

        public void PuntosAdicionalesConKapikua()
        {}
    }
}
