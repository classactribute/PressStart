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

        public async Task OnGetAsync()
        {
            Game = await db.Games.FindAsync(Id);
            CommentList = await db.Comments.ToListAsync();
            UserList = await db.Users.ToListAsync();
        }     

        public Game Game {get;set;}

        [BindProperty]
        public Comment Comment {get;set;}

        public Microsoft.AspNetCore.Identity.IdentityUser ThisUser {get; set; }

        // [BindProperty]
        // public int Rating {get; set; }

        public string CommentText {get; set; }

        public List<Comment> CommentList {get;set;}= new List<Comment>();
        public List<Microsoft.AspNetCore.Identity.IdentityUser> UserList { get; set; } = new List<Microsoft.AspNetCore.Identity.IdentityUser>(); 

        public InputModel CommentInput { get; set; }

        // public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "User")]
            public string User { get; set; }

            [Required]
            [Display(Name = "CommentText")]
            public string CommentText { get; set; }

            [Required]
            [Display(Name = "Game")]
            public string Game { get; set; }
        }    
 
        // public async Task onGetAsync(int? id)
        // {
        //     Game = await db.Games.FirstOrDefaultAsync(m => m.GameId == id);
        //     NewComment = await db.Comments.Include(m => m.CommentText == CommentText).Include(m => m.Rating == Rating).FirstOrDefaultAsync(m => m.CommentId == id);
        // }
        public async Task<IActionResult> onGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Game = await db.Games.FirstOrDefaultAsync(m => m.GameId == id);
            Comment = await db.Comments.FirstOrDefaultAsync(m => m.CommentId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (ModelState.IsValid)
            {
                return Page();
            }
            var Comment = new PressStart.Models.Comment { CommentText = CommentInput.CommentText, User = ThisUser, Game = Game };
            db.Comments.Add(Comment);
            await db.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
