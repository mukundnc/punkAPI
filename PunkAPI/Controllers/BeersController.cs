using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PunkModels;
using PunkService.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunkAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BeersController : ControllerBase
    {
        private readonly ILogger<BeersController> _logger;
        private readonly IBeerService _beerService;

        public BeersController(ILogger<BeersController> logger, IBeerService beerService)
        {
            _logger = logger;
            _beerService = beerService;
        }

        [HttpGet]
        [Route("")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IEnumerable<Beer>> GetBeersByName([FromQuery] string name)
        {
            var beers = await _beerService.GetBeers(name);
            return beers;

        }

        [HttpPost]
        [Route("{beerId}/ratings")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IEnumerable<UserRating>> AddUserRating([FromRoute] int beerId, [FromBody] UserRating rating)
        {
            var result = await _beerService.AddUserRating(beerId, rating);
            return result;
        }
    }
}
