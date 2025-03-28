using ArticleApp.Models;
using BmsPager;
using Dul.Domain.Common;
using Dul.Web;
using Microsoft.AspNetCore.Components;


namespace ArticleAppBlazorServer.Pages.Articles
{
    public partial class Index
    {
        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        private IArticleRepository ArticleRepository { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private List<Article> articles;

        // 페이저 객체 생성
        private BmsPagerBase pager = new BmsPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 3,
            PagerButtonCount = 5
        };

        /// <summary>
        /// 페이지 로드  
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("※※※ [1]Index 시작");

            //[1] 전체 데이터 출력
            //articles = await ArticleRepository.GetArticlesAsync();

            //[2] 페이징 처리
            try
            {
                PagingResult<Article> pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
                pager.RecordCount = pagingData.TotalRecords; // 전체 레코드 수
                Logger.LogInformation($"※※※ [2] 전체 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
                articles = pagingData.Records.ToList(); // 현재 페이지의 레코드

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "index 오류");
            }

        }

        //pager버튼 클릭 콜백 함수

        private async void PageIndexChangedIndex(int pageIndex)
        {
            Logger.LogInformation($"※※※ [3] PageIndexChanged 호출 pageIndex={pageIndex}");
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            PagingResult<Article> pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = pagingData.TotalRecords; // 전체 레코드 수
            articles = pagingData.Records.ToList(); // 현재 페이지의 레코드

            StateHasChanged(); //현재 페이지를 다시 그림
        }

        /// <summary>
        ///  제목의 td 클릭시 상세 페이지로 이동
        /// </summary>
        /// <param name="id"></param>
        private void btnTitle_Click(int id)
        {
            Logger.LogInformation($"※※※ [4] btnTitle_Click 호출 id={id}");
            NavigationManager.NavigateTo($"/Articles/Details/{id}");
        }
    }
}
