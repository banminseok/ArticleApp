using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Nodes;
using UploadApp.Shared;

namespace Hawaso.Pages.Customers
{
    public partial class Create
    {
        #region Parameters
        [Parameter]
        public int Id { get; set; } = 0;
        #endregion

        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        public IReplyRepository ReplyRepositoryReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }
        [Inject]
        public IFileStorageManager UploadAppFileStorageManager { get; set; }

        protected Reply model = new Reply();
        public string ParentId { get; set; }
        protected int[] parentIds = { 1, 2, 3 };

        // 부모 글의 답변형 게시판 계층 정보를 임시 보관
        public int ParentRef { get; set; } = 0;  //부모글의 Id를 임시 보관
        public int ParentStep { get; set; } = 0;
        public int ParentRefOrder { get; set; } = 0;

        #region Lifecycle Methods
        /// <summary>
        /// 페이지 초기화 이벤트 처리기
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // 답변 글쓰기 페이지라면, 기존 데이터 읽어오기 
            if (Id!=0)
            {
                // 기존 글의 데이터를 읽어오기
                model = await ReplyRepositoryReference.GetByIdAsync(Id);
                model.Id = 0; // 답변 페이지는 새로운 글로 초기화 
                model.Name = "";
                model.Title = "Re: " + model.Title;
                model.Content = "\r\n====\r\n" + model.Content;

                // Nullable 값 형식이 null일 수 있으므로 null 병합 연산자를 사용하여 기본값을 설정
                ParentRef = model.Ref ?? 0;
                ParentStep = model.Step ?? 0;
                ParentRefOrder = model.RefOrder ?? 0;
            }
        }
        #endregion

        protected async void FormSubmit()
        {
            int.TryParse(ParentId, out int parentId);
            model.ParentId = parentId;

            if (Id != 0)
            {
                // Reply: 답변 글이라면,
                await ReplyRepositoryReference.AddAsync(model, ParentRef, ParentStep, ParentRefOrder);
            }
            else
            {
                // Create: 일반 작성 글이라면,
                await ReplyRepositoryReference.AddAsync(model);
            }
            //await ReplyRepositoryReference.AddAsync(model);
            Logger.LogInformation($"※※※ Reply model 저장 , {JsonSerializer.Serialize(model)}");
            NavigationManagerReference.NavigateTo("/Replys");
        }

    }
}
