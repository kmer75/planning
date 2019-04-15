using AutoMapper;
using PlanningApi.DTO;
using PlanningApi.Services;
using PlanningApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PlanningApi.Controllers
{
    //[Authorize(Policy = "API_ACCESS")]
    //[Authorize(Policy = "USER")]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class UploadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Post a media file
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostMediaFile([FromQuery] PostMediaFileFQViewModel vm)
        {
            try
            {
                var file = vm.File;
                if (file == null || file.Length == 0)
                    return Content("file not selected");

                string extension = file.ContentType;
                long size = file.Length;
                string folderName = "Upload"; 
                string ContentRootPath = _hostingEnvironment.ContentRootPath; //path of the project -> TODO has to be a path referenced into CONSTANT or config file

                string newPath = Path.Combine(ContentRootPath, folderName);

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fullPath = Path.Combine(newPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                return Ok(new { file });
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }



        /// <summary>
        /// remove a file by his name
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RemoveMediaFile(string filename)
        {
            try
            {
                
                string folderName = "Upload"; 
                string ContentRootPath = _hostingEnvironment.ContentRootPath; //path of the project

                string newPath = Path.Combine(ContentRootPath, folderName);

                if (!Directory.Exists(newPath))
                {
                    return NotFound("this directory doesn't exist");
                }
                
                string fullPath = Path.Combine(newPath, filename);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                return Ok("file removed with success");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }

    }
}
