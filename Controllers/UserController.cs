using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MarinaCargo.Models;
using MarinaCargo.Models.ViewModels;

namespace MarinaCargo.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private SignInManager<User> _signManager;
        private UserManager<User> _UserManager;
        private IShipRepo _repo;
        public UserController(UserManager<User> UserManager, SignInManager<User> signManager, IShipRepo repo)
        {
            _UserManager = UserManager;
            _signManager = signManager;
            _repo = repo;

        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel model)
        { 
            if (ModelState.IsValid)
            {
                var User = new User { Email = model.Email, UserName = model.UserName, Zip = model.Zip };
                var result = await _UserManager.CreateAsync(User, model.Password);
                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(User, false);
                    return RedirectToAction("SignIn");
                }
                else
                {
                    foreach(var e in result.Errors)
                    {
                        ModelState.AddModelError("", e.Description);
                    }
                }
            }
                return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn(string returnUrl = "")
        {
            var model = new SignInViewModel { ReturnUrl = returnUrl };
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Username, model.Password, model.Remember, false);

                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Incorrect username/password.  Try again.");
            return View(model);
        }

        public async Task<RedirectResult> SignOut(string returnUrl = "/")
        {
            await _signManager.SignOutAsync();
            return (Redirect(returnUrl));
        }

        public ViewResult Summary()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount()
        {
            User _user = await _UserManager.FindByNameAsync(User.Identity.Name);

            if (_user != null)
            {
                EditViewModel user = new EditViewModel
                {
                    UserName = _user.UserName,
                    Email = _user.Email,
                    Zip = _user.Zip,
                };
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(EditViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                if(vmodel.CurrentPass == null) {
                    ModelState.AddModelError("", "Please enter your password.");
                    return View(vmodel);
                }

                var _user = await _UserManager.FindByNameAsync(User.Identity.Name);                
                var passIntegrity = await _UserManager.CheckPasswordAsync(_user, vmodel.CurrentPass);

                if (!passIntegrity) {
                    ModelState.AddModelError("", "Incorrect password. Try again.");
                    return View(vmodel);
                }
                _user.Email = vmodel.Email;
                _user.Zip = vmodel.Zip;
                _user.UserName = vmodel.UserName;
                             
                if(vmodel.NewPass != null) {
                    var resultPass = await _UserManager.ChangePasswordAsync(_user, vmodel.CurrentPass, vmodel.NewPass);

                    if (!resultPass.Succeeded)
                    {
                        ModelState.AddModelError("", "Something went wrong. Try again.");
                        return View(vmodel);
                    }
                }
                var resultMain = await _UserManager.UpdateAsync(_user);

                if (resultMain.Succeeded)
                {
                    return RedirectToAction("Summary", "User");
                    
                }
            }
            return View(vmodel);
        }

        [HttpGet]
        public async Task<IActionResult> EditShip()
        {
            var _user = await _UserManager.FindByNameAsync(User.Identity.Name);

            ShipInfo preShip = _repo.SearchResult(_user.UserName.ToUpper());
            ShipInfo newShip = new ShipInfo { };
            if(preShip != null)
            {
                return View(preShip);
            }
            newShip.Zip = _user.Zip;
            return View(newShip);
        }

        [HttpPost]
        public IActionResult EditShip(ShipInfo shipInfo)
        {
            if (ModelState.IsValid)
            {
                _repo.SaveShip(shipInfo, User.Identity.Name.ToString().ToUpper());
                return RedirectToAction("Summary");
            }
            else
            {
                return View("EditShip");
            }
        }
    }
}
