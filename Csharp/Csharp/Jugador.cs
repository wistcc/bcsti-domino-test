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
            Fichas = new List<Ficha>();
        }

        public Jugador(params int[] fichas) : this()
        {
            for (var i = 0; i < fichas.Length; i+= 2)
            {
                Fichas.Add(new Ficha(fichas[i], fichas[i+1]));
            }
        }

        public bool PoseeFicha(Ficha ficha)
        {
            return Fichas.Contains(ficha);
        }
    }
}