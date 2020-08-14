using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MarinaCargo.Models;


namespace MarinaCargo.Models
{
    public class EFOrderRepo : IOrderRepo
    {
        private ApplicationDbContext context;
        public EFOrderRepo(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);

        public void SaveOrder(Order order, string userID)
        {
            order.UserID = userID;

            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }    
            context.SaveChanges();
        }
    }
}
