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

        public AddGameModel(PressStartContext db, ILogger<RegisterModel> logger, IHostingEnvironment environment)
        {
            this.db = db;
            this.logger = logger;
            _environment = environment;
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

        public IFormFile UploadThumb { get; set; }


        [BindProperty]

        public IFormFile UploadRom { get; set; }




        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;

                if (UploadThumb != null)
                {
                    string fileExtension = Path.GetExtension(UploadThumb.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".gif", ".png" };
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        // Display error and the form again
                        ModelState.AddModelError(string.Empty, "Only image files (jpg, jpeg, gif, png) are allowed");
                        return Page();
                    }
                    // FIXME: sanitize the original name or assign random
                    var invalids = System.IO.Path.GetInvalidFileNameChars();
                    var newFileName = String.Join("_", UploadThumb.FileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
                    var destPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "Uploads", UploadThumb.FileName);
                    // FIXME: handle IO errors when copying the file
                    try
                    {
                        using (var fileStream = new FileStream(destPath, FileMode.Create))
                        {
                            UploadThumb.CopyTo(fileStream);
                        }
                    }
                    catch (Exception ex) when (ex is IOException || ex is SystemException)
                    {
                        // TODO: Log this as an error
                        ModelState.AddModelError(string.Empty, "Internal error saving the uploaded file");
                        return Page();
                    }
                    imagePath = Path.Combine("Uploads", newFileName);
                }
                
                // ********S3 FOR ROM************ \\
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
                    BucketName = bucketName,
                    Key = filename
                };

                PutObjectResponse response = await client.PutObjectAsync(request);

                GamePath = Path.Combine("https://presssroms.s3.amazonaws.com/", UploadRom.FileName);
               

                var newGame = new PressStart.Models.Game { GameName = GameName, GameType = GameType, GamePath = GamePath, ThumbnailPath = imagePath, Description = Description };
                db.Add(newGame);
                await db.SaveChangesAsync();

                return RedirectToPage("AddGameSuccess");
            }



            return Page();


        }
    }
}

