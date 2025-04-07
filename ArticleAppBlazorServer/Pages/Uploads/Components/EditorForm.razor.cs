using ArticleApp.Models;
using ArticleAppBlazorServer.Services;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using UploadApp.Shared;

namespace ArticleAppBlazorServer.Pages.Uploads.Components
{
    public partial class EditorForm
    {
        #region Injectors
        /// <summary>
        /// 리포지토리 클래스에 대한 참조 
        /// </summary>
        [Inject]
        public IUploadRepository UploadRepositoryReference { get; set; }

        [Inject]
        public IFileUploadService FileUploadService { get; set; }

        [Inject]
        public IFileStorageManager FileStorageManager { get; set; }
        #endregion

        #region Fields
        private string parentId = "0";

        protected int[] parentIds = { 1, 2, 3 };

        //rivate bool isShow = false;
        public bool IsShow { get; set; } = false;
        #endregion



        public void Show()
        {
            IsShow = true;
        }
        public void Hide()
        {
            IsShow = false;
        }

        #region Parameters
        /// <summary>
        /// 폼의 제목 영역
        /// </summary>
        [Parameter]
        public RenderFragment EditorFormTitle { get; set; }

        /// <summary>
        /// 넘어온 모델 개체 
        /// </summary>
        [Parameter]
        public Upload Model { get; set; }

        /// <summary>
        /// 부모 컴포넌트에게 생성(Create)이 완료되었다고 보고하는 목적으로 부모 컴포넌트에게 알림
        /// 학습 목적으로 Action 대리자 사용
        /// </summary>
        [Parameter]
        public Action CreateCallback { get; set; }

        /// <summary>
        /// 부모 컴포넌트에게 수정(Edit)이 완료되었다고 보고하는 목적으로 부모 컴포넌트에게 알림
        /// 학습 목적으로 EventCallback 구조체 사용
        /// </summary>
        [Parameter]
        public EventCallback<bool> EditCallback { get; set; }
        #endregion

        #region Event Handlers
        protected override void OnParametersSet()
        {
            // ParentId가 넘어온 값이 있으면... 즉, 0이 아니면 ParentId 드롭다운 리스트 기본값 선택
            parentId = Model.ParentId.ToString();
            if (parentId == "0")
            {
                parentId = "";
            }
        }

        /// <summary>
        /// Submit 버튼 클릭 이벤트 처리
        /// </summary>
        protected async void CreateOrEditClick()
        {
            #region 파일 업로드 관련 추가 코드 영역
            if (selectedFiles != null && selectedFiles.Length > 0)
            {
                // 파일 업로드
                var file = selectedFiles.FirstOrDefault();
                var fileName = "";
                int fileSize = 0;
                if (file != null)
                {
                    //file.Name = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}{file.Name}";
                    fileName = file.Name;
                    fileSize = Convert.ToInt32(file.Size);

                    //[A] byte[] 형태
                    var ms = new MemoryStream();
                    await file.Data.CopyToAsync(ms);
                    await FileStorageManager.UploadAsync(ms.ToArray(), file.Name, "", true);
                    //[B] Stream 형태
                    //string folderPath = Path.Combine(WebHostEnvironment.WebRootPath, "files");
                    //
                    //await FileStorageManager.UploadAsync(file.Data, file.Name, "", true);
                    //await FileUploadService.UploadAsync(file);

                    Model.FileName = fileName;
                    Model.FileSize = fileSize;
                }
            }
            #endregion
     


            if (!int.TryParse(parentId, out int newParentId))
            {
                newParentId = 0;
            }
            Model.ParentId = newParentId;

            if (Model.Id == 0)
            {
                // Create
                await UploadRepositoryReference.AddAsync(Model);
                CreateCallback?.Invoke();
            }
            else
            {
                // Edit
                await UploadRepositoryReference.EditAsync(Model);
                await EditCallback.InvokeAsync(true);
            }
            //IsShow = false; // this.Hide()
        }

        private IFileListEntry[] selectedFiles;
        protected void HandleSelection(IFileListEntry[] files)
        {
            this.selectedFiles = files;
        }

        

        #endregion
    }
}
