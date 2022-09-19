using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunkModels
{
    public interface IPunkProxy
    {
        Task<Beer> GetBeerById(int id);
        Task<IEnumerable<Beer>> GetBeersByName(string name);
    }
}
