using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesList.Models.Domain;
using MoviesList.Repositories.Abstract;

namespace MoviesList.Controllers
{
    [Authorize]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Genre model)
        {
            if(!ModelState.IsValid)
                return View(model);
            var result = _genreService.Add(model);
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
            var data = _genreService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(Genre model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = _genreService.Update(model);
            if (result)
            {
                TempData["msg"] = "Movie Added Successfuly!";
                return RedirectToAction(nameof(GenreList));
            }
            else
            {
                TempData["msg"] = "Error On Server Side";
                return View(model);
            }

        }
        public IActionResult GenreList()
            {
            var data = this._genreService.List().ToList();
                return View(data);
            }
        public IActionResult Delete(int id)
        {
            var result = this._genreService.Delete(id);
            return RedirectToAction(nameof(GenreList));
        }

    }
}
