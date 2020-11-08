using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_Biblioteka.Models
{
    public class Location
    {
        string zone { get; set; }
        int id { get; set; }
        int shelfNumber { get; set; }
        int shelfRow { get; set; }

        public Location(string zoneName, int zoneID, int shelfNumber, int shelfRow)
        {
            this.zone = zoneName;
            this.id = zoneID;
            this.shelfNumber = shelfNumber;
            this.shelfRow = shelfRow;
        }

        public override string ToString()
        {
            return string.Format($"Zona {zone} (ID {id}), lentyna {shelfNumber}, eilė {shelfRow}");
        }
    }
}