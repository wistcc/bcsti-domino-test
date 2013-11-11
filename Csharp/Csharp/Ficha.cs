using System.Collections;
using System;
using System.Text;

namespace Csharp
{
    /// <summary>
    /// Representa una ficha en un juego de domino
    /// </summary>
    public class Ficha
    {
        public struct ValorFicha
        {
            public int A;
            public int B;

            public ValorFicha(int a, int b)
            {
                A = a;
                B = b;
            }

            public override string ToString()
            {
                return String.Format("[{0},{1}]", A, B);
            }

            public override bool Equals(object obj)
            {
                if (obj is ValorFicha)
                {
                    var valor = (ValorFicha)obj;
                    return (A == valor.A && B == valor.B) ||
                           (A == valor.B && B == valor.A) ;
                }
                
                return false;
            }
        }

        public ValorFicha Valor { get; set; }

        public Ficha(int a, int b)
        {
            Valor = new ValorFicha(a, b);
        }

        public void Voltear()
        {
            Valor = new ValorFicha(Valor.B, Valor.A);
        }


        public bool PuedeJugar(Ficha fichaPropuesta)
        {
            return  Valor.A == fichaPropuesta.Valor.A ||
                    Valor.A == fichaPropuesta.Valor.B || 
                    Valor.B == fichaPropuesta.Valor.A || 
                    Valor.B == fichaPropuesta.Valor.B;
        }

        public bool PuedeJugarA(Ficha fichaPropuesta)
        {
            return Valor.A == fichaPropuesta.Valor.A ||
                   Valor.A == fichaPropuesta.Valor.B;
        }

        public bool PuedeJugarB(Ficha fichaPropuesta)
        {
            return Valor.B == fichaPropuesta.Valor.A ||
                   Valor.B == fichaPropuesta.Valor.B;
        }

        public override string ToString()
        {
            return String.Format("[{0},{1}]", Valor.A, Valor.B);
        }
    }
}
