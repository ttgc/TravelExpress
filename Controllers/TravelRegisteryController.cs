﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: TravelRegisteryController
        public async Task<IActionResult> Index(string startCity, string endCity, DateTime? startTime)
        {
            int occupiedSeats = _context.Booking.Sum(q => q.Seats).Value;
            IQueryable<Travel> query = _context.Travel.Where(
                s => s.Seats > occupiedSeats
            );

            if (startTime.HasValue) query = query.Where(s => s.TimeStart.GetValueOrDefault() >= startTime.Value);
            if (!String.IsNullOrEmpty(startCity))
            {
                query = query.Where(s => _context.Address.Find(s.From).City.ToLower() == startCity.ToLower());
            }
            if (!String.IsNullOrEmpty(endCity))
            {
                query = query.Where(s => _context.Address.Find(s.To).City.ToLower() == endCity.ToLower());
            }

            return View(
                await query.Select(s => new TravelListModel() {
                    AvailableSeats = s.Seats.Value - occupiedSeats, TotalSeats = s.Seats.Value,
                    Driver = s.Driver, StartTime = s.TimeStart.Value, EndTime = s.TimeEnd.Value,
                    From = string.Format(
                        "%d %s\n%s\n%s %s\n(%s) %s",
                        s.FromNavigation.Number.GetValueOrDefault(0),
                        s.FromNavigation.Street,
                        s.FromNavigation.Complement,
                        s.FromNavigation.PostalCode,
                        s.FromNavigation.City,
                        s.FromNavigation.State,
                        s.FromNavigation.Country
                    ), To = string.Format(
                        "%d %s\n%s\n%s %s\n(%s) %s",
                        s.ToNavigation.Number.GetValueOrDefault(0),
                        s.ToNavigation.Street,
                        s.ToNavigation.Complement,
                        s.ToNavigation.PostalCode,
                        s.ToNavigation.City,
                        s.ToNavigation.State,
                        s.ToNavigation.Country
                    ), ID = s.IdTravel
                }).ToListAsync()
            );
        }

        // GET: TravelRegisteryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TravelRegisteryController/Create
        public ActionResult Create()
        {
            return View();
        }


        public IActionResult RegisterTravel()
        {
            System.Diagnostics.Debug.WriteLine("Function Used");
            return View();
        }

        [HttpPost]
        [Route("/TravelRegistery/ConfirmRegister", Name = "RegisterTravel")]

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
            travel.Driver = "dummy"; //TODO replace dummy value
            travel.Seats = newTravel.Seats;
            travel.TimeStart = newTravel.date1;
            travel.TimeEnd = newTravel.date2;
            /*System.DateTime start = new System.DateTime()
            travel.TimeStart = newTravel.date1;
            travel.TimeEnd = newTravel.date2;*/
            _context.Add(travel);
            try
            {
                _context.SaveChanges();
            }catch(Microsoft.EntityFrameworkCore.DbUpdateException exception)
            {
                Travel_Express.Database.Users driver = new Database.Users();
                driver.Mail = "dummy";
                _context.Add(driver);
                _context.SaveChanges();
            }

            return View();
        }


        
        // POST: TravelRegisteryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TravelRegisteryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TravelRegisteryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TravelRegisteryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TravelRegisteryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
