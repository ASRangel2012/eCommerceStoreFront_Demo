using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinaCargo.Models;

namespace MarinaCargo.Models
{
    public interface IProductRepo
    {
        IEnumerable<Product> Products { get; }
    }
}
