using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travel_Express.Database;
using Travel_Express.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Travel_Express.Controllers
{
    /// <summary>
    /// This Class controls anything related to log in, log out or Sign in
    /// </summary>
    public class AccountController : Controller
    {
        private readonly Context _context;

        public AccountController(Context context)
        {
            _context = context;
        }

        // go to the sign in page
        public IActionResult Signup()
        {
            return View();
        }

        // go to the log in page
        public IActionResult Login()
        {
            return View();
        }

        // log out
        //[Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public bool IsLogged()
        {
            return User.Identity.IsAuthenticated;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateLogin([Bind("Mail,Password")] NewAccount account)
        {
            //we check wether the user exists and has the same password
            var users = await _context.Users.FindAsync(account.Mail);
            if (users == null)
            {
                return RedirectToAction("Login");
            }
            if(users.PasswordHash == EncodePassword(account.Password))
            {
                //Cookie Instanciation
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Mail),
                    new Claim(ClaimTypes.Email, account.Mail),
                    //new Claim(ClaimTypes.Role, "Administrator"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateSignup([Bind("Mail,Password")] NewAccount newAccount)
        {
            Users users = new Users() { Mail = newAccount.Mail, 
                                        PasswordHash = EncodePassword(newAccount.Password) };
            if (ModelState.IsValid)
            {
                bool invalidUsername = false;
                try
                {
                    // checks if the data can be written
                    _context.Add(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    invalidUsername = true;
                }
                catch (Npgsql.PostgresException)
                {
                    invalidUsername = true;
                }
                if (invalidUsername)
                {
                    return RedirectToAction("Signup");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Login");
        }

        public string EncodePassword(string password)
        {
            SHA256 mySHA256 = SHA256.Create();
            byte[] hashValue = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashValue)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
