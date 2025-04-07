using BlazorInputFile;

namespace ArticleAppBlazorServer.Services
{
    public interface IFileUploadService
    {
        Task UploadAsync(IFileListEntry file);
    }
}
