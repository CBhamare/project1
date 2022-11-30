using MoviesList.Models.Domain;
using MoviesList.Repositories.Abstract;

namespace MoviesList.Repositories.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly DatabaseContext ctx;
        public GenreService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(Genre Model)
        {
            try
            {
                ctx.genre.Add(Model);
                ctx.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;
                ctx.genre.Remove(data);
                ctx.SaveChanges();
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Genre GetById(int id)
        {
            return ctx.genre.Find(id);            
        }

        public IQueryable<Genre> List()
        {
            var data = ctx.genre.AsQueryable();
            return data;
        }

        public bool Update(Genre Model)
        {
            try
            {
                ctx.genre.Update(Model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
