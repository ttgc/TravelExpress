using System;
using System.Collections.Generic;

namespace Travel_Express.Database
{
    public partial class Address
    {
        public Address()
        {
            TravelFromNavigation = new HashSet<Travel>();
            TravelToNavigation = new HashSet<Travel>();
        }

        public int IdAddress { get; set; }
        public int? Number { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Travel> TravelFromNavigation { get; set; }
        public virtual ICollection<Travel> TravelToNavigation { get; set; }
    }
}
