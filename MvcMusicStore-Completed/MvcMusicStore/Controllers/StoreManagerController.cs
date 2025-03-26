using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        private readonly MusicStoreEntities _db;

        public StoreManagerController(MusicStoreEntities db)
        {
            _db = db;
        }

        //
        // GET: /StoreManager/

        public IActionResult Index()
        {
            var albums = _db.Albums.Include(a => a.Genre).Include(a => a.Artist);
            return View(albums.ToList());
        }

        //
        // GET: /StoreManager/Details/5

        public IActionResult Details(int id)
        {
            Album album = _db.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        //
        // GET: /StoreManager/Create

        public IActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
            return View();
        } 

        //
        // POST: /StoreManager/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                _db.Albums.Add(album);
                _db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }
        
        //
        // GET: /StoreManager/Edit/5
 
        public IActionResult Edit(int id)
        {
            Album album = _db.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }
            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(album).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // GET: /StoreManager/Delete/5
 
        public IActionResult Delete(int id)
        {
            Album album = _db.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {            
            Album album = _db.Albums.Find(id);
            if (album != null)
            {
                _db.Albums.Remove(album);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}