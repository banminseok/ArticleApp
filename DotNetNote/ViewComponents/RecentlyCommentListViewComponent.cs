using DotNetNote.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.ViewComponents
{
    /// <summary>
    /// 최근 댓글 리스트
    /// Views>Shared 폴더에 Components 폴더를 만들고, 그 안에 RecentlyCommentList 폴더 생성 , Default.cshtml 파일 생성
    /// </summary>
    public class RecentlyCommentListViewComponent : ViewComponent
    {
        // 댓글 리포지토리 개체
        private INoteCommentRepository _repository;

        public RecentlyCommentListViewComponent(
            INoteCommentRepository repository)
        {
            _repository = repository;
        }

        // 최근 댓글 리스트 전달
        //public IViewComponentResult Invoke() => View(_repository.GetRecentComments());
        public IViewComponentResult Invoke()
        {
            // 최근 댓글 리스트를 가져와서 뷰에 전달
            var recentComments = _repository.GetRecentComments();
            return View(recentComments);
        }
    }
}
