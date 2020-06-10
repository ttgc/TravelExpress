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

namespace Travel_Express.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context;

        public AccountController(Context context)
        {
            _context = context;
        }

        // GET: Account/Create
        public IActionResult Signin()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateLogin([Bind("Mail,Password")] NewAccount account)
        {
            var users = await _context.Users.FindAsync(account.Mail);
            if (users == null)
            {
                return NotFound();
            }
            if(users.PasswordHash == EncodePassword(account.Password))
            {
                //login
                return RedirectToAction("Login");
            }
            else
            {
                //logout
                return NotFound();
            }
        }

        /*protected void isValidUser(object sender, EventArgs e)
        {
            int userId = 0;
            string conn_str = "Data Source=localhost;Initial Catalog=Books;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_ValidateUser";
                    cmd.Parameters.AddWithValue("@username", lgn.UserName);
                    cmd.Parameters.AddWithValue("@password", lgn.Password);
                    cmd.Connection = conn;
                    conn.Open();
                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
                switch (userId)
                {
                    case -1:
                        lgn.FailureText = "Wrong login information";
                        break;

                    default:
                        FormsAuthentication.RedirectFromLoginPage(lgn.UserName, lgn.RememberMeSet);
                        break;
                }
            }
        }*/

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateSignin([Bind("Mail,Password")] NewAccount newAccount)
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
                    return RedirectToAction("Signin");
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

        /*
        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Mail,PasswordHash,AcceptPet,AcceptSmoke,AcceptMusic,AcceptTalking,AcceptDeviation,AcceptEveryone")] Users users)
        {
            if (id != users.Mail)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Mail == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(string id)
        {
            return _context.Users.Any(e => e.Mail == id);
        }*/
    }
}
