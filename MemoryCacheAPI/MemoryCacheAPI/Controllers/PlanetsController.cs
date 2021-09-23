using MemoryCacheAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MemoryCacheAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanetsController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private const string STARWARSPLANETS_KEY = "Planets";

        public PlanetsController(IMemoryCache memoryChache)
        {
            _memoryCache = memoryChache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            if (_memoryCache.TryGetValue(STARWARSPLANETS_KEY, out ResultStarWarsPlanetsViewModel resultApi))
            {
                resultApi.From = "Memory Cache";
                return Ok(resultApi);
            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://swapi.dev/api/planets/?format=json");

                    var responseData = await response.Content.ReadAsStringAsync();

                    resultApi = JsonSerializer.Deserialize<ResultStarWarsPlanetsViewModel>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (resultApi == null)
                        return BadRequest();

                    var memoryCacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(36), //tempo de renovação do cache
                        SlidingExpiration = TimeSpan.FromSeconds(12) //se ñ for acessado durante tempo, ele sai da memoria
                    };

                    _memoryCache.Set(STARWARSPLANETS_KEY, resultApi, memoryCacheEntryOptions);

                    resultApi.From = "Extern Call";
                    return Ok(resultApi);
                }
            }
        }
    }
}
