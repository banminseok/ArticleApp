using ArticleApp.Models;
using BmsPager;
using Dul.Domain.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace ArticleAppBlazorServer.Pages.Articles
{
    public partial class Manage
    {
        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        private IArticleRepository ArticleRepository { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private string editorFormTitle = ""; //페이지 타이틀

        private bool isShow = false;    // 모달 창 표시 여부
        private Article selectArticle = new Article(); // 선택한 항목 (관리)
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
        /// 새로만들기 창 띄우기
        /// </summary>
        private void btnCreate_Click()
        {
            editorFormTitle = "<h5>새로 만들기</h5>";
            selectArticle = new Article();
            Logger.LogInformation($"※※※ [1] 새로만들기 시작 ,모달창은 부트스트랩(data-toggle='modal' data-target='#articleEditDialog')으로 띄움");
        }
        /// <summary>
        /// editForm에서 저장 또는 수정 후 부모 컴포넌트 새로고침
        /// </summary>
        private async void parentSaveOrUpdate()
        {
            PagingResult<Article> pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = pagingData.TotalRecords; // 전체 레코드 수
            Logger.LogInformation($"※※※ [2] 전체 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
            articles = pagingData.Records.ToList(); // 현재 페이지의 레코드
            StateHasChanged();
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="article"></param>
        private void EditBy(Article article)
        {
            editorFormTitle = "<h5>수정하기</h5>";
            selectArticle = article;
            Logger.LogInformation($"※※※ [1] 수정하기 : {selectArticle.Id}, 모달창은 부트스트랩(data-toggle='modal' data-target='#articleEditDialog')으로 띄움");
        }

        /// <summary>
        /// 삭제  
        /// </summary>
        /// <param name="article"></param>
        private void DeleteBy(Article article)
        {
            selectArticle = article;
            Logger.LogInformation($"※※※ [1] 삭제하기 : {selectArticle.Id}");
        }
        /// <summary>
        /// 게시글을 공지로 설정...
        /// </summary>
        /// <param name="article"></param>
        private void PinnedBy(Article article)
        {
            selectArticle = article;
            Logger.LogInformation($"※※※ [1] 공지글로 올리기 : {selectArticle.Id}");
            isShow = true;
        }

        /// <summary>
        /// 공지글로 설정 /해제
        /// </summary>
        private async Task btnTogglePinned_Click()
        {
            selectArticle.IsPinned = !selectArticle.IsPinned; //공지글로 설정/해제 toggle
            await ArticleRepository.EditArticleAsync(selectArticle);

            PagingResult<Article> pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = pagingData.TotalRecords; // 전체 레코드 수
            articles = pagingData.Records.ToList(); // 현재 페이지의 레코드

            isShow = false; //창닫기
            StateHasChanged(); //현재 페이지를 다시 그림
        }

        /// <summary>
        /// 창닫기
        /// </summary>
        private void btnClose_Click()
        {
            isShow = false; //창닫기
            selectArticle = new Article(); //선택한 항목 초기화
        }


        /// <summary>
        /// 삭제 -> 모달 폼 닫기 -> 선택했던 데이터 초기화 -> 전체 데이터 다시 읽어오기 -> Refresh
        /// </summary>
        private async void btnDelete_Click()
        {
            await ArticleRepository.DeleteArticleAsync(selectArticle.Id); // 삭제
            await JSRuntime.InvokeAsync<object>("closeModal", "articleDeleteDialog"); // _Host.cshtml
            selectArticle = new Article(); // 선택 항목 초기화

            var pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = pagingData.TotalRecords;
            articles = pagingData.Records.ToList();

            StateHasChanged();
        }
        /// <summary>
        /// 페이지 로드
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("※※※ [1]Index 시작");

            //[1] 전체 데이터 출력
            //articles = await ArticleRepository.GetArticlesAsync();

            //[2] 페이징 처리
            PagingResult<Article> pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = pagingData.TotalRecords; // 전체 레코드 수
            Logger.LogInformation($"※※※ [2] 전체 레코드 수 {pager.RecordCount} , {pager.PageNumber}페이지");
            articles = pagingData.Records.ToList(); // 현재 페이지의 레코드
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
    }
}
