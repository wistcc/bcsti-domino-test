using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;

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

        private int _turnoActual;
        public int TurnoActual
        {
            get { return _turnoActual; }
        }

        #endregion


        public JuegoDomino()
        {
            Jugadores = new List<Jugador>();
            Fichas = new List<Ficha>();
            Score = new List<int> [2];
            Score[0] = new List<int> {0};
            Score[1] = new List<int> {0};
            
            _turnoActual = 0;

            //Se crean todas las fichas posibles
            var fichasPosibles = new List<Ficha>();
            for (var i = 0; i < 7; i++)
            {
                for (var j = i; j < 7; j++)
                {
                    fichasPosibles.Add(new Ficha(i,j));
                }
            }

            //Se reparten entre los jugadores
            fichasPosibles = fichasPosibles.OrderBy(a => Guid.NewGuid()).ToList();
            for (var i = 0; i < 4; i++)
            {
                var equipo = i%2 == 0 ? 1 : 2; 
                var jugador = new Jugador(){Equipo = (Frentes)equipo};
                for (var j = 0; j < 7; j++)
                {
                    jugador.Fichas.Add(fichasPosibles[i*7 + j]);
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
                //Quiere decir que nunca se ha jugado. Se instancia la lista y comienza el juego
                Fichas = new List<Ficha> { ficha };
                jugador.Fichas.Remove(ficha);
                PasarTurno();
            }
            else
            {
                if (Fichas.Any(f => f.Valor.Equals(ficha.Valor)))
                {
                    throw new ArgumentException("No se puede jugar una ficha repetida");
                }

                //Se revisa si se quiere beneficiar un lado especifico
                if (ladoPreferido.HasValue)
                {
                    //TODO: Abstraer
                    var valorIzq = Fichas.First().Valor.A;
                    var valorDer = Fichas.Last().Valor.B;

                    if (ficha.Valor.A == valorIzq && ficha.Valor.B == valorDer ||
                        ficha.Valor.B == valorIzq && ficha.Valor.A == valorDer)
                    {
                        //TODO: DRY THIS SHHH UP!
                        if (ladoPreferido == valorIzq)
                        {
                            //se juega en el lado derecho
                            if (Fichas.Last().Valor.B == ficha.Valor.B)
                                ficha.Voltear();

                            Fichas.Add(ficha);
                            jugador.Fichas.Remove(ficha);
                            PasarTurno(jugador.Fichas.Count == 0);
                            return;
                        }
                        else
                        {
                            if (Fichas.First().Valor.A == ficha.Valor.A)
                                ficha.Voltear();

                            Fichas.Insert(0, ficha);
                            jugador.Fichas.Remove(ficha);
                            PasarTurno(jugador.Fichas.Count == 0);
                            return;
                        }
                    }
                }

                //Se buscan los extremos jugados en el tablero
                if (Fichas.First().PuedeJugarA(ficha))
                {
                    if (Fichas.First().Valor.A == ficha.Valor.A)
                        ficha.Voltear();

                    Fichas.Insert(0, ficha);
                    jugador.Fichas.Remove(ficha);
                    PasarTurno(jugador.Fichas.Count == 0);
                }
                else if (Fichas.Last().PuedeJugarB(ficha))
                {
                    if (Fichas.Last().Valor.B == ficha.Valor.B)
                        ficha.Voltear();

                    Fichas.Add(ficha);
                    jugador.Fichas.Remove(ficha);
                    PasarTurno(jugador.Fichas.Count == 0);
                }
                else
                {
                    throw new Exception("Jugada Invalida!");
                }
                
            }
        }

        private void PasarTurno(bool terminarPartida = false)
        {
            if (terminarPartida)
            {
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
            var frenteGanador = Jugadores.First(j => j.Fichas.Count == 0).Equipo;
            var score = Jugadores.Sum(j => j.Fichas.Sum(f => f.Puntos));

            Score[(int) frenteGanador].Add(score);
        }


        public string DibujarTablero()
        {
            foreach (var f in Fichas)
            {
                Trace.Write(f.Valor);
            }

            return "";
        }

        public void PasarJuego(Jugador jugador)
        {
            if (_turnoActual == -1)
                throw new Exception("El partido ha terminado");

            if (jugador == Jugadores[_turnoActual])
            {
                var tieneFichas = false;

                foreach (var ficha in jugador.Fichas)
                {
                    if (Fichas.First().PuedeJugarA(ficha) || Fichas.Last().PuedeJugarB(ficha))
                        throw new Exception("El jugador no puede pasar con fichas");
                }

                PasarTurno();
            }
        }
    }
}
