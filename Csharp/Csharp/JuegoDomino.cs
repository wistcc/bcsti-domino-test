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

        public void JugarFicha(Jugadores jugador, Ficha ficha)
        {
            if (Fichas == null)
            {
                //Quiere decir que nunca se ha jugado. Se instancia la lista y comienza el juego
                if (ficha.Valor.A == ficha.Valor.B)
                {
                    Fichas = new List<Ficha> {ficha};
                }
                else
                    throw new ArgumentException("El juego debe comenzar con un doble!");
            }
            else
            {
                foreach (var f in Fichas)
                {
                    if (f.Valor.Equals(ficha.Valor))
                        throw new ArgumentException("No se puede jugar una ficha repetida");
                }

                //Se buscan los extremos jugados en el tablero
                if (Fichas.First().PuedeJugarA(ficha))
                {
                    if (Fichas.First().Valor.A == ficha.Valor.A)
                        ficha.Voltear();

                    Fichas.Insert(0, ficha);
                }
                else if (Fichas.Last().PuedeJugarB(ficha))
                {
                    if (Fichas.Last().Valor.B == ficha.Valor.B)
                        ficha.Voltear();

                    Fichas.Add(ficha);
                }

            }
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
