using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using UploadApp.Shared;

namespace Hawaso.Pages.Customers
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private ILogger<Details> Logger { get; set; }

        [Inject]
        public IReplyRepository ReplyRepositoryReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IFileStorageManager UploadAppFileStorageManager { get; set; }

        protected string content = "";
        protected string folderPath = "";

        protected Reply model = new Reply();

        protected override async Task OnInitializedAsync()
        {
            folderPath = UploadAppFileStorageManager.GetFolderPath("", Id.ToString(), "");
            model = await ReplyRepositoryReference.GetByIdAsync(Id);
            Logger.LogInformation($"※※※ id: ${Id}, model 불러오기 , {JsonSerializer.Serialize(model)}");
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
        }
    }
}
