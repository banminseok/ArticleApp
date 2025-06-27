
namespace ArticleAppBlazorServer.Models
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
    }
}