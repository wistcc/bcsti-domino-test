using System.Collections.Generic;

namespace Csharp
{
    /// <summary>
    /// Representa un jugador en el juego de Domino
    /// </summary>
    public class Jugador
    {
        public string Nombre { get; set; }

        public Frentes Equipo { get; set; }

        public List<Ficha> Fichas { get; set; }

        public Jugador()
        {
            this.Fichas = new List<Ficha>();
        }

        public bool PoseeFicha(Ficha ficha)
        {
            return Fichas.Contains(ficha);
        }
    }
}