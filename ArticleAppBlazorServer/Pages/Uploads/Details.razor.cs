using ArticleApp.Models;
using ArticleAppBlazorServer.Services;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using UploadApp.Shared;

namespace ArticleAppBlazorServer.Pages.Uploads
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        public IUploadRepository UploadRepositoryReference{ get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IFileStorageManager UploadAppFileStorageManager { get; set; }

        protected string content = "";
        protected string folderPath = "";

        protected Upload model = new Upload();

        protected override async Task OnInitializedAsync()
        {
            folderPath = UploadAppFileStorageManager.GetFolderPath("", Id.ToString(), "");
            model = await UploadRepositoryReference.GetByIdAsync(Id);
            Logger.LogInformation($"※※※ id: ${Id}, model 불러오기 , {JsonSerializer.Serialize(model)}");
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
        }
    }
}
