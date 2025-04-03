using ArticleApp.Models;
using BmsPager;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ArticleAppBlazorServer.Pages.Uploads
{
    public partial class Index
    {
        [Inject]
        public ILogger<Index> LoggerReference { get; set; }

        [Inject]
        public IUploadRepository UploadRepositoryReference{ get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected List<Upload> models;

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
                var resultsSet = await UploadRepositoryReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                pager.RecordCount = resultsSet.TotalRecords;
                LoggerReference.LogInformation($"※※※ [2] 전체 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
                models = resultsSet.Records.ToList();
                
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
                var resultsSet = await UploadRepositoryReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                pager.RecordCount = resultsSet.TotalRecords;
                LoggerReference.LogInformation($"※※※ [2] 전체 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
                models = resultsSet.Records.ToList();
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
            

            StateHasChanged(); //현재 페이지를 다시 그림
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
