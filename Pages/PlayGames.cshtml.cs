using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PressStart.Data;
using PressStart.Models;

namespace PressStart.Pages
{
    public class PlayGamesModel : PageModel
    {

        private readonly PressStartContext db;
        
        public PlayGamesModel(PressStartContext db) => this.db = db;

        [BindProperty(SupportsGet = true)]
        

        public int Id{get; set;}
        public async Task OnGetAsync() => Game = await db.Games.FindAsync(Id);

        public Game Game {get;set;}

        public Comment comment {get;set;}

        public async Task onGetAsync(int? id)
        {
            Game = await db.Games.FirstOrDefaultAsync(m => m.GameId == id);
        }

        
        public List<Comment> comments{get;set;}= new List<Comment>();

        //public async Task onGetAsync() => comments = await db.Comments.FirstOrDefault(x => x.GameId == Id);
    }
}
