using Microsoft.AspNetCore.Mvc;
using SegundoExamenNetCore.Models;
using SegundoExamenNetCore.Repositories;
using System.Numerics;

namespace SegundoExamenNetCore.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoryComics repo;

        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult Find()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        [HttpPost]
        public IActionResult Find(int idComic)
        {
            Comic comic = this.repo.FindComicById(idComic);
            return View("ComicDetails", comic);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Comic comic)
        {
            this.repo.InsertComic(comic.Nombre, comic.Imagen, comic.Descripcion);
            ViewData["MENSAJE"] = "Comic creado!";
            return RedirectToAction("Index");
        }

        public IActionResult CreateLambda()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLambda(Comic comic)
        {
            this.repo.InsertComicLambda(comic.Nombre, comic.Imagen, comic.Descripcion);
            ViewData["MENSAJE"] = "Comic creado!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("Comics/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Comic comic = this.repo.FindComicById(id);
            return View(comic);
        }

        [HttpPost]
        [Route("Comics/Delete")] 
        public IActionResult DeleteConfirmed(int id)
        {
            this.repo.DeleteComic(id);
            return RedirectToAction("Index");
        }


    }
}

