using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace PressStart.Pages
{
    public class SubscribeModel : PageModel
    {
        public void OnGet()
        {
        }
    }

    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;

        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Subscribe()
        {
            ViewBag.stripeKey = _configuration["Stripe:publishable_key"];
            return View();
        }

    }
}
