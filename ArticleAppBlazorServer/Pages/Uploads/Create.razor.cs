using ArticleApp.Models;
using ArticleAppBlazorServer.Managers;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Nodes;
using UploadApp.Shared;

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
        [Inject]
        public IFileStorageManager UploadAppFileStorageManager { get; set; }

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
            #region 파일 업로드 관련 추가 코드 영역
            if (selectedFiles !=null && selectedFiles.Length>0)
            {
                //파일 업로드
                var file = selectedFiles.FirstOrDefault();
                var fileName = "";
                int fileSize = 0;
                if (file != null)
                {
                    fileName = file.Name;
                    fileSize = Convert.ToInt32(file.Size);
                    fileName = await UploadAppFileStorageManager.UploadAsync(file.Data, fileName, "", true);

                    model.FileName = fileName;
                    model.FileSize = fileSize;
                }
            }
            #endregion
            await UploadRepositoryReference.AddAsync(model);
            Logger.LogInformation($"※※※ model 저장 , {JsonSerializer.Serialize(model)}");
            NavigationManagerReference.NavigateTo("/Uploads");
        }


        private IFileListEntry[] selectedFiles;
        protected void HandleSelection(IFileListEntry[] files)
        {
            this.selectedFiles = files;
        }
    }
}
