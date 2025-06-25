using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using UploadApp.Shared;

namespace Hawaso.Pages.Customers
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private ILogger<Edit> Logger { get; set; }


        [Inject]
        public IReplyRepository ReplyRepositoryReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }
        [Inject]
        public IFileStorageManager UploadAppFileStorageManager { get; set; }

        protected Reply model = new Reply();

        public string ParentId { get; set; }

        protected int[] parentIds = { 1, 2, 3 };

        protected string content = "";

        protected override async Task OnInitializedAsync()
        {
            model = await ReplyRepositoryReference.GetByIdAsync(Id);
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
            ParentId = model.ParentId.ToString();
        }

        protected async void FormSubmit()
        {
            int.TryParse(ParentId, out int parentId);
            model.ParentId = parentId;
            
            await ReplyRepositoryReference.EditAsync(model);
            NavigationManagerReference.NavigateTo("/Replys");
        }

    }
}
