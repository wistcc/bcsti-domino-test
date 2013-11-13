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

        //CONSTANTES
        private const int PuntajeExtra = 25; //Cantidad otorgada en pases corridos, kapikua y otros

        //PRIVADAS
        private int _numeroPases; //Cantidd de veces que se pasa consecutivamente
        private int _numeroJugadas; //Contador de cada turno (global)
        private int _turnoActual; //A cual jugador le toca en el turno actual
        private int _ganadorPartidaAnterior; //Pesona que ganó la patrida anterior
        private bool _juegoTerminado; //Indica que el juego concluyo
        private int _cantidadPartidasJugadas; //Indica cuantas partidas se han jugado

        //PROPIEDADES
        public List<Ficha> Fichas { get; set; }

        public List<Jugador> Jugadores { get; set; }

        public List<Int32>[] Score;

        public int TurnoActual
        {
            get { return _turnoActual; }
        }

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

        #region Métodos Públicos

        public JuegoDomino(List<Jugador> jugadores = null)
        {
            Jugadores = new List<Jugador>();
            Fichas = new List<Ficha>();
            Score = new List<int>[2];
            Score[0] = new List<int> { 0 };
            Score[1] = new List<int> { 0 };

            _cantidadPartidasJugadas = 0;
            _ganadorPartidaAnterior = 0;
            _juegoTerminado = false;

            NuevaPartida(jugadores);
        }

        public void NuevaPartida(List<Jugador> jugadores = null)
        {
            if (_juegoTerminado)
                throw new Exception("No se puede comenzar otra partida, el juego ha terminado");

            Jugadores = new List<Jugador>();
            Fichas = new List<Ficha>();
            _turnoActual = _ganadorPartidaAnterior;
            _numeroPases = 0;
            _numeroJugadas = 0;

            if (jugadores == null)
                GenerarFichasAleatorias();
            else
                Jugadores = jugadores;

            //Si es la primera partida, solo puede salir el doble seis
            if (_cantidadPartidasJugadas == 0)
            {
                var primerJugador = Jugadores.First(f => f.Fichas.Contains(new Ficha(6, 6)));
                _turnoActual = Jugadores.IndexOf(primerJugador);
            }
        }

        private void GenerarFichasAleatorias()
        {
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
                throw new Excepciones.JugadorIntentoJugarEnTurnoErroneoException();

            if (!jugador.PoseeFicha(ficha))
                throw new Excepciones.JugadorIntentoJugarFichaQueNoPoseeException();

            if (Fichas.Count == 0)
            {
                //Quiere decir que nunca se ha jugado
                if (_cantidadPartidasJugadas == 0 && !ficha.Equals(new Ficha(6, 6)))
                    throw new Excepciones.JuegoNoComenzoConDobleSeisException();

                Fichas.Add(ficha);
                jugador.Fichas.Remove(ficha);
                ManejarSiguienteTurno();
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

        public string DibujarTablero()
        {
            foreach (var f in Fichas)
            {
                Trace.Write(f.Valor);
            }

            Trace.WriteLine("");

            return "";
        }

        public void PasarTurno(Jugador jugador)
        {
            if (_turnoActual == -1)
                throw new Exception("El partido ha terminado"); //TODO: Deberiamos usar excepciones personalziadas?

            if (jugador != Jugadores[_turnoActual])
                return; //TODO: Esto debería tirar otra excepcion?

            if (jugador.Fichas.Any(ficha => Fichas.First().PuedeJugarA(ficha) || Fichas.Last().PuedeJugarB(ficha)))
            {
                throw new Excepciones.JugadorNoPuedePasarConFichasException();
            }

            _numeroPases++;
            ManejarSiguienteTurno(_numeroPases == 4);
        }

        public void PasarTurno(int numeroJugador)
        {
            if (numeroJugador >= 0 && numeroJugador < 4)
                PasarTurno(Jugadores[numeroJugador]);
        }

        #endregion

        #region Métodos Privados

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
            ManejarSiguienteTurno(jugador.Fichas.Count == 0);
        }

        private void ManejarSiguienteTurno(bool terminarPartida = false)
        {
            if (terminarPartida)
            {
                _cantidadPartidasJugadas++;
                _ganadorPartidaAnterior = _turnoActual;
                _turnoActual = -1;
                CalcularScores();
                return;
            }

            _numeroJugadas++;
            _turnoActual++;

            if (_turnoActual == 4)
                _turnoActual = 0;

            //Primer pase tiene penalidad
            if (_numeroJugadas == 2 && Fichas.Count == 1)
                Score[(int)Jugadores[_ganadorPartidaAnterior].Equipo].Add(PuntajeExtra);

            //PuntajeExtra no aplica si el frente también pasa
            if (_numeroJugadas == 3 && Fichas.Count == 1)
                Score[(int)Jugadores[_ganadorPartidaAnterior].Equipo].Add(PuntajeExtra * -1);

            //PuntajeExtra si tres jugadores pasan consecutivamente
            if (_numeroPases == 3)
                Score[(int)Jugadores[_turnoActual].Equipo].Add(PuntajeExtra);

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

            if (Score[(int)frenteGanador].Sum() >= 200)
            {
                _juegoTerminado = true;
            }
        }

        #endregion

    }
}
