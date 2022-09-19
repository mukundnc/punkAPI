using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PunkModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunkService
{
    public class BeerService : IBeerService
    {
        private readonly ILogger<BeerService> _logger;
        private readonly IRatingsRepo _ratingsRepo;
        private readonly IPunkProxy _punkProxy;

        public BeerService(ILogger<BeerService> logger, IRatingsRepo ratingsRepo, IPunkProxy punkProxy)
        {
            _logger = logger;
            _ratingsRepo = ratingsRepo;
            _punkProxy = punkProxy;
        }

        public async Task<IEnumerable<UserRating>> AddUserRating(int beerId, UserRating rating)
        {
            _logger.LogInformation($"beerid: {beerId}, rating:{JsonConvert.SerializeObject(rating)}");
            var result = await _ratingsRepo.AddRating(beerId, rating);
            if (!result)
            {
                _logger.LogError($"Error failing to add rating ${beerId}");
                throw new Exception(Constants.AddFailedError);
            }
            var ratings = await _ratingsRepo.GetRatings(beerId);
            return ratings;
        }

        public async Task<IEnumerable<Beer>> GetBeers(string beerName)
        {
            _logger.LogInformation($"search beer name: {beerName}");
            var beers = await _punkProxy.GetBeersByName(beerName);
            foreach(var beer in beers)
            {
                beer.Ratings = await _ratingsRepo.GetRatings(beer.Id);
            }
            return beers;
        }
    }
}
