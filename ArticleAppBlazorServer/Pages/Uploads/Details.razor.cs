using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

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

        protected string content = "";

        protected Upload model = new Upload();

        protected override async Task OnInitializedAsync()
        {
            model = await UploadRepositoryReference.GetByIdAsync(Id);
            Logger.LogInformation($"※※※ model 불러오기 , {JsonSerializer.Serialize(model)}");
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
        }
    }
}
