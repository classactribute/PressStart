using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authorization;

namespace PressStart.Pages
{
    public class AdminIndexModel : PageModel
    {
        [Authorize(Roles = "Admin")]
        public void OnGet()
        {
        }
    }
}
