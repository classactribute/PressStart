using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using PressStart.Data;

namespace PressStart.Pages.Admin.Games
{

    [Authorize(Roles = "Admin")]
    public class AddGameModel : PageModel
    {
        private PressStartContext db;
    private readonly ILogger<RegisterModel> logger;
    public AddGameModel(PressStartContext db, ILogger<RegisterModel> logger) {
        this.db = db;
        this.logger = logger;
    }

    [BindProperty, Required, MinLength(1), MaxLength(500), Display(Name = "GameName")]
    public string GameName { get; set; }

    [BindProperty, Required, MinLength(1), MaxLength(150), Display(Name = "GameType")]
    public string GameType { get; set; }

    [BindProperty, Required, MinLength(1), MaxLength(2000), Display(Name = "GamePath")]
    public string GamePath { get; set; }

    [BindProperty]
    public string ThumbnailPath { get; set; }

    [BindProperty]
    public string Description { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        //var userName = User.Identity.Name; // userName is email
        //var user = db.Users.Where(u => u.UserID == userName).FirstOrDefault(); // find user record
        var newGame = new PressStart.Models.Game { GameName = GameName, GameType = GameType, GamePath = GamePath, ThumbnailPath = ThumbnailPath, Description = Description };
        db.Add(newGame);
        await db.SaveChangesAsync();
        return RedirectToPage("AddGameSuccess");
      }
      return Page();
    }
    }
}
