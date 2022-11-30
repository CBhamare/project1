using MoviesList.Models.Domain;
using MoviesList.Models.DTO;

namespace MoviesList.Repositories.Abstract
{
    public interface IMovieService
    {
        bool Add(Movie model);
        bool Update(Movie model);
        Movie GetById(int id);
        bool Delete(int id);
        MovieListVm List(string term="", bool paging = false, int Currentpage = 0);
        List<int> GetGenreByMovieId(int movieID);


    }
}
