/*
 * Creador : Chema 
 * Fecha Creacion: 29/06/2012
 * Notas
 * 
 * Interfaz para reconocer las clases que son bonus
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la interfaz
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarPaper.Class.Objects.Buenos
{
    public interface IBonus
    {
        EnumBonusType BonusType
        { get; set; }
    }  
}
