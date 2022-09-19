using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunkModels
{
    public interface IBeerService
    {
        Task<IEnumerable<UserRating>> AddUserRating(int beerId, UserRating rating);
        Task<IEnumerable<Beer>> GetBeers(string beerName);
    }
}
