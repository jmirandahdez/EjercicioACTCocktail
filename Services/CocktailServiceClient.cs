using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;

namespace CocktailExercise.Services
{
    internal class CocktailServiceClient : Proxy
    {
        private readonly IConfiguration _configuration;

        private string URL { get; set; }

        /// <summary>
        /// Cosntructor de la clase
        /// </summary>
        /// <param name="configuration">Inicializador de IConfiguration</param>
        internal CocktailServiceClient(IConfiguration configuration) 
        {
            _configuration = configuration;
            URL = _configuration.GetValue<string>("AppSettings:URLCocktailService");
        }
              
        /// <summary>
        /// Cliente para verbo Get generico
        /// </summary>
        /// <typeparam name="T">Tipo de dato a devolver</typeparam>
        /// <param name="controllerAddress">Direccion y parametros</param>
        /// <returns>Respuesta del metodo get de la API</returns>
        internal override T Get<T>(string controllerAddress)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(URL)
            };

            T result;
            Stream stream = null;
            try
            {
                string serviceAddress = string.Format("{0}{1}", client.BaseAddress, controllerAddress);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, serviceAddress);

                using (var response = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult())
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return default(T);
                    }

                    stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                    using (var streamReader = new StreamReader(stream))
                    {
                        var responseResult = streamReader.ReadToEnd();
                        stream = null;

                        result = JsonConvert.DeserializeObject<T>(responseResult);
                        response.Content.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                result = default(T);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }

            return result;
        }
    }
}
