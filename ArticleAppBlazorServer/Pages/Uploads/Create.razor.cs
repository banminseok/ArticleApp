using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ArticleAppBlazorServer.Pages.Uploads
{
    public partial class Create
    {
        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        public IUploadRepository UploadRepositoryReference{ get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected Upload model = new Upload();
        public string ParentId { get; set; }
        protected int[] parentIds = { 1, 2, 3 };

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected async void FormSubmit()
        {
            int.TryParse(ParentId, out int parentId);
            model.ParentId = parentId;
            await UploadRepositoryReference.AddAsync(model);
            Logger.LogInformation($"※※※ model 저장 , {JsonSerializer.Serialize(model)}");
            NavigationManagerReference.NavigateTo("/Uploads");
        }
    }
}
