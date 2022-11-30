using MoviesList.Models.Domain;
using MoviesList.Models.DTO;

namespace MoviesList.Repositories.Abstract
{
    public interface IGenreService
    {
        bool Add(Genre Model);
        bool Update(Genre Model);
        Genre GetById(int id);
        bool Delete(int id);
        IQueryable<Genre> List();
        
    }
}
