using ArticleApp.Models;
using ArticleAppBlazorServer.Managers;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using UploadApp.Shared;

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
        [Inject]
        public IFileStorageManager UploadAppFileStorageManager { get; set; }

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
            #region 파일 업로드 관련 추가 코드 영역
            if (selectedFiles != null && selectedFiles.Length > 0)
            {
                // 파일 업로드
                var file = selectedFiles.FirstOrDefault();
                var fileName = "";
                int fileSize = 0;
                if (file != null)
                {
                    fileName = file.Name;
                    fileSize = Convert.ToInt32(file.Size);

                    // 첨부 파일 삭제 
                    await UploadAppFileStorageManager.DeleteAsync(model.FileName, "");

                    // 다시 업로드
                    fileName = await UploadAppFileStorageManager.UploadAsync(file.Data, file.Name, "", true);

                    model.FileName = fileName;
                    model.FileSize = fileSize;
                }
            }
            #endregion
            await UploadRepositoryReference.EditAsync(model);
            NavigationManagerReference.NavigateTo("/Uploads");
        }

        private IFileListEntry[] selectedFiles;
        protected async void HandleSelection(IFileListEntry[] files)
        {
            this.selectedFiles = files;
        }
    }
}
