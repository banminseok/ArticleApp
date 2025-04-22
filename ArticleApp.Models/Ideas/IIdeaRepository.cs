
namespace ArticleApp.Models
{
    public interface IIdeaRepository
    {
        Task<Idea> AddIdeaAsync(Idea idea);
        Task<List<Idea>> GetIdeasAsync();
    }
}