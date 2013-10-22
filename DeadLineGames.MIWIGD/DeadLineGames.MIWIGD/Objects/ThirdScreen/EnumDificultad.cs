/*
 * Creador : Chema 
 * Fecha Creacion: 06/06/2012
 * Notas
 * 
 * Enumeraci�n que indica la dificultad con la que se est� jugando.
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creaci�n de la enumeraci�n.
 * 
 * 1.1 02/07/2012
 * 
 * Se a�ade el m�todo de extensi�n Parse a la enumeracion
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarPaper.Class.Events.Director
{
    public enum EnumDificultad
    {
        SuperFacil,
        Facil,
        Normal,
        Dificil,
        SuperDificil        
    }

    public static class EnumDificultadExtensions
    {
        public static EnumDificultad Parse(string dificultad)
        {            
            if (dificultad == "Super Easy")
            {
                return EnumDificultad.SuperFacil;
            }
            else if (dificultad == "Easy")
            {
                return EnumDificultad.Facil;
            }
            else if (dificultad == "Normal")
            {
                return EnumDificultad.Normal;
            }
            else if (dificultad == "Hard")
            {
                return EnumDificultad.Dificil;
            }
            else if (dificultad == "Extra Hard")
            {
                return EnumDificultad.SuperDificil;
            }
            else
            {
                return EnumDificultad.Normal;
            }
        }
    }
}
