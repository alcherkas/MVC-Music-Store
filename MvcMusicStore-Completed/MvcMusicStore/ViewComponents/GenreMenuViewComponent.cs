using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMusicStore.Models;

namespace MvcMusicStore.ViewComponents
{
    public class GenreMenuViewComponent : ViewComponent
    {
        private readonly MusicStoreEntities _storeDB;

        public GenreMenuViewComponent(MusicStoreEntities storeDB)
        {
            _storeDB = storeDB;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await _storeDB.Genres.ToListAsync();
            
            return View(genres);
        }
    }
}