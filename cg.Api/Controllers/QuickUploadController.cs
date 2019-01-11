using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using AutoMapper;
using cg.Api.Controllers.DTO;
using cg.Api.Core;
using cg.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cg.Api.Controllers
{

    [Route("api/QuickUpload")]
    public class QuickUploadController : Controller
    {
        private static int DefaultBufferSize = 80 * 1024;

        public QuickUploadController()
        {

        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async System.Threading.Tasks.Task<IActionResult> UploadImageAsync(IFormFile upload, string directory = "assets/images", 
                CancellationToken cancellationToken = default(CancellationToken))
        {
            if (upload == null)
            {
                return BadRequest("File is empty");
            }

            if (!Directory.Exists(directory))
            {
                // Try to create the directory.
                Directory.CreateDirectory(directory);
            }
            
            var fileName = Path.GetFileName(upload.FileName);

            using (var fileStream = new FileStream(Path.Combine(directory, $"{fileName}"), FileMode.Create))
            {
                var inputStream = upload.OpenReadStream();
                await inputStream.CopyToAsync(fileStream, DefaultBufferSize, cancellationToken);
            }

            var url = Path.Combine("http://localhost:5001/MyImages/", fileName);
            return new JsonResult(new { fileName = fileName, uploaded = 1, url = url, error = new { } });
        }
    }
}