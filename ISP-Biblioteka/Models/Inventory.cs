using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_Biblioteka.Models
{
    public class Inventory
    {
        public enum InventoryTypes
        {
            Knyga,
            Kompiuteris,
            Monitorius,
            Spausdintuvas
        }

        InventoryTypes type { get; set; }
        int id { get; set; }
        decimal cost { get; set; }
        DateTime registrationDate { get; set; }
        DateTime expirationDate { get; set; }

        Location loc;

        public Inventory(InventoryTypes type, decimal cost, DateTime registrationDate, DateTime expirationDate, Location loc)
        {
            this.type = type;
            this.cost = cost;
            this.registrationDate = registrationDate;
            this.expirationDate = expirationDate;

            if (loc.Equals(null))
            {
                this.loc = new Location("ERROR", -1, -1, -1);
            }
            else this.loc = loc;

            
        }

        public override string ToString()
        {
            return string.Format($"ID: {id} - {type} - {cost} - registruota {registrationDate}, galioja iki {expirationDate}");
        }
    }


}