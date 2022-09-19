using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using PunkModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PunkService
{
    public class PunkProxy : IPunkProxy
    {
        private readonly ILogger<PunkProxy> _logger;
        private readonly HttpClient _client;
        private const string beerName = "beer_name";

        public PunkProxy(ILogger<PunkProxy> logger, HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _logger = logger;
            _client = httpClient;
            _client.BaseAddress = new Uri(apiSettings.Value.Punk);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Application.Json));
        }

        public async Task<Beer> GetBeerById(int id)
        {
            var result = new Beer();
            try
            {
                var resp = await _client.GetAsync($"{id}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<Beer>>(json)[0];
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to fetch beerid {id} from PunkAPI: {ex}");
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<Beer>> GetBeersByName(string name)
        {
            var result = new List<Beer>();
            try
            {
                var query = new Dictionary<string, string> { { beerName, name } };
                var resp = await _client.GetAsync(QueryHelpers.AddQueryString(string.Empty, query));
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<Beer>>(json);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch beer name {name} from PunkAPI: {ex}");
                throw ex;
            }
            return result;
        }
    }
}
