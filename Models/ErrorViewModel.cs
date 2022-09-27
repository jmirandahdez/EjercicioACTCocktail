using System;

namespace CocktailExercise.Models
{
    /// <summary>
    /// Modelo para Error
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
