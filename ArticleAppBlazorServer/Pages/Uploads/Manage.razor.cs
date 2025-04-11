using ArticleApp.Models;
using ArticleAppBlazorServer.Managers;
using BlazorUtils;
using BmsPager;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Threading.Tasks;
using UploadApp.Shared;


namespace ArticleAppBlazorServer.Pages.Uploads
{
    public partial class Manage
    {
        [Inject]
        public ILogger<Index> LoggerReference { get; set; }

        [Inject]
        public IUploadRepository UploadRepositoryReference{ get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IFileStorageManager FileStorageManagerReference { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public int ParentId { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery]
        public string? ParentKey { get; set; } = "";

        public ArticleAppBlazorServer.Pages.Uploads.Components.EditorForm EditFormReference { get; set; }
        public ArticleAppBlazorServer.Pages.Uploads.Components.DeleteDialog DeleteDialogReference { get; set; }

        protected List<Upload> models;
        protected Upload model = new Upload();

        //입력폼 제목
        public string EditorFormTitle { get; set; } = "Create";

        /// <summary>
        /// 공지사항으로 올리기 폼을 띄울건지 여부 
        /// </summary>
        public bool IsInlineDialogShow { get; set; } = false;

        // 페이저 객체 생성
        private BmsPagerBase pager = new BmsPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 10,
            PagerButtonCount = 5
        };


        protected override async Task OnInitializedAsync()
        {
            await DisplayData();
        }

        /// <summary>
        /// NoticeApp - UploadApp - ReplyApp을 거쳐 여기 코드가 더 정리됩니다.
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            
            try
            {
                if(ParentKey!="" && !string.IsNullOrEmpty(ParentKey))
                {
                    var resultsSet = await UploadRepositoryReference.GetAllByParentKeyAsync(pager.PageIndex, pager.PageSize, ParentKey);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                    LoggerReference.LogInformation($"※※※ ParentKey!=\"\"");

                }
                else if (ParentId != 0)
                {
                    var resultsSet = await UploadRepositoryReference.GetAllByParentIdAsync(pager.PageIndex, pager.PageSize, ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                    LoggerReference.LogInformation($"※※※ ParentId != 0");

                }
                else
                {
                    var resultsSet = await UploadRepositoryReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                    LoggerReference.LogInformation($"※※※ else");

                }
                LoggerReference.LogInformation($"※※※ [2] ParentId: {ParentId} , ParentKey: {ParentKey} , 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
                StateHasChanged(); //현재 페이지를 다시 그림
                /*
                if (ParentId == 0)
                {
                    await Task.Delay(500);
                    var resultsSet = await UploadRepositoryReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else
                {
                    var resultsSet = await UploadRepositoryReference.GetAllByParentIdAsync(pager.PageIndex, pager.PageSize, ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }*/
            }
            catch (Exception e)
            {
                LoggerReference.LogInformation($"※※※Error ({nameof(DisplayData)}):{e.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task SearchData()
        {
            try
            {
                if (ParentKey != "" && !string.IsNullOrEmpty(ParentKey))
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else if (ParentId != 0)
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllByParentIdAsync(pager.PageIndex, pager.PageSize, this.searchQuery, this.ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                /*    if (ParentId == 0)
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else
                {
                    var resultsSet = await UploadRepositoryReference.SearchAllByParentIdAsync(pager.PageIndex, pager.PageSize, this.searchQuery, this.ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }*/
                StateHasChanged(); //현재 페이지를 다시 그림
                LoggerReference.LogInformation($"※※※ [2] ParentId: {ParentId} , 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
            }
            catch (Exception e)
            {
                LoggerReference.LogInformation($"※※※Error ({nameof(SearchData)}):{e.Message}");
            }
        }

        protected void NameClick(int id)
        {
            LoggerReference.LogInformation($"NameClick: {id}");
            NavigationManagerReference.NavigateTo($"/Uploads/Details/{id}");
        }


        /// <summary>
        /// 페이지 버튼 클릭 이벤트 BmsPagerComponent에서 호출해 주는 이벤트
        /// </summary>
        /// <param name="pageIndex"></param>
        protected async void PageIndexChangedA(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;
            if (this.searchQuery == "")
            {
                await DisplayData();
            }
            else
            {
                await SearchData();
            }


        }

        protected void ShowEditorForm()
        {
            EditorFormTitle = "CREATE";
            this.model = new Upload();
            this.model.ParentKey = ParentKey; // 
            EditFormReference.Show();
        }
        protected void EditBy(Upload model)
        {
            EditorFormTitle = "EDIT";
            this.model = new Upload();
            this.model = model;
            this.model.ParentKey = ParentKey; // 
            EditFormReference.Show();
        }
        protected void DeleteBy(Upload model)
        {
            LoggerReference.LogInformation($"DeleteBy: {model.Id}");
            //삭제 처리 다이얼 로그
            this.model = model;
            DeleteDialogReference.Show();

        }

        protected void ToggleBy(Upload modal)
        {
            this.model = modal; 
            IsInlineDialogShow = true;
        }
        protected async void DownloadBy(Upload model)
        {
            if(!string.IsNullOrEmpty(model.FileName))
            {
                byte[] fileBytes = await FileStorageManagerReference.DownloadAsync(model.FileName, "");
                if (fileBytes != null)
                {
                    // DownCount
                    // model.DownloadCount++;
                    //await UploadRepositoryReference.EditAsync(model);

                    await FileUtil.SaveAs(JSRuntime, model.FileName, fileBytes);
                }
                else
                {
                    LoggerReference.LogInformation($"DownloadBy: {model.Id} - FileBytes is null");
                }
            }
            else
            {
                LoggerReference.LogInformation($"DownloadBy: {model.Id} - FilePath is null");
            }
        }

        protected void ToggleClose()
        {
            IsInlineDialogShow = false;
            this.model = new Upload();
        }
        protected async void ToggleClick()
        {
            this.model.IsPinned = !this.model.IsPinned;
            await UploadRepositoryReference.EditAsync(this.model);
            IsInlineDialogShow = false;
            this.model = new Upload();
            await DisplayData();
        }

        /// <summary>
        /// EditorForm에서 CreateCallback 이벤트가 발생하면 호출되는 메서드
        /// </summary>
        protected async void CreatOrEdit()
        {
            EditFormReference.Hide();
            await DisplayData();
            StateHasChanged();
        }
        /// <summary>
        /// DeleteDialog에서 DeleteCallback 이벤트가 발생하면 호출되는 메서드
        /// </summary>
        protected async void DeleteClick()
        {
            if(!string.IsNullOrEmpty(this.model?.FileName))
            {
                // 첨부 파일 삭제 
                await FileStorageManagerReference.DeleteAsync(this.model.FileName, "");
            }
            await UploadRepositoryReference.DeleteAsync(this.model.Id);
            DeleteDialogReference.Hide();
            this.model = new Upload();
            await DisplayData();
        }

        private string searchQuery = "";

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged();
        }

        /// <summary>
        /// 엑셀 export 버튼 클릭 시 호출되는 메서드
        /// </summary>
        protected async void DownloadExcel()
        {
            using (var package= new ExcelPackage())
            {
                // 새 Excel 워크시트를 "Uploads"라는 이름으로 추가
                var worksheet = package.Workbook.Worksheets.Add("Uploads");

                // 모델 데이터를 "B2" 셀부터 시작하여 Excel 워크시트에 로드
                // 컬렉션에서 Created, Name, Title, DownCount, FileName 속성을 선택하여 테이블 형태로 로드
                var tableBody = worksheet.Cells["B2:B2"].LoadFromCollection(
                    (from m in models select new { m.Created, m.Name, m.Title, m.DownCount, m.FileName })
                    , true); // true는 헤더를 포함한다는 의미

                // 테이블의 특정 열(업로드 관련 데이터)에 대해 오프셋을 설정
                // Offset(1, 1, models.Count, 1): 테이블의 첫 번째 데이터 셀부터 시작하여 열 범위를 지정
                var uploadCol = tableBody.Offset(1, 1, models.Count, 1);

                // 조건부 서식을 추가하여 업로드 열에 색상 스케일을 적용
                var rule = uploadCol.ConditionalFormatting.AddThreeColorScale();
                rule.LowValue.Color = Color.SkyBlue; // 낮은 값에 대해 하늘색 적용
                rule.MiddleValue.Color = Color.White; // 중간 값에 대해 흰색 적용
                rule.HighValue.Color = Color.Red; // 높은 값에 대해 빨간색 적용

                // 테이블 헤더 범위를 지정 ("B2:F2" 범위)
                var header = worksheet.Cells["B2:F2"];

                // 기본 열 너비를 25로 설정
                worksheet.DefaultColWidth = 25;

                // 날짜 형식을 지정 (3행 2열부터 데이터 끝까지 적용)
                // "yyyy MMM d DDD" 형식: 연도, 월, 일, 요일을 표시
                worksheet.Cells[3, 2, models.Count + 2, 2].Style.Numberformat.Format = "yyyy MMM d DDD";

                // 테이블 본문 스타일 설정
                tableBody.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // 텍스트를 왼쪽 정렬
                tableBody.Style.Fill.PatternType = ExcelFillStyle.Solid; // 단색 채우기 패턴 설정
                tableBody.Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke); // 배경색을 연한 회색으로 설정
                tableBody.Style.Border.BorderAround(ExcelBorderStyle.Medium); // 테두리를 중간 두께로 설정

                // 헤더 스타일 설정
                header.Style.Font.Bold = true; // 헤더 텍스트를 굵게 설정
                header.Style.Font.Color.SetColor(Color.White); // 헤더 텍스트 색상을 흰색으로 설정
                header.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue); // 헤더 배경색을 짙은 파란색으로 설정

                // Excel 파일을 생성하여 클라이언트에 다운로드
                // 파일 이름은 현재 날짜와 시간을 기반으로 생성
                FileUtil.SaveAs(JSRuntime, $"{DateTime.Now.ToString("yyyyMMddhhmmss")}_Uploads.xlsx", package.GetAsByteArray());
            }
        }
    }
}
