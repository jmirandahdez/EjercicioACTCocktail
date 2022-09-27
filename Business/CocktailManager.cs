using AutoMapper;
using CocktailExercise.Entities;
using CocktailExercise.Models;
using CocktailExercise.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using CocktailExercise.Utilities;
using System.Threading.Tasks;

namespace CocktailExercise.Business
{
    /// <summary>
    /// Clase para administracion de cocteles
    /// </summary>
    internal class CocktailManager
    {
        /// <summary>
        /// Propiedad IConfiguration
        /// </summary>
        readonly IConfiguration _configuration;

        /// <summary>
        /// Cosntructor de la clase
        /// </summary>
        /// <param name="configuration">Inicializador de IConfiguration</param>
        public CocktailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Obtiene los cocteles de la busqueda
        /// </summary>
        /// <param name="type">Tipo de busqueda 1.-Nombre 2.-Ingrediente</param>
        /// <param name="value">Valor de busqueda</param>
        /// <returns>Listado de cocteles</returns>
        internal async Task<List<CocktailModel>> GetCocktails(string type, string value)
        {
            List<CocktailModel> cocktails = new List<CocktailModel>();
            try
            {
                var result = await new CocktailService(_configuration).GetCocktails(type, value);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<Drink, CocktailModel>());
                foreach (var drink in result)
                {
                    var mapper = config.CreateMapper();
                    CocktailModel cocktail = mapper.Map<CocktailModel>(drink);

                    // Metodo de extension
                    cocktail.strIngredients = drink.ConvertToLine();
                    cocktails.Add(cocktail);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return cocktails;
        }

        /// <summary>
        /// Persiste el coctel en favoritos
        /// </summary>
        /// <param name="cocktail">Coctel a guardar</param>
        /// <returns>True en caso de exito, false en caso contrario</returns>
        internal bool SaveFavoriteCocktail(CocktailModel cocktail)
        {
            List<CocktailModel> favorites = new List<CocktailModel>();
            string key = "Favorites";
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

            try
            {
                if (cache.Contains(key))
                {
                    favorites = (List<CocktailModel>)cache.Get(key);
                }

                var exists = favorites.FirstOrDefault(x => x.idDrink == cocktail.idDrink);

                if (exists == null)
                {
                    favorites.Add(cocktail);
                }

                int timeToExpired = 24;
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(timeToExpired)
                };

                cache.Set(new CacheItem(key, favorites), policy);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Elimina coctel de favoritos
        /// </summary>
        /// <param name="cocktail">Coctel a eliminar</param>
        /// <returns>True en caso de exito, false en caso contrario</returns>
        internal bool DeleteFavoriteCocktail(string cocktail)
        {
            List<CocktailModel> favorites = new List<CocktailModel>();
            string key = "Favorites";
            ObjectCache cache = MemoryCache.Default;

            try
            {
                if (cache.Contains(key))
                {
                    favorites = (List<CocktailModel>)cache.Get(key);
                }

                var exists = favorites.FirstOrDefault(x => x.idDrink == cocktail);

                if (exists != null)
                {
                    favorites = favorites.Where(x => x.idDrink != cocktail).ToList();
                }

                int timeToExpired = 600;
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(timeToExpired)
                };

                cache.Set(new CacheItem(key, favorites), policy);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Obtiene los coteles agregados a favoritos
        /// </summary>
        /// <returns>Listado de cocteles</returns>
        internal List<CocktailModel> GetFavoriteCocktail()
        {
            List<CocktailModel> favorites = new List<CocktailModel>();
            string key = "Favorites";
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

            try
            {
                if (cache.Contains(key))
                {
                    favorites = (List<CocktailModel>)cache.Get(key);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return favorites;
        }      
    }
}
