using ArticleApp.Models;
using ArticleAppBlazorServer.Managers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using UploadApp.Shared;

namespace ArticleAppBlazorServer.Pages.Replys
{
    public partial class Delete
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private ILogger<Create> Logger { get; set; }


        [Inject]
        public IReplyRepository ReplyRepositoryReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IFileStorageManager UploadAppFileStorageManager { get; set; }

        protected Reply model = new Reply();

        protected string content = "";

        protected override async Task OnInitializedAsync()
        {
            model = await ReplyRepositoryReference.GetByIdAsync(Id);
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
        }

        protected async void DeleteClick()
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Id}번 글을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                // 파일 삭제
                if(!string.IsNullOrEmpty(model.FileName))
                {
                    // 첨부 파일 삭제
                    await UploadAppFileStorageManager.DeleteAsync(model.FileName,"");
                }

                // DB 삭제
                await ReplyRepositoryReference.DeleteAsync(Id); // 삭제
                NavigationManagerReference.NavigateTo("/Replys"); // 리스트 페이지로 이동
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }
    }
}
