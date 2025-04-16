using ArticleApp.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace ArticleAppBlazorServer.Controllers
{
    public class ListOfCategoryController : Controller
    {
        private readonly ICategoryRepository _repository;

        public ListOfCategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }
        // GET: ListOfCategory 
        public IActionResult Index()
        {
            //var categoryRepository = new CategoryRepositoryInMemory();
            //var categories = categoryRepository.GetCategories();
            var categories = _repository.GetCategories();
            return View(categories);
        }
    }
}
