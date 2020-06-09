using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Travel_Express.Database;

namespace Travel_Express.Views.Account
{
    public class DetailsModel : PageModel
    {
        private readonly Travel_Express.Database.Context _context;

        public DetailsModel(Travel_Express.Database.Context context)
        {
            _context = context;
        }

        public Users Users { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Users = await _context.Users.FirstOrDefaultAsync(m => m.Mail == id);

            if (Users == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
