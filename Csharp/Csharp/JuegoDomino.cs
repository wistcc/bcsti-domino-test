using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Csharp
{
    /// <summary>
    /// Representa un juego de domino
    /// </summary>
    public class JuegoDomino
    {
        public List<Ficha> Fichas { get; set; }

        public List<Jugador> Jugadores { get; set; } 

        public JuegoDomino()
        {
            Jugadores = new List<Jugador>();
            Fichas = new List<Ficha>();

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
                var jugador = new Jugador();
                for (var j = 0; j < 7; j++)
                {
                    jugador.Fichas.Add(fichasPosibles[i*7 + j]);
                }
                Jugadores.Add(jugador);
            }

        }

        public void JugarFicha(Jugador jugador, Ficha ficha)
        {
            if (!jugador.PoseeFicha(ficha))
            {
                return;
                //throw new ArgumentException("El jugador no puede jugar con fichas que no posee");
            }

            if (Fichas == null || Fichas.Count == 0)
            {
                //Quiere decir que nunca se ha jugado. Se instancia la lista y comienza el juego
                Fichas = new List<Ficha> { ficha };
                jugador.Fichas.Remove(ficha);
            }
            else
            {
                if (Fichas.Any(f => f.Valor.Equals(ficha.Valor)))
                {
                    throw new ArgumentException("No se puede jugar una ficha repetida");
                }

                //Se buscan los extremos jugados en el tablero
                if (Fichas.First().PuedeJugarA(ficha))
                {
                    if (Fichas.First().Valor.A == ficha.Valor.A)
                        ficha.Voltear();

                    Fichas.Insert(0, ficha);
                    jugador.Fichas.Remove(ficha);
                }
                else if (Fichas.Last().PuedeJugarB(ficha))
                {
                    if (Fichas.Last().Valor.B == ficha.Valor.B)
                        ficha.Voltear();

                    Fichas.Add(ficha);
                    jugador.Fichas.Remove(ficha);
                }
            }
        }

        public void JugarFicha(int numeroJugador, Ficha ficha)
        {
            if (numeroJugador >= 0 && numeroJugador < 4)
                JugarFicha(Jugadores[numeroJugador], ficha);
        }

        public string DibujarTablero(Ficha ficha = null)
        {
            if (Fichas == null)
                return "No se ha jugado";

            foreach (var f in Fichas)
            {
                Trace.Write(f.Valor);
            }

            return "";
        }
    }
}
