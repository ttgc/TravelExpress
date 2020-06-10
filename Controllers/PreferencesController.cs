using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel_Express.Database;
using Microsoft.EntityFrameworkCore;
using Travel_Express.Models;

namespace Travel_Express.Controllers
{
    public class PreferencesController : Controller
    {
        private readonly Context _context;

        public PreferencesController(Context context)
        {
            _context = context;
        }

        private string UserEmail()
        {
            return User.Identity.Name;
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            return View();
        }

        // GET: return the current account preferences
        public async Task<Preferences> GetPrefs()
        {
            string id = UserEmail();
            if (id == null)
            {
                return null;
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return null;
            }
            return new Preferences()
            {
                AcceptDeviation = (bool)users.AcceptDeviation,
                AcceptEveryone = (bool)users.AcceptEveryone,
                AcceptMusic = (bool)users.AcceptMusic,
                AcceptPet = (bool)users.AcceptPet,
                AcceptSmoke = (bool)users.AcceptSmoke,
                AcceptTalking = (bool)users.AcceptTalking,
            };
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([Bind("AcceptPet,AcceptSmoke,AcceptMusic,AcceptTalking,AcceptDeviation,AcceptEveryone")] Preferences prefs)
        {
            string id = UserEmail();
            var users = await _context.Users.FindAsync(id);
            if(users == null)
                return NotFound();

            users.AcceptDeviation = prefs.AcceptDeviation;
            users.AcceptEveryone = prefs.AcceptEveryone;
            users.AcceptMusic = prefs.AcceptMusic;
            users.AcceptPet = prefs.AcceptPet;
            users.AcceptSmoke = prefs.AcceptSmoke;
            users.AcceptTalking = prefs.AcceptTalking;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Mail))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("MyAccount");
        }

        private bool UsersExists(string id)
        {
            return _context.Users.Any(e => e.Mail == id);
        }
    }
}
