using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunkModels
{
    public interface IRatingsRepo
    {
        Task<IEnumerable<UserRating>> GetRatings(int beerId);
        Task<bool> AddRating(int beerId, UserRating rating);
    }
}
