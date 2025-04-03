using ArticleApp.Models;
using Microsoft.AspNetCore.Components;

namespace ArticleAppBlazorServer.Pages.Uploads
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private ILogger<Create> Logger { get; set; }


        [Inject]
        public IUploadRepository UploadRepositoryReference{ get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected Upload model = new Upload();

        public string ParentId { get; set; }

        protected int[] parentIds = { 1, 2, 3 };

        protected string content = "";

        protected override async Task OnInitializedAsync()
        {
            model = await UploadRepositoryReference.GetByIdAsync(Id);
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
            ParentId = model.ParentId.ToString();
        }

        protected async void FormSubmit()
        {
            int.TryParse(ParentId, out int parentId);
            model.ParentId = parentId;
            await UploadRepositoryReference.EditAsync(model);
            NavigationManagerReference.NavigateTo("/Uploads");
        }
    }
}
