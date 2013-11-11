using System;
using System.Collections.Generic;
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
            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));

            Assert.IsNotNull(juego);
        }

        [TestMethod]
        public void SePuedeJugarUnaFichaNueva()
        {
            var juego = new JuegoDomino();
            juego.Jugadores[0] = new Jugador(new [] {6,6,6,4});
            juego.Jugadores[1] = new Jugador(new [] {6,0,1,1});

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6)); //Primera
            juego.JugarFicha(juego.Jugadores[1], new Ficha(6, 0)); //Segunda

            Assert.IsTrue(juego.Fichas.Count > 1);
        }

        [TestMethod]
        public void SoloSePuedeJugarUnNumeroComunEnTablero()
        {
            var juego = new JuegoDomino();
            juego.Jugadores[0] = new Jugador(new[] { 5, 5, 6, 4 });
            juego.Jugadores[1] = new Jugador(new[] { 3, 4, 1, 1 });
            juego.Jugadores[2] = new Jugador(new[] { 2, 3, 1, 0 });

            juego.JugarFicha(juego.Jugadores[0], new Ficha(5, 5));
            juego.JugarFicha(juego.Jugadores[1], new Ficha(4, 3));
            juego.JugarFicha(juego.Jugadores[2], new Ficha(3, 2));

            Assert.IsTrue(juego.Fichas.Count == 1);
        }

        [TestMethod]
        public void SeJuegaUnaRondaExitosamente()
        {
            var juego = new JuegoDomino();

            juego.Jugadores[0] = new Jugador(new[] { 6, 6, 6, 4 });
            juego.Jugadores[1] = new Jugador(new[] { 3, 4, 6, 0 });
            juego.Jugadores[2] = new Jugador(new[] { 2, 3, 2, 0 });
            juego.Jugadores[3] = new Jugador(new[] { 2, 3, 5, 2 });

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));
            juego.JugarFicha(juego.Jugadores[1], new Ficha(6, 0));
            juego.JugarFicha(juego.Jugadores[2], new Ficha(0, 2));
            juego.JugarFicha(juego.Jugadores[3], new Ficha(5, 2));

            Assert.AreEqual(4, juego.Fichas.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NoSePuedeRepetirFichaYaJugada()
        {
            var juego = new JuegoDomino();

            juego.Jugadores[0] = new Jugador(new[] { 6, 6, 6, 4 });
            juego.Jugadores[1] = new Jugador(new[] { 3, 4, 6, 0 });
            juego.Jugadores[2] = new Jugador(new[] { 2, 3, 2, 0 });
            juego.Jugadores[3] = new Jugador(new[] { 2, 3, 5, 2 });

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));
            juego.JugarFicha(juego.Jugadores[1], new Ficha(6, 0));
            juego.JugarFicha(juego.Jugadores[2], new Ficha(0, 2));
            juego.JugarFicha(juego.Jugadores[3], new Ficha(5, 2));

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));
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

        [TestMethod]
        public void JugadorSoloPuedeJugarFichasQueTengaAsignadas()
        {
            var juego = new JuegoDomino();
            var primerJugador = juego.Jugadores[0];

            //Se sobrescriben las fichas asignadas a un usuario para la prueba
            primerJugador.Fichas = new List<Ficha>
            {
                new Ficha(1,1), new Ficha(0,2)
            };

            juego.JugarFicha(primerJugador, new Ficha(4,5));

            Assert.AreEqual(0, juego.Fichas.Count);

        }

        public void CuandoJugadorPoneUnaFichaSeDebeQuitarDeSuColeccion()
        {
            
        }

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
