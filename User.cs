using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarinaCargo.Models
{
    public class User : IdentityUser
    {
        public int UserID { get; set; }
        public override string Email { get; set; }
        public string Zip { get; set; }
        public override string UserName { get; set; }
        public string Password { get; set; }
        public bool Premium { get; set; }
    }
}
