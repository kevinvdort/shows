using System.Collections.Generic;

namespace RTL.TvMaze.Api.Shows.Models
{
    public class ShowModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PersonModel> Cast { get; set; }
    }
}
