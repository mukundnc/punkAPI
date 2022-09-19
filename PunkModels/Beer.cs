using System.Collections.Generic;

namespace PunkModels
{
    public class Beer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<UserRating> Ratings { get; set; }
    }
}
