using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly MusicStoreEntities _storeDB;

        public StoreController(MusicStoreEntities storeDB)
        {
            _storeDB = storeDB;
        }

        //
        // GET: /Store/

        public IActionResult Index()
        {
            var genres = _storeDB.Genres.ToList();

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco

        public IActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database
            var genreModel = _storeDB.Genres
                .Include(g => g.Albums)
                .SingleOrDefault(g => g.Name == genre);

            if (genreModel == null)
            {
                return NotFound();
            }

            return View(genreModel);
        }

        //
        // GET: /Store/Details/5

        public IActionResult Details(int id)
        {
            var album = _storeDB.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefault(a => a.AlbumId == id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }
    }
}