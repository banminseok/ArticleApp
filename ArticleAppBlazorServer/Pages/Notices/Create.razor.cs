using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ArticleAppBlazorServer.Pages.Notices
{
    public partial class Create
    {
        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        public INoticeRepositoryAsync NoticeRepositoryAsyncReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected Notice model = new Notice();
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
            await NoticeRepositoryAsyncReference.AddAsync(model);
            Logger.LogInformation($"※※※ model 저장 , {JsonSerializer.Serialize(model)}");
            NavigationManagerReference.NavigateTo("/Notices");
        }
    }
}
