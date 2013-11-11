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
        public List<Ficha> Fichas { get; set; }

        public void JugarFicha(Jugadores jugador, Ficha ficha)
        {
            if (Fichas == null)
            {
                //Quiere decir que nunca se ha jugado
                if (ficha.Valor.A == ficha.Valor.B)
                {
                    Fichas = new List<Ficha> {ficha};
                }
                else
                    throw new ArgumentException("El juego debe comenzar con un doble!");
            }
            else
            {
                //Quiere decir que ya se ha jugado, permito anexar una nueva
                
                //Se buscan los extremos jugados en el tablero
                if (Fichas.First().PuedeJugarA(ficha))
                {
                    Fichas.Insert(0, ficha);
                }
                else if (Fichas.Last().PuedeJugarB(ficha))
                {
                    Fichas.Add(ficha);
                }

            }
        }

        public string DibujarTablero(Ficha ficha = null)
        {
            if (Fichas == null)
                return "No se ha jugado";

            for (int i = 0; i < Fichas.Count; i++)
            {
                Trace.Write(Fichas[i].Valor);
            }

            return "";
        }
    }
}
