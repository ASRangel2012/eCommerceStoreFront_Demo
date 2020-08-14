using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarinaCargo.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public float Price { get; set; }
        public string Category { get; set; }
        public Boolean Promo { get; set; }
    }


}