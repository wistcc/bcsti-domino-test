using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Csharp
{
    /// <summary>
    /// Representa un juego de domino
    /// </summary>
    public class JuegoDomino
    {
        #region Propiedades

        public List<Ficha> Fichas { get; set; }

        public List<Jugador> Jugadores { get; set; }

        public List<Int32>[] Score;

        private int _numeroPases;
        private int _turnoActual;
        private int _ganadorPartidaAnterior;

        public int TurnoActual
        {
            get { return _turnoActual; }
        }


        private bool _juegoTerminado;
        public bool JuegoTerminado
        {
            get { return _juegoTerminado; }
        }

        private int ExtremoIzq
        {
            get { return Fichas.First().Valor.A; }
        }

        private int ExtremoDer
        {
            get { return Fichas.Last().Valor.B; }
        }

        #endregion


        public JuegoDomino()
        {
            Jugadores = new List<Jugador>();
            Fichas = new List<Ficha>();
            Score = new List<int>[2];
            Score[0] = new List<int> { 0 };
            Score[1] = new List<int> { 0 };
            
            _ganadorPartidaAnterior = 0;
            _juegoTerminado = false;

            NuevaPartida();
        }

        public void NuevaPartida()
        {
            if (_juegoTerminado)
                throw new Exception("No se puede comenzar otra partida, el juego ha terminado");

            Jugadores = new List<Jugador>();
            Fichas = new List<Ficha>();
            _turnoActual = _ganadorPartidaAnterior;
            _numeroPases = 0;

            //Se crean todas las fichas posibles
            var fichasPosibles = new List<Ficha>();
            for (var i = 0; i < 7; i++)
            {
                for (var j = i; j < 7; j++)
                {
                    fichasPosibles.Add(new Ficha(i, j));
                }
            }
            //Se organizan las fichas aleatoriamente
            fichasPosibles = fichasPosibles.OrderBy(a => Guid.NewGuid()).ToList();
            
            for (var i = 0; i < 4; i++)
            {
                var equipo = i % 2 == 0 ? 0 : 1;
                var jugador = new Jugador
                {
                    Equipo = (Frentes)equipo
                };
                for (var j = 0; j < 7; j++)
                {
                    jugador.Fichas.Add(fichasPosibles[i * 7 + j]);
                }
                Jugadores.Add(jugador);
            }
        }

        public void JugarFicha(int numeroJugador, Ficha ficha, int? ladoPreferido = null)
        {
            if (numeroJugador >= 0 && numeroJugador < 4)
                JugarFicha(Jugadores[numeroJugador], ficha, ladoPreferido);
        }

        public void JugarFicha(Jugador jugador, Ficha ficha, int? ladoPreferido = null)
        {
            if (_turnoActual == -1)
                throw new Exception("El partido ha terminado");

            if (jugador != Jugadores[_turnoActual])
                throw new ArgumentException("El jugador no puede jugar si no es su turno");

            if (!jugador.PoseeFicha(ficha))
                throw new ArgumentException("El jugador no puede jugar con fichas que no posee");

            if (Fichas == null || Fichas.Count == 0)
            {
                //Quiere decir que nunca se ha jugado
                Fichas.Add(ficha);
                jugador.Fichas.Remove(ficha);
                PasarTurno();
            }
            else
            {
                if (Fichas.Any(f => f.Valor.Equals(ficha.Valor)))
                {
                    throw new ArgumentException("No se puede jugar una ficha repetida");
                }

                //Se revisa si el jugador tiene intencion de un lado especifico
                if (ladoPreferido.HasValue)
                {
                    if (ficha.Valor.A == ExtremoIzq && ficha.Valor.B == ExtremoDer ||
                         ficha.Valor.B == ExtremoIzq && ficha.Valor.A == ExtremoDer)
                    {
                        ProcesarJugada(jugador, ficha, ladoPreferido == ExtremoDer);
                        return;
                    }
                }

                //De lo contrario se intenta colocar la ficha en el tablero, comenzando por el lado izquierdo
                if (Fichas.First().PuedeJugarA(ficha))
                {
                    ProcesarJugada(jugador, ficha, true);
                }
                else if (Fichas.Last().PuedeJugarB(ficha))
                {
                    ProcesarJugada(jugador, ficha);
                }
                else
                {
                    throw new Exception("Jugada Invalida!");
                }

            }
        }

        private void ProcesarJugada(Jugador jugador, Ficha ficha, bool insertarEnExtremoIzq = false)
        {
            if (insertarEnExtremoIzq)
            {
                if (Fichas.First().Valor.A == ficha.Valor.A)
                    ficha.Voltear();

                Fichas.Insert(0, ficha);
            }
            else
            {
                if (Fichas.Last().Valor.B == ficha.Valor.B)
                    ficha.Voltear();

                Fichas.Add(ficha);
            }

            _numeroPases = 0;
            jugador.Fichas.Remove(ficha);
            PasarTurno(jugador.Fichas.Count == 0);
        }

        private void PasarTurno(bool terminarPartida = false)
        {
            if (terminarPartida)
            {
                _ganadorPartidaAnterior = _turnoActual;
                _turnoActual = -1;
                CalcularScores();
                return;
            }

            _turnoActual++;
            if (_turnoActual == 4)
                _turnoActual = 0;
        }

        private void CalcularScores()
        {
            Frentes frenteGanador;

            if (Jugadores.Any(j => j.Fichas.Count == 0))
            {
                //Quiere decir que un jugador dominó
                frenteGanador = Jugadores.First(j => j.Fichas.Count == 0).Equipo;
            }
            else
            {
                //Quiere decir que hubo un tranque y se cuentan las fichas de cada equipo
                var equipo1 = Jugadores.Where(f => f.Equipo == Frentes.Equipo1).Sum(j => j.Fichas.Sum(f => f.Puntos));
                var equipo2 = Jugadores.Where(f => f.Equipo == Frentes.Equipo2).Sum(j => j.Fichas.Sum(f => f.Puntos));

                frenteGanador = equipo1 < equipo2 ? Frentes.Equipo1 : Frentes.Equipo2;
            }

            //Una vez se tiene el ganador, se suman todas las fichas restantes y se asigna el puntaje
            var score = Jugadores.Sum(j => j.Fichas.Sum(f => f.Puntos));
            Score[(int)frenteGanador].Add(score);

            if (Score[(int) frenteGanador].Sum() >= 200)
            {
                _juegoTerminado = true;
            }
        }


        public string DibujarTablero()
        {
            foreach (var f in Fichas)
            {
                Trace.Write(f.Valor);
            }

            Trace.WriteLine("");

            return "";
        }

        public void PasarJuego(Jugador jugador)
        {
            if (_turnoActual == -1)
                throw new Exception("El partido ha terminado");

            if (jugador != Jugadores[_turnoActual])
                return;

            if (jugador.Fichas.Any(ficha => Fichas.First().PuedeJugarA(ficha) || Fichas.Last().PuedeJugarB(ficha)))
            {
                throw new Exception("El jugador no puede pasar con fichas");
            }

            _numeroPases++;
            PasarTurno(_numeroPases == 4);
        }
    }
}
