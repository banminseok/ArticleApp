using ArticleApp.Models;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using OfficeOpenXml;
using UploadApp.Shared;

namespace ArticleAppBlazorServer.Pages.Replys
{
    public partial class Import
    {
        [Inject]
        public IReplyRepository ReplyRepositoryReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IFileStorageManager UploadAppFileStorageManager { get; set; }


        protected Reply model = new Reply();
        public string ParentId { get; set; }
        protected int[] parentIds = { 1, 2, 3 };

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

                    fileName = await UploadAppFileStorageManager.UploadAsync(file.Data, file.Name, "", true);

                    model.FileName = fileName;
                    model.FileSize = fileSize;
                }
            }
            #endregion

            foreach (var m in Models)
            {
                m.FileName = model.FileName;
                m.FileSize = model.FileSize;
                await ReplyRepositoryReference.AddAsync(m);
            }
            NavigationManagerReference.NavigateTo("/Uploads");
        }

        public List<Reply> Models { get; set; } = new List<Reply>();
        private IFileListEntry[] selectedFiles;
        protected async void HandleSelection(IFileListEntry[] files)
        {
            this.selectedFiles = files;

            // 엑셀 데이터 읽어오기 
            if (selectedFiles != null && selectedFiles.Length > 0)
            {
                var file = selectedFiles.FirstOrDefault(); // [1] 업로드된 파일의 첫번ㅉ째 파일을 가져옴

                // [2] 선택된 파일을 메모리 스트림으로 읽어온다
                using (var stream = new MemoryStream())  // 메모리 스트리밍 객체 생성
                {
                    // [3] 넘겨온 파일객체의 실제 data를 메모리 스트림에 복사한다
                    await file.Data.CopyToAsync(stream);
                    // [4] 스트림 객체에서 워크시트를 읽어서 inMemory 엑셀 파일을 생성한다
                    using (var package = new ExcelPackage(stream)) // EPPlus 패키지를 만든다.
                    {
                        // [5] 엑셀 파일의 첫번째 워크시트를 가져온다
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        // [6] 엑셀 파일의 행(row) 개수를 가져온다
                        var rowCount = worksheet.Dimension.Rows;
                        // [7] 엑셀 파일의 각 행을 읽어서 Upload 모델에 매핑한다  
                        for (int row = 2; row <= rowCount; row++)
                        {
                            // [8] 엑셀 파일의 각 행을 읽어서 Upload 모델에 매핑한다
                            // Modles라는 속성에 Upload 모델을 추가한다
                            Models.Add(new Reply
                            {
                                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                DownCount = int.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()),
                            }); 
                        }
                    }
                }
                StateHasChanged();
            }
        }
    }
}
