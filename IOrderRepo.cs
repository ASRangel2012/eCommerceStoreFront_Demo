using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinaCargo.Models;

namespace MarinaCargo.Models
{
    public interface IOrderRepo
    {
        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order, string userID);
    }
}
