using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;

namespace MvcMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MusicStoreEntities _storeDB;
        private readonly HtmlEncoder _htmlEncoder;

        public ShoppingCartController(MusicStoreEntities storeDB, HtmlEncoder htmlEncoder)
        {
            _storeDB = storeDB;
            _htmlEncoder = htmlEncoder;
        }

        //
        // GET: /ShoppingCart/

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(_storeDB, HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public IActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedAlbum = _storeDB.Albums
                .SingleOrDefault(album => album.AlbumId == id);

            if (addedAlbum == null)
            {
                return NotFound();
            }

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(_storeDB, HttpContext);

            cart.AddToCart(addedAlbum);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(_storeDB, HttpContext);

            // Get the name of the album to display confirmation
            string albumName = _storeDB.Carts
                .Include(c => c.Album)
                .SingleOrDefault(item => item.RecordId == id)?.Album?.Title ?? "Item";

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = _htmlEncoder.Encode(albumName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }
    }
}