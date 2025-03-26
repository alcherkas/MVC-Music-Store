using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly MusicStoreEntities _storeDB;
        const string PromoCode = "FREE";

        public CheckoutController(MusicStoreEntities storeDB)
        {
            _storeDB = storeDB;
        }

        //
        // GET: /Checkout/AddressAndPayment

        public IActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddressAndPayment(Order order, string promoCode)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.Equals(promoCode, PromoCode, StringComparison.OrdinalIgnoreCase) == false)
                    {
                        return View(order);
                    }
                    else
                    {
                        order.Username = User.Identity.Name;
                        order.OrderDate = DateTime.Now;

                        //Save Order
                        _storeDB.Orders.Add(order);
                        _storeDB.SaveChanges();

                        //Process the order
                        var cart = ShoppingCart.GetCart(_storeDB, HttpContext);
                        cart.CreateOrder(order);

                        return RedirectToAction("Complete", new { id = order.OrderId });
                    }
                }
                catch
                {
                    //Invalid - redisplay with errors
                    return View(order);
                }
            }
            
            return View(order);
        }

        //
        // GET: /Checkout/Complete

        public IActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = _storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}