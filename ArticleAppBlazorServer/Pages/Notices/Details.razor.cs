using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace ArticleAppBlazorServer.Pages.Notices
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        public INoticeRepositoryAsync NoticeRepositoryAsyncReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected string content = "";

        protected Notice model = new Notice();

        protected override async Task OnInitializedAsync()
        {
            model = await NoticeRepositoryAsyncReference.GetByIdAsync(Id);
            Logger.LogInformation($"※※※ model 불러오기 , {JsonSerializer.Serialize(model)}");
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
        }
    }
}
