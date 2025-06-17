using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace DotNetNote.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiFileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public WebApiFileUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        [Consumes("application/json", "multipart/form-data")]
        // files 매개변수 이름은 <input type="file" name="files" /> 
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            // 파일을 업로드할 폴더: wwwroot\\files
            var uploadFolder = Path.Combine(_environment.WebRootPath, "files");

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // 파일명 
                    var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));

                    using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return Ok(new { message = "OK" });
        }
    }
}
