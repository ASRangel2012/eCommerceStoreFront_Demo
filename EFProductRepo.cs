using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarinaCargo.Models
{
    public class EFProductRepo : IProductRepo
    {
        private ApplicationDbContext context;
        public EFProductRepo(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<Product> Products => context.Products;
    }
}