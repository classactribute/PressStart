using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PressStart.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Amazon.S3.Model;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using System;

namespace PressStart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();

            var bucketName = "presssroms";
            var AWSKey = keys.AWSKey;
            var AWSSecret = keys.AWSSecret;
            var AWSRegion = RegionEndpoint.GetBySystemName("us-east-1");
            var cred = new BasicAWSCredentials(AWSKey, AWSSecret);
            var client = new AmazonS3Client(cred, AWSRegion);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    try
                    {
                        var filename = formFile.FileName;
                        PutObjectRequest request = new PutObjectRequest()
                        {
                            InputStream = formFile.OpenReadStream(),
                            BucketName = "presssroms",
                            Key = filename
                        };

                        PutObjectResponse response = await client.PutObjectAsync(request);
                    }
                    catch (Exception ex)
                    {
                        return NotFound(new { count = files.Count, size, filePaths });
                    }

                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, filePaths });
        }
    }
}