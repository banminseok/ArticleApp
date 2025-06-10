using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.ViewComponents
{
    public class MainSummaryListViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // ViewComponent에서 사용할 데이터를 준비합니다.
            var summaryData = new List<string>
            {
                "Summary Item 1",
                "Summary Item 2",
                "Summary Item 3"
            };
            // ViewComponent의 뷰를 반환합니다.
            return View(summaryData);
        }
    }
}
