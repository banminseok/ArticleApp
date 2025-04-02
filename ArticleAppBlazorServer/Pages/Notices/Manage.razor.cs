using ArticleApp.Models;
using BmsPager;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;


namespace ArticleAppBlazorServer.Pages.Notices
{
    public partial class Manage
    {
        [Inject]
        public ILogger<Index> LoggerReference { get; set; }

        [Inject]
        public INoticeRepositoryAsync NoticeRepositoryAsyncReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        public ArticleAppBlazorServer.Pages.Notices.Components.EditorForm EditFormReference { get; set; }

        protected List<Notice> models;
        protected Notice model = new Notice();

        //입력폼 제목
        public string EditorFormTitle { get; set; } = "Create";

        // 페이저 객체 생성
        private BmsPagerBase pager = new BmsPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 3,
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
            await Task.Delay(1000);
            try
            {
                var resultsSet = await NoticeRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                pager.RecordCount = resultsSet.TotalRecords;
                LoggerReference.LogInformation($"※※※ [2] 전체 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
                models = resultsSet.Records.ToList();
                StateHasChanged(); //현재 페이지를 다시 그림
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

        }

        protected void NameClick(int id)
        {
            LoggerReference.LogInformation($"NameClick: {id}");
            NavigationManagerReference.NavigateTo($"/Notices/Details/{id}");
        }


        /// <summary>
        /// 페이지 버튼 클릭 이벤트 BmsPagerComponent에서 호출해 주는 이벤트
        /// </summary>
        /// <param name="pageIndex"></param>
        protected async void PageIndexChangedA(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;
            await DisplayData();
        }

        protected void ShowEditorForm()
        {
            EditorFormTitle = "CREATE";
            model = new Notice();
            EditFormReference.Show();
        }
        protected void EditBy(Notice model)
        {
            EditorFormTitle = "EDIT";
            this.model = model;
            EditFormReference.Show();
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
    }
}
