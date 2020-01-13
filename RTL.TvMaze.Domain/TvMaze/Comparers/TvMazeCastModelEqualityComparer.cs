using RTL.TvMaze.Domain.TvMaze.Models;

namespace RTL.TvMaze.Domain.TvMaze.Comparers
{
    public class TvMazeCastModelEqualityComparer : ITvMazeCastModelEqualityComparer
    {
        public bool Equals(TvMazeCastModel x, TvMazeCastModel y)
        {
            return x.Person?.Id == y.Person?.Id;
        }

        public int GetHashCode(TvMazeCastModel obj)
        {
            return obj.Person.Id.GetHashCode() ^ obj.ShowId.GetHashCode();
        }
    }
}
