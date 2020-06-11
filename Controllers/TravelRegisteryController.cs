using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Express.Database;
using Travel_Express.Models;

namespace Travel_Express.Controllers
{
    public class TravelRegisteryController : Controller
    {
        public Travel_Express.Database.Context _context;

        public TravelRegisteryController(Travel_Express.Database.Context context)
        {
            _context = context;
        }

        public int GetOccupiedSeats(Travel s)
        {
            return _context.Booking.Where(b => b.IdTravel == s.IdTravel).Sum(q => q.Seats).GetValueOrDefault();
        }

        // GET: TravelRegisteryController
        public IActionResult Index(string startCity, string endCity, DateTime? startTime)
        {
            //int occupiedSeats = _context.Booking.Where(s.ID).Sum(q => q.Seats).Value;
            IQueryable<Travel> query = _context.Travel.Where(
                s => s.Seats.Value > _context.Booking.Where(b => b.IdTravel == s.IdTravel).Sum(q => q.Seats).GetValueOrDefault()
            );

            if (startTime.HasValue) query = query.Where(s => s.TimeStart.Value >= startTime.Value);
            if (!String.IsNullOrEmpty(startCity))
            {
                query = query.Where(s => s.FromNavigation.City.ToLower() == startCity.ToLower());
            }
            if (!String.IsNullOrEmpty(endCity))
            {
                query = query.Where(s => s.ToNavigation.City.ToLower() == endCity.ToLower());
            }

            return View(
                query.Select(s => new TravelListModel() {
                    AvailableSeats = s.Seats.Value - _context.Booking.Where(b => b.IdTravel == s.IdTravel).Sum(q => q.Seats).GetValueOrDefault(),
                    TotalSeats = s.Seats.Value,
                    Driver = s.Driver, StartTime = s.TimeStart.Value, EndTime = s.TimeEnd.Value,
                    From = $"{s.FromNavigation.Number.GetValueOrDefault(0)} {s.FromNavigation.Street}\n{s.FromNavigation.Complement}\n{s.FromNavigation.PostalCode} {s.FromNavigation.City}\n({s.FromNavigation.State}) {s.FromNavigation.Country}",
                    To = $"{s.ToNavigation.Number.GetValueOrDefault(0)} {s.ToNavigation.Street}\n{s.ToNavigation.Complement}\n{s.ToNavigation.PostalCode} {s.ToNavigation.City}\n({s.ToNavigation.State}) {s.ToNavigation.Country}",
                    ID = s.IdTravel
                }).ToList()
            );
        }

        public Preferences GetPrefs(string id)
        {
            if (id == null)
            {
                return null;
            }

            var users = _context.Users.Find(id);
            if (users == null)
            {
                return null;
            }
            Preferences p = new Preferences()
            {
                AcceptDeviation = users.AcceptDeviation.GetValueOrDefault(),
                AcceptEveryone = users.AcceptEveryone.GetValueOrDefault(),
                AcceptMusic = users.AcceptMusic.GetValueOrDefault(),
                AcceptPet = users.AcceptPet.GetValueOrDefault(),
                AcceptSmoke = users.AcceptSmoke.GetValueOrDefault(),
                AcceptTalking = users.AcceptTalking.GetValueOrDefault(),
            };
            return p;
        }

        public IActionResult Details(int id)
        {
            System.Diagnostics.Debug.WriteLine("Function Used " + id);
            Travel s = _context.Travel.Find(id);
            IQueryable<Travel> query = _context.Travel.Where(
                s => s.IdTravel == id
            );
            int occupiedSeats = GetOccupiedSeats(s);
            //without Select method, null exception are thrown from the "FromNavigation" values, hence the following code
            TravelListModel trm = query.Select(s => new TravelListModel()
            {
                AvailableSeats = s.Seats.Value - occupiedSeats,
                TotalSeats = s.Seats.Value,
                Driver = s.Driver,
                StartTime = s.TimeStart.Value,
                EndTime = s.TimeEnd.Value,
                From = $"{s.FromNavigation.Number.GetValueOrDefault(0)} {s.FromNavigation.Street}\n{s.FromNavigation.Complement}\n{s.FromNavigation.PostalCode} {s.FromNavigation.City}\n({s.FromNavigation.State}) {s.FromNavigation.Country}",
                To = $"{s.ToNavigation.Number.GetValueOrDefault(0)} {s.ToNavigation.Street}\n{s.ToNavigation.Complement}\n{s.ToNavigation.PostalCode} {s.ToNavigation.City}\n({s.ToNavigation.State}) {s.ToNavigation.Country}",
                ID = s.IdTravel
            }).ToList()[0]; //only one instance should be stored here
            TravelDetailsModel tdm = new TravelDetailsModel(GetPrefs(s.Driver), trm);
            return View(tdm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DetailBook(int id, IFormCollection formFields)
        {
            return BookSeats(id, int.Parse(formFields["nb_Seats"]));
        }

        [Authorize]
        public ActionResult Book(int id)
        {
            return BookSeats(id, 1);
        }
        public ActionResult BookSeats(int id, int seats)
        {
            Booking book = new Booking();
            book.Author = User.Identity.Name;
            book.IdTravel = id;
            book.Seats = seats;
            book.Pending = true;
            _context.Add(book);
            bool fail = true;
            try
            {
                _context.SaveChanges();
                fail = false;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException excep)
            {

            }


            if (!fail)
            {
                ViewData["Message"] = "Votre voyage a bien été réservé! \n Vous avez reservé " + seats + " sièges";
                ViewData["Title"] = "Voyage réservé";
            }else{
                ViewData["Message"] = "Vous ne pouvez pas reserver plusieurs fois le même voyage!";
                ViewData["Title"] = "Voyage Déjà réservé";
            }
        

            return View("Book");
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
        public async Task<ActionResult> ConfirmNewTravel( NewTravelModel newTravel)
        {
            System.Diagnostics.Debug.WriteLine("There is:"+newTravel.Seats+" Seats for a travel going from "+newTravel.FromStreet);
            Address From = new Address();
            Address To = new Address();
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

            Travel travel = new Travel();
            travel.From = ((Address)bddFrom.Entity).IdAddress;
            travel.To = ((Address)bddTo.Entity).IdAddress;
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
