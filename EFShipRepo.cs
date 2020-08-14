using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinaCargo.Models;


namespace MarinaCargo.Models
{
    public class EFShipRepo : IShipRepo
    {
        private ApplicationDbContext context;
        public EFShipRepo(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<ShipInfo> ShipInfoDB => context.ShipInfoDB;


        public ShipInfo SearchResult(string userID)
        {
            ShipInfo result = context.ShipInfoDB.FirstOrDefault(x => x.UserID.ToUpper() == userID.ToUpper());
            if (result != null)
                return result;
            return null;
        }
        public void SaveShip(ShipInfo shipInfo, string userID)
        {
            shipInfo.UserID = userID;

            if (shipInfo.ShipID == 0)
            {
                context.Add(shipInfo);
            }
            ShipInfo newEntry = context.ShipInfoDB.FirstOrDefault(p => p.UserID == shipInfo.UserID);

            if (newEntry != null)
            {
                newEntry.UserID = shipInfo.UserID;
                newEntry.FirstName = shipInfo.FirstName;
                newEntry.LastName = shipInfo.LastName;
                newEntry.Add1 = shipInfo.Add1;
                newEntry.Add2 = shipInfo.Add2;
                newEntry.City = shipInfo.City;
                newEntry.State = shipInfo.State;
                newEntry.Zip = shipInfo.Zip;                
            }
            context.SaveChanges();
        }
    }
}
