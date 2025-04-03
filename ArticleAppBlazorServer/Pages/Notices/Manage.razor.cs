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

        [Parameter]
        public int ParentId { get; set; } = 0;

        public ArticleAppBlazorServer.Pages.Notices.Components.EditorForm EditFormReference { get; set; }
        public ArticleAppBlazorServer.Pages.Notices.Components.DeleteDialog DeleteDialogReference { get; set; }

        protected List<Notice> models;
        protected Notice model = new Notice();

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
            
            try
            {
                if (ParentId == 0)
                {
                    await Task.Delay(500);
                    var resultsSet = await NoticeRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();                    
                }
                else
                {
                    var resultsSet = await NoticeRepositoryAsyncReference.GetAllByParentIdAsync(pager.PageIndex, pager.PageSize, ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                LoggerReference.LogInformation($"※※※ [2] ParentId: {ParentId} , 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
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
                if (ParentId == 0)
                {
                    var resultsSet = await NoticeRepositoryAsyncReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
                else
                {
                    var resultsSet = await NoticeRepositoryAsyncReference.SearchAllByParentIdAsync(pager.PageIndex, pager.PageSize, this.searchQuery, this.ParentId);
                    pager.RecordCount = resultsSet.TotalRecords;
                    models = resultsSet.Records.ToList();
                }
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
            if (this.searchQuery == "")
            {
                await DisplayData();
            }
            else
            {
                await SearchData();
            }


            StateHasChanged(); //현재 페이지를 다시 그림
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
        protected void DeleteBy(Notice model)
        {
            LoggerReference.LogInformation($"DeleteBy: {model.Id}");
            //삭제 처리 다이얼 로그
            this.model = model;
            DeleteDialogReference.Show();

        }

        protected void ToggleBy(Notice modal)
        {
            this.model = modal; 
            IsInlineDialogShow = true;
        }
        protected void ToggleClose()
        {
            IsInlineDialogShow = false;
            this.model = new Notice();
        }
        protected async void ToggleClick()
        {
            this.model.IsPinned = !this.model.IsPinned;
            await NoticeRepositoryAsyncReference.EditAsync(this.model);
            IsInlineDialogShow = false;
            this.model = new Notice();
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
            await NoticeRepositoryAsyncReference.DeleteAsync(this.model.Id);
            DeleteDialogReference.Hide();
            this.model = new Notice();
            await DisplayData();
        }

        private string searchQuery = "";

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged();
        }

    }
}
