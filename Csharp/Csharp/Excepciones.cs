using System;

namespace Csharp.Excepciones
{
    public class JuegoNoComenzoConDobleSeisException : Exception
    {
    }

    public class JugadorNoPuedePasarConFichasException : Exception
    {
        
    }

    public class JugadorIntentoJugarFichaQueNoPoseeException : Exception
    {
    }

    public class JugadorIntentoJugarEnTurnoErroneoException : Exception
    {}
}