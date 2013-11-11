using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp
{
    /// <summary>
    /// Representa un juego de domino
    /// </summary>
    public class JuegoDomino
    {
        public Ficha Fichas { get; set; }

        public void JugarFicha(Jugadores jugador, Ficha ficha)
        {
            if (Fichas == null)
            {
                //Quiere decir que nunca se ha jugado
                if (ficha.Valor.A == ficha.Valor.B)
                    Fichas = ficha;
                else
                    throw new ArgumentException("El juego debe comenzar con un doble!");
            }
            else
            {
                //Quiere decir que ya se ha jugado, permito anexar una nueva

                if (Fichas.Valor.A == ficha.Valor.A || Fichas.Valor.A == ficha.Valor.B)
                {
                    Fichas.Siguiente = ficha;
                }
                else if (Fichas.Valor.B == ficha.Valor.A || Fichas.Valor.B == ficha.Valor.B)
                {
                    Fichas.Anterior = ficha;
                }
            }
        }

        public string DibujarTablero(Ficha ficha = null)
        {
            if (ficha == null)
                ficha = Fichas;

            Trace.Write(ficha.Valor);

            return "";
        }
    }
}
