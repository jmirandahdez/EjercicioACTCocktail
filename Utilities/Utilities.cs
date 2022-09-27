using CocktailExercise.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CocktailExercise.Utilities
{
    /// <summary>
    /// Clase para funciones de aopyo para la solucion
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Convierte los valores de un objeto a un string concatenado
        /// </summary>
        /// <param name="obj">Objeto a tratar</param>
        /// <returns>String concatenado con el valor de las propiedades del objeto</returns>
        internal static string ConvertToLine(this Drink obj)
        {
            string line = string.Empty;

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.Name.ToLower().Contains("stringredient") && !string.IsNullOrEmpty(Convert.ToString(propertyInfo.GetValue(obj))))
                {
                    line += " " + Convert.ToString(propertyInfo.GetValue(obj)) + ",";
                }
            }

            return line.TrimEnd(',');
        }
    }
}
