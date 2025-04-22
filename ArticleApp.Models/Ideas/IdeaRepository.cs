using Microsoft.EntityFrameworkCore;

namespace ArticleApp.Models
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly IDbContextFactory<IdeaAppDbContext> _contextFactory;
        private readonly IdeaAppDbContext _context;

        public IdeaRepository(IDbContextFactory<IdeaAppDbContext> contextFactory, IdeaAppDbContext context)
        {
            this._contextFactory = contextFactory;
            this._context = context;
        }

        public async Task<List<Idea>> GetIdeasAsync()
        {
            //using (var context = _contextFactory.CreateDbContext())
            //{
            //    return await context.Ideas.ToListAsync();
            //}
            return await _context.Ideas.ToListAsync();
        }

        public async Task<Idea> AddIdeaAsync(Idea idea)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Ideas.Add(idea);
                await context.SaveChangesAsync();
                return idea;
            }
        }
    }
}