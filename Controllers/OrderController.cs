using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using MarinaCargo.Models;
using MarinaCargo.Models.ViewModels;

namespace MarinaCargo.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IOrderRepo _repo;
        private IShipRepo _shipRepo;
        private Cart _cart;

        public OrderController(IOrderRepo repoService, IShipRepo shipRepoService, Cart cartService)
        {
            _repo = repoService;
            _shipRepo = shipRepoService;
            _cart = cartService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult CheckShip()
        {
            if (_cart.Lines.Count() == 0)
            {
                TempData["message"] = "Your cart is empty!";
                    return RedirectToAction("Index", "Cart");
            }
            var order = new Order { };
            if (User.Identity.IsAuthenticated)
            {
                order.UserID = User.Identity.Name.ToString().ToUpper();
                ShipInfo userInfo = _shipRepo.SearchResult(order.UserID);
                if(userInfo != null)
                {
                    order.Name = userInfo.FirstName + " " + userInfo.LastName;
                    order.Add1 = userInfo.Add1;
                    order.Add2 = userInfo.Add2;
                    order.City = userInfo.City;
                    order.State = userInfo.State;
                    order.Zip = userInfo.Zip;
                }
            }
            return View(order);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckShip(Order order)
        {
            if(_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "The cart is empty!");

            }
            if (ModelState.IsValid)
            {
                string id;
                if (User.Identity.IsAuthenticated)
                {
                    id = order.UserID = User.Identity.Name.ToString().ToUpper();
                }
                else
                {
                    id = "";
                }
                order.Lines = _cart.Lines.ToArray();
                _repo.SaveOrder(order, id);
                TempData["orderSuccess"] = "Your order was successfully placed!  Your order number is #" + order.OrderID.ToString() + ".";
                _cart.Clear();
                return RedirectToAction("Index","Home");
            }
            else
            {
                return View(order);
            }
        }
        
        [HttpGet]
        [AllowAnonymous]

        public ActionResult CheckBill()
        {
            var currentOrder = new BillViewModel();
            return View(currentOrder);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckBill(BillViewModel currentOrder)
        {
            if (ModelState.IsValid)
            {
                var order = currentOrder.GetOrder;
                _repo.SaveOrder(order, "");
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(currentOrder);
            }
        }

        [AllowAnonymous]
        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }

        public ViewResult ViewOrders(string category)
         => View(new OrdersListViewModel
         {
             Orders = _repo.Orders
             .Where(p => User.Identity.Name.ToString().ToUpper() == p.UserID)
             .OrderBy(p => p.OrderID)

         });
    }
}