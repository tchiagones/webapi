using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Configuration;
using System.Linq.Expressions;
using Newtonsoft.Json;
using DP.CCM.Entity.Models.ValueObject;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DP.ProductionOrderWebApi.Tests
{
    public class JsonHelper
    {
        HttpClient _client = new HttpClient();
        private string _addressBase = ConfigurationManager.AppSettings["urlApi"];
        MediaTypeFormatter[] formatter;

        public JsonHelper()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_addressBase);
            _client.DefaultRequestHeaders.Add("ContentType", "application/json");
            formatter = new MediaTypeFormatter[] { new JsonMediaTypeFormatter() };
        }

        public TResult Get<TResult>(string urlMethod)           
        {
            try
            {
                HttpResponseMessage response = _client.GetAsync(string.Concat(this._addressBase, urlMethod)).Result;
                return JsonConvert.DeserializeObject<TResult>(response.Content.ReadAsStringAsync().Result);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public TResult Post<TFilter, TResult>(string urlMethod, TFilter parameters)
        {
            try
            {
                var response = _client.PostAsJsonAsync<TFilter>(urlMethod, parameters).Result;
                return JsonConvert.DeserializeObject<TResult>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}