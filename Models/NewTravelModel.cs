using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Express.Models
{
    public class NewTravelModel
    {
        public NewTravelModel()
        {
            
        }

        public int FromNumber { get; set; }
        public string FromStreet { get; set; }
        public String FromComp { get; set; }
        public String FromCity { get; set; }
        public String FromPostalCode { get; set; }
        public String FromCountry { get; set; }
       
        public DateTime date1 { get; set; }
        public String Depart { get; set; }
        public int ToNumber { get; set; }
        public String ToStreet { get; set; }
        public String ToComp { get; set; }
        public String ToCity { get; set; }
        public String ToPostalCode { get; set; }
        public String ToCountry { get; set; }
        
        public DateTime date2 { get; set; }
       
        //public DateTime? TimeStart { get; set; }
        //public DateTime? TimeEnd { get; set; }
        public int Seats { get; set; }

       
    }
}
