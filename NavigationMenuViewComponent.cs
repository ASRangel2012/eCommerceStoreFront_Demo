using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarinaCargo.Models;

namespace MarinaCargo.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepo _repo;

        public NavigationMenuViewComponent(IProductRepo repo)
        {
            _repo = repo;
        }

        public IViewComponentResult Invoke()
        {
            return View(_repo.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
        }
    }
}
