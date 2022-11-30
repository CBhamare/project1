using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesList.Models.Domain;
using MoviesList.Repositories.Abstract;

namespace MoviesList.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genreService;
        public MovieController(IMovieService MovieService, IFileService fileService, IGenreService genreService)
        {
            _movieService = MovieService;
            _fileService = fileService;
            _genreService = genreService;
        }
        public IActionResult Add()
        {
            var model = new Movie();
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text=a.GenreName,Value=a.ID.ToString()});
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Movie model)
        {
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.ID.ToString() });
            //if (!ModelState.IsValid)
            //    return View(model);

            if (model.ImageFile != null) 
            {
            var fileresult = this._fileService.SaveImage(model.ImageFile);
            if(fileresult.Item1==0)
            {
                TempData["msg"] = "File Could Not Saved";
            }
            var imageName = fileresult.Item2;
            model.MovieImage = imageName;
            }
            var result = _movieService.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfuly!";
                return RedirectToAction(nameof(Add));
            }else
            {
                TempData["msg"] = "Error On Server Side";
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var model = _movieService.GetById(id);
            var selectGenres = _movieService.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genreService.List(),"ID","GenreName", selectGenres);
            model.MultiGenreList = multiGenreList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Movie model)
        {
            var selectGenres = _movieService.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genreService.List(), "ID", "GenreName", selectGenres);
            model.MultiGenreList = multiGenreList;
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileresult = this._fileService.SaveImage(model.ImageFile);
                if (fileresult.Item1 == 0)
                {
                    TempData["msg"] = "File Could Not Saved";
                }
                var imageName = fileresult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieService.Update(model);
            if (result)
            {
                TempData["msg"] = "Movie Updated Successfuly!";
                return RedirectToAction(nameof(MovieList));
            }
            else
            {
                TempData["msg"] = "Error On Server Side";
                return View(model);
            }

        }
        public IActionResult MovieList()
            {
            var data = this._movieService.List();
                return View(data);
            }
        public IActionResult Delete(int id)
        {
            var result = this._movieService.Delete(id);
            return RedirectToAction(nameof(MovieList));
        }

    }
}
