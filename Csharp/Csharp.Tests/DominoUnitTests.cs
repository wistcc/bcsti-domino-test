using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        #region Helpers

        private List<Jugador> InicializarJugadores()
        {
            var list = new List<Jugador>
            {
                new Jugador(6,4, 6,6, 5,4, 4,4, 1,2, 3,0, 0,5),
                new Jugador(3,4, 6,0, 0,0, 0,1, 5,6, 0,4, 3,2),
                new Jugador(2,4, 2,0, 1,1, 1,3, 1,4, 1,5, 1,6),
                new Jugador(2,2, 5,2, 2,6, 3,3, 3,5, 3,6, 5,5)
            };

            return list;
        }

        private List<Jugador> InicializarJugadoresConTranque()
        {
            var list = new List<Jugador>
            {
                new Jugador(6,0, 6,1, 6,2, 6,3, 6,4, 6,5, 1,2),
                new Jugador(5,5, 5,0, 5,2, 5,4, 3,5, 1,5, 3,2),
                new Jugador(2,4, 2,0, 1,1, 1,3, 1,4, 0,4, 6,6),
                new Jugador(2,2, 0,0, 0,1, 3,3, 3,0, 4,4, 3,4)
            };

            return list;
        }
        private List<Jugador> InicializarJugadoresConPasoAlIniciar()
        {
            var list = new List<Jugador>
            {
                new Jugador(6,0, 6,1, 6,2, 6,3, 6,4, 6,5, 1,2),
                new Jugador(5,5, 2,4, 5,2, 5,4, 3,5, 1,5, 3,2),
                new Jugador(5,0, 2,0, 1,1, 1,3, 1,4, 0,4, 6,6),
                new Jugador(2,2, 0,0, 0,1, 3,3, 3,0, 4,4, 3,4)
            };

            return list;
        }

        private void SimularJuego(JuegoDomino juego)
        {
            while (juego.Jugadores[0].Fichas.Count > 0 &&
                   juego.Jugadores[1].Fichas.Count > 0 &&
                   juego.Jugadores[2].Fichas.Count > 0 &&
                   juego.Jugadores[3].Fichas.Count > 0)
            {
                var jugador = juego.Jugadores[juego.TurnoActual];

                if (juego.Fichas.Count == 0)
                {
                    juego.JugarFicha(jugador, jugador.Fichas[0]);
                }
                else
                {
                    var fichaInicial = juego.Fichas.First();
                    var fichaFinal = juego.Fichas.Last();

                    var puedeJugar = false;
                    foreach (var ficha in jugador.Fichas)
                    {
                        if (fichaInicial.PuedeJugarA(ficha) || fichaFinal.PuedeJugarB(ficha))
                        {
                            juego.JugarFicha(jugador, ficha);
                            puedeJugar = true;
                            break;
                        }
                    }

                    if (!puedeJugar)
                    {
                        if (juego.Fichas.Count == 1)
                        {
                            var turnoSiguiente = (juego.TurnoActual + 1 == 4) ? 0 : juego.TurnoActual + 1;
                            var equipoContrario = (int)juego.Jugadores[turnoSiguiente].Equipo;

                            juego.Score[equipoContrario].Add(juego.Penalidad);
                        }
                        juego.PasarJuego(jugador);
                    }
                }

                Trace.WriteLine("");
                juego.DibujarTablero();
            }
        }

        #endregion


        [TestMethod]
        public void DebePoderComenzarUnJuego()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };
            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));

            Assert.IsNotNull(juego);
        }

        [TestMethod]
        public void SePuedeJugarUnaFichaNueva()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6)); //Primera
            juego.JugarFicha(juego.Jugadores[1], new Ficha(6, 0)); //Segunda

            Assert.IsTrue(juego.Fichas.Count > 1);
        }

        [TestMethod]
        public void SePuedeJugarUnaFichaYEspecificarElOrden()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };

            juego.JugarFicha(0, new Ficha(6, 4));
            juego.JugarFicha(1, new Ficha(4, 0));
            juego.JugarFicha(2, new Ficha(0, 2));
            juego.JugarFicha(3, new Ficha(2, 6), 6);

            Assert.IsTrue(juego.Fichas.First().Valor.A == 6 && juego.Fichas.Last().Valor.B == 6);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SoloSePuedeJugarMovidasValidas()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));
            juego.JugarFicha(juego.Jugadores[1], new Ficha(4, 3));

            Assert.IsTrue(juego.Fichas.Count == 1);
        }

        [TestMethod]
        public void SeJuegaUnaRondaExitosamente()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };

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
        [ExpectedException(typeof(ArgumentException))]
        public void JugadorSoloPuedeJugarFichasQueTengaAsignadas()
        {
            var juego = new JuegoDomino();
            var primerJugador = juego.Jugadores[0];

            //Se sobrescriben las fichas asignadas a un usuario para la prueba
            primerJugador.Fichas = new List<Ficha>
            {
                new Ficha(1,1), new Ficha(0,2)
            };

            juego.JugarFicha(primerJugador, new Ficha(4, 5));

        }

        [TestMethod]
        public void CuandoJugadorPoneUnaFichaSeDebeQuitarDeSuColeccion()
        {
            var juego = new JuegoDomino();
            juego.Jugadores[0] = new Jugador(6, 0, 6, 6, 5, 4, 4, 4, 1, 2, 3, 0, 2, 3);
            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 0));

            Assert.AreEqual(6, juego.Jugadores[0].Fichas.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void JugadorSoloPuedeJugarSiEsSuTurno()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };

            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));
            juego.JugarFicha(juego.Jugadores[0], new Ficha(4, 4));
        }

        [TestMethod]
        public void JugadorPuedePasar()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };

            juego.JugarFicha(juego.Jugadores[0], new Ficha(3, 0));
            juego.JugarFicha(juego.Jugadores[1], new Ficha(3, 2));
            juego.JugarFicha(juego.Jugadores[2], new Ficha(2, 0));
            juego.PasarJuego(juego.Jugadores[3]);

            Assert.AreEqual(0, juego.TurnoActual);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void JugadorNoPuedePasarConFichasDisponibles()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };

            juego.JugarFicha(juego.Jugadores[0], new Ficha(3, 0));
            juego.JugarFicha(juego.Jugadores[1], new Ficha(0, 6));
            juego.JugarFicha(juego.Jugadores[2], new Ficha(6, 1));
            juego.PasarJuego(juego.Jugadores[3]);
        }

        [TestMethod]
        public void SiUnJugadorSeQuedaSinFichasSeTerminaLaPartida()
        {
            var juego = new JuegoDomino { Jugadores = InicializarJugadores() };

            SimularJuego(juego);

            Assert.IsTrue(juego.TurnoActual == -1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SiSeTerminaLaPartidaNosePuedeJugar()
        {
            var juego = new JuegoDomino { Jugadores = InicializarJugadores() };
            SimularJuego(juego);

            var otroJugadorConFichas = juego.Jugadores.First(f => f.Fichas.Any());
            juego.JugarFicha(otroJugadorConFichas, otroJugadorConFichas.Fichas.First());
        }

        [TestMethod]
        public void ElJuegoCalculaElScoreDeQuienGano()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadores()
            };

            var scoreOriginal = juego.Score.Select(f => f.Sum()).ToList();
            SimularJuego(juego);
            var scoreFinal = juego.Score;

            Assert.IsTrue(scoreOriginal[0] != scoreFinal[0].Sum()
                       || scoreOriginal[1] != scoreFinal[1].Sum());
        }

        [TestMethod]
        public void ElJuegoDetectaCuandoHayUnTranque()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadoresConTranque()
            };

            SimularJuego(juego);

            Assert.IsTrue(juego.TurnoActual == -1 && juego.Jugadores.Count(f => f.Fichas.Any()) == 4);
        }


        public void SiHayUnTranqueElJuegoDecideElGanador()
        { }

        public void SePuedeJugarMasDeUnaPartida()
        { }

        public void ElJugadorQueDominoLaPartidaAnteriorDebeComenzarLaSiguiente()
        { }

        public void LaPrimeraPartidaDebeComenzarConDobleSeis()
        { }

        public void ElEquipoCon200PuntosGanaElJuego()
        { }

        [TestMethod]
        public void PenalidadSiElSegundoJugadorNoTieneFichas()
        {
            var juego = new JuegoDomino
            {
                Jugadores = InicializarJugadoresConPasoAlIniciar()
            };

            SimularJuego(juego);

            var scoreSinPenalidadDeLaPartida = juego.Jugadores.Sum(j => j.Fichas.Sum(f => f.Puntos));

            Assert.IsTrue(scoreSinPenalidadDeLaPartida + juego.Penalidad == juego.Score[0].Sum()
                          || scoreSinPenalidadDeLaPartida + juego.Penalidad == juego.Score[1].Sum());
        }

        public void PenalidadSegundoJugadorNoAplicaSiTerceroNoTiene()
        { }

        public void PenalidadSiTresJugadoresPasanConsecutivamente()
        { }

        public void PuntosAdicionalesConKapikua()
        { }
    }
}
