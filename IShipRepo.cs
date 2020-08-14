using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarinaCargo.Models;

namespace MarinaCargo.Models
{
    public interface IShipRepo
    {
        IEnumerable<ShipInfo> ShipInfoDB { get; }

        ShipInfo SearchResult(string userID);
        void SaveShip(ShipInfo shipInfo, string userID);
    }
}
