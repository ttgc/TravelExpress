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

       
        public string FromStreet { get; set; }
        public String FromComp { get; set; }
        public String FromPostalCode { get; set; }
        public String time1 { get; set; }
        public String date1 { get; set; }
        public String Depart { get; set; }
        public String ToStreet { get; set; }
        public String ToComp { get; set; }
        public String ToPostalCode { get; set; }
        public String time2 { get; set; }
        public String date2 { get; set; }
        public String Arrival { get; set; }
        //public DateTime? TimeStart { get; set; }
        //public DateTime? TimeEnd { get; set; }
        public int Seats { get; set; }

       
    }
}
