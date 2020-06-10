using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Travel_Express.Controllers
{
    public class TravelRegisteryController : Controller
    {
        public Travel_Express.Database.Context _context;

        public TravelRegisteryController(Travel_Express.Database.Context context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult RegisterTravel()
        {
            System.Diagnostics.Debug.WriteLine("Function Used");
            return View();
        }

        [HttpPost]
        [Route("/TravelRegistery/ConfirmRegister", Name = "RegisterTravel")]
        [Authorize]
        //public ActionResult ConfirmNewTravel()
        //public async Task<ActionResult> ConfirmNewTravel([Bind("starting_street,start_comp,start_code,time1_h,time1, arrival_street,arrival_comp,arrival_code,time2_h,time2,seats")] Travel_Express.Models.NewTravelModel newTravel)
        public async Task<ActionResult> ConfirmNewTravel( Travel_Express.Models.NewTravelModel newTravel)
        {
            System.Diagnostics.Debug.WriteLine("There is:"+newTravel.Seats+" Seats for a travel going from "+newTravel.FromStreet);
            Travel_Express.Database.Address From = new Travel_Express.Database.Address();
            Travel_Express.Database.Address To = new Travel_Express.Database.Address();
            From.Number = newTravel.FromNumber;
            From.Street = newTravel.FromStreet;
            From.Complement = newTravel.FromComp;
            From.PostalCode = newTravel.FromPostalCode;
            From.City = newTravel.FromCity;
            From.Country = newTravel.FromCountry;

            To.Number = newTravel.ToNumber;
            To.Street = newTravel.ToStreet;
            To.Complement = newTravel.ToComp;
            To.PostalCode = newTravel.ToPostalCode;
            To.City = newTravel.ToCity;
            To.Country = newTravel.ToCountry;

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry bddFrom =_context.Add(From);
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry bddTo =_context.Add(To);

            _context.SaveChanges();

            Travel_Express.Database.Travel travel = new Travel_Express.Database.Travel();
            travel.From = ((Travel_Express.Database.Address)bddFrom.Entity).IdAddress;
            travel.To = ((Travel_Express.Database.Address)bddTo.Entity).IdAddress;
            travel.Driver = User.Identity.Name; //user email or name
            travel.Seats = newTravel.Seats;
            travel.TimeStart = newTravel.date1;
            travel.TimeEnd = newTravel.date2;
            /*System.DateTime start = new System.DateTime()
            travel.TimeStart = newTravel.date1;
            travel.TimeEnd = newTravel.date2;*/
            _context.Add(travel);
            try
            {
                await _context.SaveChangesAsync();
                //_context.SaveChanges();
            } catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return RedirectToAction("RegisterTravel", "TravelRegistery");
            }

            return View();
        }
    }
}
