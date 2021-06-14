using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PressStart.Data;
using PressStart.Models;
using Microsoft.AspNetCore.Identity;

namespace PressStart.Pages
{
    [Authorize]
    public class PlayGamesModel : PageModel
    {

        private readonly PressStartContext db;
        
        public PlayGamesModel(PressStartContext db)
        {
            this.db = db;
        } 

        [BindProperty(SupportsGet = true)]
        

        public int Id{get; set;}
        public async Task OnGetAsync() => Game = await db.Games.FindAsync(Id);

        public Game Game {get;set;}

        public Comment newComment {get;set;}

        [BindProperty]
        public int Rating {get; set; }

        [BindProperty, Required, MinLength(1), MaxLength(2000), Display(Name = "CommentText") ]
        public string CommentText {get; set; }
        public Microsoft.AspNetCore.Identity.IdentityUser ThisUser {get; set; }

        public async Task onGetAsync(int? id)
        {
            Game = await db.Games.FirstOrDefaultAsync(m => m.GameId == id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                return Page();
            }
            
            db.Comments.Add(newComment);
            await db.SaveChangesAsync();

            return RedirectToPage($"/PlayGames");
        }

        
        public List<Comment> comments{get;set;}= new List<Comment>();

        //public async Task onGetAsync() => comments = await db.Comments.FirstOrDefault(x => x.GameId == Id);
    }
}
