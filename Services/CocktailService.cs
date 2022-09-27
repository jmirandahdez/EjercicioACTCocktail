using CocktailExercise.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailExercise.Services
{
    internal class CocktailService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Cosntructor de la clase
        /// </summary>
        /// <param name="configuration">Inicializador de IConfiguration</param>
        internal CocktailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Obtiene los cocteles de la busqueda
        /// </summary>
        /// <param name="type">Tipo de busqueda 1.-Nombre 2.-Ingrediente</param>
        /// <param name="value">Valor de busqueda</param>
        /// <returns>Listado de cocteles</returns>
        internal Task<List<Drink>> GetCocktails(string type, string value)
        {
            var method = type == "1" ? "search.php?s" : "filter.php?i";
            var result = new CocktailServiceClient(_configuration).Get<Cocktails>(string.Format("{0}={1}", method, value));

            return Task.FromResult(result != null && result.Drinks != null ? result.Drinks : new List<Drink>());
        }
    }
}
