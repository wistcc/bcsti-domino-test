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
        private List<Jugador> InicializarJugadores()
        {
            var list = new List<Jugador>
            {
                new Jugador(6,4, 6,6, 5,4, 4,4, 1,2, 3,0, 2,3),
                new Jugador(3,4, 6,0, 0,0, 0,1, 0,2, 0,4, 0,5),
                new Jugador(2,4, 2,0, 1,1, 1,3, 1,4, 1,5, 1,6),
                new Jugador(2,2, 5,2, 2,6, 3,3, 3,5, 3,6, 5,5)
            };

            return list;
        }
            
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
            juego.Jugadores = InicializarJugadores();

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6)); //Primera
            juego.JugarFicha(juego.Jugadores[1], new Ficha(6, 0)); //Segunda

            Assert.IsTrue(juego.Fichas.Count > 1);
        }

        [TestMethod]
        public void SoloSePuedeJugarUnNumeroComunEnTablero()
        {
            var juego = new JuegoDomino();
            juego.Jugadores = InicializarJugadores();

            juego.JugarFicha(juego.Jugadores[0], new Ficha(5, 5));
            juego.JugarFicha(juego.Jugadores[1], new Ficha(4, 3));
            juego.JugarFicha(juego.Jugadores[2], new Ficha(3, 2));

            Assert.IsTrue(juego.Fichas.Count == 1);
        }

        [TestMethod]
        public void SeJuegaUnaRondaExitosamente()
        {
            var juego = new JuegoDomino();
            juego.Jugadores = InicializarJugadores();

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));
            juego.JugarFicha(juego.Jugadores[1], new Ficha(6, 0));
            juego.JugarFicha(juego.Jugadores[2], new Ficha(0, 2));
            juego.JugarFicha(juego.Jugadores[3], new Ficha(5, 2));

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

        [TestMethod]
        public void CuandoJugadorPoneUnaFichaSeDebeQuitarDeSuColeccion()
        {
            var juego = new JuegoDomino();
            var primerJugador = new Jugador(6,0,6,6,5,4,4,4,1,2,3,0,2,3);
            juego.JugarFicha(primerJugador, new Ficha(6,0));
            
            Assert.AreEqual(6, primerJugador.Fichas.Count);
        }

        public void JugadorSoloPuedeJugarSiEsSuTurno()
        {
            
        }

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
