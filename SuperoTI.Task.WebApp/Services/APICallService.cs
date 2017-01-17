using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperoTI.Task.WebApp.Services
{
    public class APICallService
    {
        string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["WebApiURL"];

        /// <summary>
        /// Retorna o model de acordo com o parâmetro passado
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="param">Parâmetro(s) de busca do model</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string controller, string param) where T : class
        {
            if (!String.IsNullOrEmpty(param))
            {
                param = "?" + param;
            }
            string url = string.Format(ApiUrl + "/{0}/{1}", controller, param);

            var client = new System.Net.Http.HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(url);

            var JsonResult = response.Content.ReadAsStringAsync().Result;

            if (typeof(T) == typeof(string))
                return null;

            var rootobject = JsonConvert.DeserializeObject<T>(JsonResult);

            return rootobject;
        }

        /// <summary>
        /// Retorna uma lista de model de acordo com o parâmetro passado
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="param">Parâmetro(s) de busca do model</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync<T>(string controller, string param) where T : class
        {
            try
            {
                if (!String.IsNullOrEmpty(param))
                {
                    param = "?" + param;
                }

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string url = string.Format(ApiUrl + "/{0}?{1}", controller, param);
                var response = await client.GetAsync(url);

                var JsonResult = response.Content.ReadAsStringAsync().Result;

                if (typeof(T) == typeof(string))
                    return null;

                var rootobject = JsonConvert.DeserializeObject<List<T>>(JsonResult);

                return rootobject;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Atualiza o model
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="param">Parâmetros de busca do model (chaves)</param>
        /// <param name="item">Model a ser atualizado</param>
        /// <returns></returns>
        public async Task<string> PutAsync<T>(string controller, string param, T item) where T : class
        {
            var client = new System.Net.Http.HttpClient();

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(string.Format(ApiUrl + "/{0}?{1}", controller, param), content);
            if (response.IsSuccessStatusCode)
            {
                return String.Empty;
            }
            else
            {
                string retorno = await response.Content.ReadAsStringAsync();
                return retorno;
            }
        }

        /// <summary>
        /// Inclui model
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="item">Model a ser incluido</param>
        /// <returns></returns>
        public async Task<string> PostAsync<T>(string controller, T item) where T : class
        {
            var client = new System.Net.Http.HttpClient();

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(string.Format(ApiUrl + "/{0}", controller), content);
            if (response.IsSuccessStatusCode)
            {
                return String.Empty;
            }
            else
            {
                string retorno = await response.Content.ReadAsStringAsync();
                return retorno;
            }
        }
    }
}