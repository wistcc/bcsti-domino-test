using System;
using System.Collections.Generic;
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
            if (ficha.Valor.A == ficha.Valor.B)
                Fichas = ficha;
            else
                throw new ArgumentException("El juego debe comenzar con un doble!");
        }
    }
}
