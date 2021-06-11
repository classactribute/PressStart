using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authorization;
using PressStart.Data;
using PressStart.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace PressStart.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminIndexModel : PageModel
    {
        public List<Microsoft.AspNetCore.Identity.IdentityUser> UserList { get; set; } = new List<Microsoft.AspNetCore.Identity.IdentityUser>();
        public List<Game> GameList {get; set; } = new List<Game>();

        private readonly PressStartContext db;
        private readonly ILogger<AdminIndexModel> _logger;
        public AdminIndexModel(PressStartContext db, ILogger<AdminIndexModel> logger)
                {
                    this.db = db;
                    _logger = logger;
                }

        public async Task OnGetAsync()
        {
            UserList = await db.Users.ToListAsync();
            GameList = await db.Games.ToListAsync();
        }
    }
}
