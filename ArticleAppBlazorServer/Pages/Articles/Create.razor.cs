using ArticleApp.Models;
using Microsoft.AspNetCore.Components;


namespace ArticleAppBlazorServer.Pages.Articles
{
    public partial class Create
    {
        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        private IArticleRepository ArticleRepository { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private string? errorMessage;

        [SupplyParameterFromForm]
        public Article Model { get; set; } = new Article();

        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("※※※[1]Create 폼 시작");
        }

        protected async Task btnSubmit_Click()
        {
            Logger.LogInformation("※※※[2]Create Submit");
            try
            {
                await ArticleRepository.AddArticleAsync(Model);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Logger.LogError(ex, "Create 오류");
            }
            //리스트 페이지로 이동
            NavigationManager.NavigateTo("/Articles");
        }
    }
}
