
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace DotNetNote.Controllers
{
    public class CKEditorController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;

        public CKEditorController(IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            _environment = environment;
            this._logger = loggerFactory.CreateLogger(nameof(CKEditorController));
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string editor1)
        {
            ViewBag.EditorText = editor1;
            return View();
        }

        #region Upload 이미지 + Ckeditor
        public IActionResult PostWrite()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PostWrite(string title, string editor)
        {
            ViewBag.EditorText = $"{title}<hr />{editor}";
            return View();
        }
        #endregion


        /// <summary>
        /// CKEditor 이미지 업로드 처리 공식과 같은 코드
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="CKEditorFuncNum"></param>
        /// <param name="CKEditor"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImage(
            ICollection<IFormFile> upload,
            string CKEditorFuncNum,
            string CKEditor,
            string langCode
            )
        {
            if (string.IsNullOrEmpty(CKEditorFuncNum))
            {
                // 로그 남기기 또는 예외 처리
                _logger.LogInformation("CKEditorFuncNum is null or empty.");

               
            }

            string imgPath = "";
            string msg = string.Empty;
            //string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPath);
            string uploadFolder = Path.Combine(_environment.WebRootPath, "files");
            try
            {
                foreach (var file in upload)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(
                       DateTime.Now.ToString("yyyyMMdd-HHMMssff") + "-"
                       + ContentDispositionHeaderValue.Parse(
                           //file.ContentDisposition).FileName.Trim('"'));
                           file.ContentDisposition).FileName.ToString().Replace("\"", "").Trim());

                        using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        imgPath = Url.Content("/files/" + fileName);
                        msg = "이미지가 정상적으로 업로드 되었습니다.";

                    }
                }
            }
            catch (Exception ex)
            {
                msg = "오류가 발생했습니다. 오류메시지: " + ex.Message;
            }
            string r = $"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, \"{imgPath}\", \"{msg}\");</script>";

            return Content(r, "text/html");
        }
    }
}
