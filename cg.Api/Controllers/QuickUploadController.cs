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
        private static string _uploadDirectory = "assets/images";
        private static string _defaultURLPath = "http://35.184.137.237:8915/MyImages/";
        private static int _defaultBufferSize = 80 * 1024;

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
            
            var fileName = CustomizeFileName(upload.FileName);

            using (var fileStream = new FileStream(Path.Combine(directory, $"{fileName}"), FileMode.Create))
            {
                var inputStream = upload.OpenReadStream();
                await inputStream.CopyToAsync(fileStream, _defaultBufferSize, cancellationToken);
            }

            var url = Path.Combine(_defaultURLPath, fileName);
            return new JsonResult(new { fileName = fileName, uploaded = 1, url = url, error = new { } });
        }


        private string CustomizeFileName(string fileName)
        {
            int fileTagNo = 1;
            int index = fileName.LastIndexOf('.');
            string extension = fileName.Substring(index);
            string name = fileName.Substring(0, index);

            fileName = name + "-" + fileTagNo + extension;
            while (System.IO.File.Exists(Path.Combine(_uploadDirectory, $"{fileName}")))
            {
                fileName = name + "-" + fileTagNo + extension;
                fileTagNo++;
            }
            return fileName;
        }
    }
}