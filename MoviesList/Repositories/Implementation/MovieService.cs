using MoviesList.Models.Domain;
using MoviesList.Models.DTO;
using MoviesList.Repositories.Abstract;

namespace MoviesList.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DatabaseContext ctx;
        public MovieService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(Movie model)
        {
            try
            {
                ctx.movies.Add(model);
                ctx.SaveChanges();
                foreach(int genreId in model.Genres)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = model.Id,
                        GenreId = genreId
                    }; 
                    ctx.moviegenre.Add(movieGenre);
                }
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
                var movieGenres = ctx.moviegenre.Where(a => a.MovieId == data.Id);
                foreach(var movieGenre in movieGenres)
                {
                    ctx.moviegenre.Remove(movieGenre);
                }
                ctx.movies.Remove(data);
                ctx.SaveChanges();
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Movie GetById(int id)
        {
            return ctx.movies.Find(id);            
        }

        public MovieListVm List(string term="",bool paging=false,int Currentpage=0)
        {
            var data = new MovieListVm();
            
            var list = ctx.movies.ToList();
            
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().StartsWith(term)).ToList();
            }

            if (paging)
            {
                int PageSize = 5;
                int Count = list.Count;
                int TotalPages = (int)Math.Ceiling(Count / (double)PageSize);
                list = list.Skip((Currentpage - 1)*PageSize).Take(PageSize).ToList();
                data.PageSize = PageSize;
                data.CurrentPage = Currentpage;
                data.TotalPages = TotalPages;
            }

            foreach (var movie in list)
            {
                var genres = (from genre in ctx.genre
                              join mg in ctx.moviegenre
                              on genre.ID equals mg.GenreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreName = string.Join(',', genres);
                movie.GenreNames = genreName;
            }

            data.MovieList = list.AsQueryable();
            return data;
        }

        public bool Update(Movie model)
        {
            try
            {
                var genresToDeleted = ctx.moviegenre.Where(a => a.MovieId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();  
                foreach(var mgenre in genresToDeleted)
                {
                    ctx.moviegenre.Remove(mgenre);
                }
                foreach(int genId in model.Genres)
                {
                    var movieGenre = ctx.moviegenre.FirstOrDefault(a => a.MovieId == model.Id && a.GenreId == genId);
                    if(movieGenre == null)
                    {
                        movieGenre = new MovieGenre
                        {
                            GenreId = genId,
                            MovieId = model.Id,
                        };
                        ctx.moviegenre.Add(movieGenre);
                    }
                }
                ctx.movies.Update(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<int> GetGenreByMovieId(int movieID)
        {
            var genreIds = ctx.moviegenre.Where(a => a.MovieId == movieID).Select(a => a.GenreId).ToList();
            return genreIds;
        }
    }
}
