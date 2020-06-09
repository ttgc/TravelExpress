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
    public class IndexModel : PageModel
    {
        private readonly Travel_Express.Database.Context _context;

        public IndexModel(Travel_Express.Database.Context context)
        {
            _context = context;
        }

        public IList<Users> Users { get;set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }
    }
}
