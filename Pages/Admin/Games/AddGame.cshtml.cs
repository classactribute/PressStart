using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using PressStart.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using PressStart.Data.Controllers;
using Amazon.S3.Model;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;


namespace PressStart.Pages.Admin.Games
{


  

    [Authorize(Roles = "Admin")]
    public class AddGameModel : PageModel
    {
        private PressStartContext db;
    private readonly ILogger<RegisterModel> logger;

    
    private IHostingEnvironment _environment;

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

    [BindProperty]
    
    public IFormFile UploadThumb {get;set;}

    [BindProperty]

    public IFormFile UploadRom { get; set; }




        public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        //var userName = User.Identity.Name; // userName is email
        //var user = db.Users.Where(u => u.UserID == userName).FirstOrDefault(); // find user record
        string ThumbnailPath = null;
        
        if(UploadThumb != null){
        
        //optional
        string fileExtension = Path.GetExtension(UploadThumb.FileName).ToLower();
        
        
        var file = Path.Combine(_environment.ContentRootPath,"wwwroot","Uploads", UploadThumb.FileName );
        
        
        using(var fileStream = new FileStream(file, FileMode.Create))
        {
          await UploadThumb.CopyToAsync(fileStream);
          
        }
        
        ThumbnailPath = Path.Combine("Uploads", UploadThumb.FileName);
        }
        
        if(UploadRom != null){
            var bucketName = "presssroms";
            var AWSKey = keys.AWSKey;
            var AWSSecret = keys.AWSSecret;
            var AWSRegion = RegionEndpoint.GetBySystemName("us-east-1");
            var cred = new BasicAWSCredentials(AWSKey, AWSSecret);
            var client = new AmazonS3Client(cred, AWSRegion);
            var filename = UploadRom.FileName;
            PutObjectRequest request = new PutObjectRequest()
                {
                 InputStream = UploadRom.OpenReadStream(),
                 BucketName = "presssroms",
                 Key = filename
                };
            PutObjectResponse response = await client.PutObjectAsync(request);
            GamePath = Path.Combine("https://presssroms.s3.amazonaws.com/", UploadRom.FileName);

                    
          }

                
        
        var newGame = new PressStart.Models.Game { GameName = GameName, GameType = GameType, GamePath = GamePath , ThumbnailPath = ThumbnailPath, Description = Description };
        
        db.Add(newGame);
        await db.SaveChangesAsync();
        return RedirectToPage("AddGameSuccess");
      }
      return Page();  
      
      
    }
  }
}

