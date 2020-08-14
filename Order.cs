using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MarinaCargo.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }

        public string UserID { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an address.")]
        public string Add1 { get; set; }
        public string Add2 { get; set; }

        [Required(ErrorMessage = "Please enter a city name.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter a state name.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter a ZIP code.")]
        public string Zip { get; set; }

    }
}
