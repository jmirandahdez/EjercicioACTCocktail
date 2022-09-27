using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailExercise.Services
{
    /// <summary>
    /// Clase abstracta para Proxy para consumo de APIS
    /// </summary>
    internal abstract class Proxy
    {
        /// <summary>
        /// Cliente para verbo Get generico
        /// </summary>
        /// <typeparam name="T">Tipo de dato a devolver</typeparam>
        /// <param name="controllerAddress">Direccion y parametros</param>
        /// <returns>Respuesta del metodo get de la API</returns>
        internal abstract T Get<T>(string controllerAddress);
    }
}
