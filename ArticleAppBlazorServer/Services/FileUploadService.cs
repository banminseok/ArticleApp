using BlazorInputFile;

namespace ArticleAppBlazorServer.Services
{
    public class FileUploadService : IFileUploadService
    {

        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment env)
        {
            this._environment = env;
        }

        public async Task UploadAsync(IFileListEntry fileEntry)
        {
            var path = Path.Combine(_environment.WebRootPath, "Upload", fileEntry.Name);
            var ms = new MemoryStream(); 
            await fileEntry.Data.CopyToAsync(ms); // MemoryStream을 사용하여 메모리에 파일을 저장(복사)합니다.
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(file);
            }
        }
    }
}
