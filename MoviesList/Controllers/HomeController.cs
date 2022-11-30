using Microsoft.AspNetCore.Mvc;
using MoviesList.Repositories.Abstract;

namespace MoviesList.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;

        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Index(string term="", int Currentpage = 1)
        {
            var movies = _movieService.List(term,true, Currentpage);
            return View(movies);
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult MovieDetail(int movieId)
        {
            var movie = _movieService.GetById(movieId);
            return View(movie);
        }

    }
}
