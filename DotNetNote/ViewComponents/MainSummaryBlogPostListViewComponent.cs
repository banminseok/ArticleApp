using DotNetNote.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.ViewComponents
{
    public class MainSummaryBlogPostListViewComponent(INoteRepository repository) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = repository.GetNoteSummaryByCategoryBlog("Blog");
            return View(model);
        }
    }
}
