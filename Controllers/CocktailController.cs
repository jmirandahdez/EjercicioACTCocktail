using CocktailExercise.Business;
using CocktailExercise.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CocktailExercise.Controllers
{
    public class CocktailController : Controller
    {
        /// <summary>
        /// Propiedad IConfiguration
        /// </summary>
        readonly IConfiguration _configuration;

        /// <summary>
        /// Cosntructor de la clase
        /// </summary>
        /// <param name="configuration">Inicializador de IConfiguration</param>
        public CocktailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: CocktailController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CocktailController/ResultCoktails/5
        public ActionResult ResultCoktails(string type, string value)
        {
            var result = new CocktailManager(_configuration).GetCocktails(type, value);
            if (result == null || result.Result == null)
            {
                return RedirectToAction("Error", "Home");                
            }

            return View(result.Result);
        }

        // GET: CocktailController/FavoriteCocktails
        public ActionResult FavoriteCocktails()
        {
            var result = new CocktailManager(_configuration).GetFavoriteCocktail();
            if (result == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(result);
        }

        // POST: CocktailController/AddFavorite
        [HttpPost]
        public ActionResult AddFavorite(IFormCollection collection)
        {
            var cocktail = new CocktailModel
            {
                idDrink = collection["item.idDrink"].ToString(),
                strDrink = collection["item.strDrink"].ToString(),
                strAlcoholic = collection["item.strAlcoholic"].ToString(),
                strIngredients = collection["item.strIngredients"].ToString(),
                strDrinkThumb = collection["item.strDrinkThumb"].ToString()
            };

            var result = new CocktailManager(_configuration).SaveFavoriteCocktail(cocktail);
            return Json(result);
        }

        // POST: CocktailController/DeleteFavorite
        [HttpPost]
        public ActionResult DeleteFavorite(IFormCollection collection)
        {
            var idCocktail = collection["item.idDrink"].ToString();
            var result = new CocktailManager(_configuration).DeleteFavoriteCocktail(idCocktail);

            if (!result)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("FavoriteCocktails", "Cocktail");
        }
    }
}
