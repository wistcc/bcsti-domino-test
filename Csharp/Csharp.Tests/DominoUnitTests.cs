﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Csharp.Excepciones;
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

        private JuegoDomino InicializarJuego(int indiceJuegosPredisenados = 0, int? overwriteTurno = null)
        {
            var jugadores = InicializarJugadores(indiceJuegosPredisenados);
            var juego = new JuegoDomino(jugadores);
            
            return juego;
        }

        private List<Jugador> InicializarJugadores(int set = 0)
        {
            var conjuntos = new[]
            {
                new List<Jugador>
                {
                    new Jugador(6,4, 6,6, 5,4, 4,4, 1,2, 3, 0, 0, 5),
                    new Jugador(3,4, 6,0, 0,0, 0,1, 5,6, 0, 4, 3, 2),
                    new Jugador(2,4, 2,0, 1,1, 1,3, 1,4, 1, 5, 1, 6),
                    new Jugador(2,2, 5,2, 2,6, 3,3, 3,5, 3, 6, 5, 5)
                },

                new List<Jugador>
                {
                    new Jugador(6,0, 6,1, 6,2, 6,3, 2,0, 6, 5, 1, 2),
                    new Jugador(5,5, 5,0, 5,2, 5,4, 3,5, 1, 5, 3, 2),
                    new Jugador(2,4, 6,4, 1,1, 1,3, 1,4, 4, 4, 6, 6),
                    new Jugador(2,2, 0,0, 0,1, 3,3, 3,0, 0, 4, 3, 4)
                },

                new List<Jugador>
                {
                    new Jugador(6,0, 6,1, 6,2, 6,3, 2,0, 6,5, 1,2),
                    new Jugador(5,5, 5,0, 5,2, 5,4, 3,5, 4,4, 3,2),
                    new Jugador(2,2, 0,0, 2,4, 3,3, 3,0, 0,4, 3,4),
                    new Jugador(0,1, 6,4, 1,1, 1,3, 1,4, 1,5, 6,6),
                },

                new List<Jugador>
                {
                    new Jugador(6,0, 6,1, 6,2, 6,3, 6,4, 6,5, 6,6),
                    new Jugador(5,5, 5,0, 5,2, 5,4, 3,5, 1,5, 1,2),
                    new Jugador(2,4, 2,0, 1,1, 1,3, 1,4, 4,4, 3,2),
                    new Jugador(2,2, 0,0, 0,1, 3,3, 3,0, 0,4, 3,4)
                },
            };

            return conjuntos[set];
        }


        private void SimularJuego(JuegoDomino juego, int? preferenciaCuadre = null)
        {
            while (juego.Jugadores[0].Fichas.Count > 0 &&
                   juego.Jugadores[1].Fichas.Count > 0 &&
                   juego.Jugadores[2].Fichas.Count > 0 &&
                   juego.Jugadores[3].Fichas.Count > 0 &&
                   juego.TurnoActual != -1)
            {
                var jugador = juego.Jugadores[juego.TurnoActual];       

                if (juego.Fichas.Count == 0)
                {
                    juego.JugarFicha(jugador,
                        jugador.Fichas.Contains(new Ficha(6, 6)) ? new Ficha(6, 6) : jugador.Fichas[0]);
                }
                else
                {
                    var fichaInicial = juego.Fichas.First();
                    var fichaFinal = juego.Fichas.Last();

                    var fichasValidas = jugador.Fichas.Where(f =>
                        fichaInicial.PuedeJugarA(f) || fichaFinal.PuedeJugarB(f)).ToList();

                    if (fichasValidas.Any())
                    {
                        if (preferenciaCuadre.HasValue)
                        {
                            if (!fichasValidas.Any(f => f.Valor.Contains(preferenciaCuadre.Value)))
                            {
                                //Si no hay fichas con el vlaor deseado, se juega una de las posibles
                                juego.JugarFicha(jugador, fichasValidas.First());
                            }
                            else
                            {
                                //Se intenta cuadrar
                                var fichaAJugar = fichasValidas.FirstOrDefault(
                                    f => fichaInicial.PuedeJugarA(f) && fichaFinal.PuedeJugarB(f));

                                if (fichaAJugar != null)
                                {
                                    juego.JugarFicha(jugador, fichaAJugar, preferenciaCuadre);
                                }
                                else
                                {
                                    //Si no se puede se cuadrar se intenta conservar el fijo (cabeza)
                                    var fichasQueNoMatanFijo = fichasValidas
                                        .Where(f => !f.Valor.Contains(preferenciaCuadre.Value))
                                        .ToList();

                                    juego.JugarFicha(jugador,
                                        fichasQueNoMatanFijo.Any()
                                            ? fichasQueNoMatanFijo.First()
                                            : fichasValidas.First());
                                }
                            }

                        }
                        else
                        {
                            juego.JugarFicha(jugador, fichasValidas.First());
                        }
                    }
                    else
                    {
                        juego.PasarTurno(jugador);
                    }
                }

                juego.DibujarTablero();
            }
        }

        #endregion

        [TestMethod]
        public void DebePoderComenzarUnJuego()
        {
            var juego = InicializarJuego(overwriteTurno:0);
            juego.JugarFicha(0, new Ficha(6, 6));

            Assert.IsNotNull(juego);
        }

        [TestMethod]
        public void SePuedeJugarUnaFichaNueva()
        {
            var juego = InicializarJuego();

            juego.JugarFicha(0, new Ficha(6, 6)); //Primera
            juego.JugarFicha(1, new Ficha(6, 0)); //Segunda

            Assert.IsTrue(juego.Fichas.Count > 1);
        }

        [TestMethod]
        public void SePuedeJugarUnaFichaYEspecificarElOrden()
        {
            var juego = InicializarJuego();

            juego.JugarFicha(0, new Ficha(6, 6));
            juego.JugarFicha(1, new Ficha(6, 0));
            juego.JugarFicha(2, new Ficha(0, 2));
            juego.JugarFicha(3, new Ficha(2, 6), 6);

            Assert.IsTrue(juego.Fichas.First().Valor.A == 6 && juego.Fichas.Last().Valor.B == 6);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SoloSePuedeJugarMovidasValidas()
        {
            var juego = InicializarJuego();

            juego.JugarFicha(0, new Ficha(6, 6));
            juego.JugarFicha(1, new Ficha(4, 3));

            Assert.IsTrue(juego.Fichas.Count == 1);
        }

        [TestMethod]
        public void SeJuegaUnaRondaExitosamente()
        {
            var juego = InicializarJuego();

            juego.JugarFicha(0, new Ficha(6, 6));
            juego.JugarFicha(1, new Ficha(6, 0));
            juego.JugarFicha(2, new Ficha(0, 2));
            juego.JugarFicha(3, new Ficha(5, 2));

            Assert.AreEqual(4, juego.Fichas.Count);
        }

        [TestMethod]
        public void DebenHaberCuatroJugadores()
        {
            var juego = new JuegoDomino();
            Assert.AreEqual(4, juego.Jugadores.Count, "No hay cuatro jugadores");
        }

        [TestMethod]
        public void LosEquiposDebenEstarOrganizadosEnFrentes()
        {
            var juego = new JuegoDomino();

            Assert.AreEqual(Frentes.Equipo1, juego.Jugadores[0].Equipo);
            Assert.AreEqual(Frentes.Equipo2, juego.Jugadores[1].Equipo);
            Assert.AreEqual(Frentes.Equipo1, juego.Jugadores[2].Equipo);
            Assert.AreEqual(Frentes.Equipo2, juego.Jugadores[3].Equipo);
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
        [ExpectedException(typeof(JugadorIntentoJugarFichaQueNoPoseeException))]
        public void JugadorSoloPuedeJugarFichasQueTengaAsignadas()
        {
            var juego = InicializarJuego();

            juego.JugarFicha(0, new Ficha(6,6));
            juego.JugarFicha(1, new Ficha(6,6));
        }

        [TestMethod]
        public void CuandoJugadorPoneUnaFichaSeDebeQuitarDeSuColeccion()
        {
            var juego = InicializarJuego(overwriteTurno:0);
            juego.JugarFicha(juego.Jugadores[0], new Ficha(6, 6));

            Assert.AreEqual(6, juego.Jugadores[0].Fichas.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(JugadorIntentoJugarEnTurnoErroneoException))]
        public void JugadorSoloPuedeJugarSiEsSuTurno()
        {
            var juego = InicializarJuego();

            juego.JugarFicha(0, new Ficha(6, 6));
            juego.JugarFicha(0, new Ficha(4, 4));
        }

        [TestMethod]
        public void JugadorPuedePasar()
        {
            var juego = InicializarJuego(3);

            juego.JugarFicha(0, new Ficha(6, 6));
            juego.PasarTurno(1);
            
        }

        [TestMethod]
        [ExpectedException(typeof(Excepciones.JugadorNoPuedePasarConFichasException))]
        public void JugadorNoPuedePasarConFichasDisponibles()
        {
            var juego = InicializarJuego();

            juego.JugarFicha(0, new Ficha(6, 6));
            juego.PasarTurno(1);
        }

        [TestMethod]
        public void SiUnJugadorSeQuedaSinFichasSeTerminaLaPartida()
        {
            var juego = InicializarJuego();
            SimularJuego(juego);

            Assert.IsTrue(juego.TurnoActual == -1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SiSeTerminaLaPartidaNosePuedeJugar()
        {
            var juego = InicializarJuego();
            SimularJuego(juego);

            var otroJugadorConFichas = juego.Jugadores.First(f => f.Fichas.Any());
            juego.JugarFicha(otroJugadorConFichas, otroJugadorConFichas.Fichas.First());
        }

        [TestMethod]
        public void ElJuegoCalculaElScoreDeQuienGano()
        {
            var juego = InicializarJuego();

            var scoreOriginal = juego.Score.Select(f => f.Sum()).ToList();
            SimularJuego(juego);
            var scoreFinal = juego.Score;

            Assert.IsTrue(scoreOriginal[0] != scoreFinal[0].Sum()
                       || scoreOriginal[1] != scoreFinal[1].Sum());
        }

        [TestMethod]
        public void ElJuegoDetectaCuandoHayUnTranque()
        {
            var juego = InicializarJuego(1);
            SimularJuego(juego, 6);

            Assert.IsTrue(juego.TurnoActual == -1 && juego.Jugadores.Count(f => f.Fichas.Any()) == 4);
        }

        [TestMethod]
        public void SiHayUnTranqueElJuegoDecideElGanador()
        {
            var juego = InicializarJuego(1);
            SimularJuego(juego, 6);

            Assert.IsTrue(juego.Score.Sum(f => f.Sum(g => g)) > 0);
        }

        [TestMethod]
        public void SePuedeJugarMasDeUnaPartida()
        {
            var juego = InicializarJuego();
            SimularJuego(juego);

            juego.NuevaPartida();

            Assert.IsTrue(juego.Score.Sum(f => f.Sum(g => g)) > 0);
            Assert.IsTrue(juego.TurnoActual != -1);
        }

        [TestMethod]
        public void ElJugadorQueDominoLaPartidaAnteriorDebeComenzarLaSiguiente()
        {
            var juego = InicializarJuego();
            SimularJuego(juego);

            juego.NuevaPartida();

            Assert.IsTrue(juego.Score.Sum(f => f.Sum(g => g)) > 0);
            Assert.IsTrue(juego.TurnoActual == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Excepciones.JuegoNoComenzoConDobleSeisException))]
        public void LaPrimeraPartidaDebeComenzarConDobleSeis()
        {
            var juego = InicializarJuego(0);
            juego.JugarFicha(0, new Ficha(6,4));
        }

        [TestMethod]
        public void LaSegundaPartidaPuedeComenzarConCualquierFicha()
        {
            var juego = new JuegoDomino();
            SimularJuego(juego); 

            juego.NuevaPartida();
            juego.Jugadores = InicializarJugadores(0);

            juego.JugarFicha(juego.Jugadores[juego.TurnoActual], 
                             juego.Jugadores[juego.TurnoActual].Fichas[0]);

            Assert.AreEqual(1, juego.Fichas.Count);
        }

        [TestMethod]
        public void ElEquipoCon200PuntosGanaElJuego()
        {
            var juego = new JuegoDomino();

            while (!juego.Score.Any(j => j.Sum() >= 200))
            {
                juego.NuevaPartida();
                SimularJuego(juego);
            }

            Assert.IsTrue(juego.JuegoTerminado);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SiElJuegoTerminoNosePuedeJugarOtraPartida()
        {
            var juego = new JuegoDomino();

            while (!juego.Score.Any(j => j.Sum() >= 200))
            {
                SimularJuego(juego);
                juego.NuevaPartida();
            }

            juego.NuevaPartida();
        }

        [TestMethod]
        public void PenalidadSiSegundoJugadorNoTieneFichas()
        {
            var juego = InicializarJuego(3);

            juego.JugarFicha(0, new Ficha(6,6));
            juego.PasarTurno(1);

            var equipoQueInicia = (int)juego.Jugadores[0].Equipo;
            Assert.IsTrue(juego.Score[equipoQueInicia].Sum() > 0);
        }

        [TestMethod]
        public void PenalidadSegundoJugadorNoAplicaSiTerceroNoTiene()
        {
            var juego = InicializarJuego(3);

            juego.JugarFicha(0, new Ficha(6, 6));
            juego.PasarTurno(1);
            juego.PasarTurno(2);

            Assert.AreEqual(juego.Score[0].Sum(), juego.Score[1].Sum());
        }

        [TestMethod]
        public void PenalidadSiTresJugadoresPasanConsecutivamente()
        {
            var juego = InicializarJuego(3);

            juego.JugarFicha(0, new Ficha(6,6));
            juego.PasarTurno(1);
            juego.PasarTurno(2);
            juego.PasarTurno(3);

            var equipoQueInicia = (int)juego.Jugadores[0].Equipo;
            Assert.IsTrue(juego.Score[equipoQueInicia].Sum() > 0);
        }

        [TestMethod]
        public void PuntosAdicionalesConKapikua()
        {
            var juego = InicializarJuego(2);

            SimularJuego(juego, 2);

            Assert.AreEqual(juego.Fichas.First().Valor.A, juego.Fichas.Last().Valor.B);
        }
    }
}
