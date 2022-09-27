using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CocktailExercise.Entities
{
    /// <summary>
    /// Entidad para Cocktails
    /// </summary>
    internal class Cocktails
    {
        [JsonPropertyName("drinks")]
        public List<Drink> Drinks { get; set; }

    }
}
