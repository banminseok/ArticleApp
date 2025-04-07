using ArticleAppBlazorServer.Services;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace ArticleAppBlazorServer.Pages.FrmFileUploadTest
{
    public partial class FrmFileUploadTest
    {
        [Inject]
        public IFileUploadService FileUploadServiceReference { get; set; } = default!;

        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        private IFileListEntry[] selectedFiles;

        protected void HandleSelection(IFileListEntry[] files)
        {
            this.selectedFiles = files;
        }

        protected async void UploadClick()
        {
            var file = selectedFiles.FirstOrDefault();
            if (file != null)
            {
                await FileUploadServiceReference.UploadAsync(file);
                await JSRuntime.InvokeAsync<object>("alert", "완료.");
            }
        }
    }
}
