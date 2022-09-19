using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PunkModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PunkService
{
    public class RatingsRepo : IRatingsRepo
    {
        private readonly ILogger<RatingsRepo> _logger;
        private readonly string _filePath;

        public RatingsRepo(ILogger<RatingsRepo> logger, IOptions<RepoSettings> repoSettings)
        {
            _logger = logger;
            _filePath = repoSettings.Value.FilePath;
        }

        public async Task<bool> AddRating(int beerId, UserRating rating)
        {
            try
            {
                await File.AppendAllLinesAsync(_filePath, new string[]{ JsonConvert.SerializeObject((beerId, rating)) });
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to write data to file {ex}");
                return false;
            }
        }

        public async Task<IEnumerable<UserRating>> GetRatings(int beerId)
        {
            var result = new List<UserRating>();
            try
            {
                var data = await File.ReadAllLinesAsync(_filePath);
                foreach(var line in data)
                {
                     var json = JsonConvert.DeserializeObject<Tuple<int, UserRating>>(line);
                    if (json.Item1 == beerId)
                    {
                        result.Add(json.Item2);
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to fetch rating for beerid: {beerId} from file {ex}");
                return result;
            }
        }
    }
}
