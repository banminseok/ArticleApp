
using Dul.Domain.Common;

namespace ArticleApp.Models.Categories
{
    /// <summary>
    /// Repository Interface
    /// </summary>
    public interface ICategoryRepository
    {
        Task<Category> AddAsync(Category model); // 입력
        Task<List<Category>> GetAllAsync(); // 출력
        Task<Category> GetByIdAsync(int id); // 상세
        Task<bool> EditAsync(Category model); // 수정
        Task<bool> DeleteAsync(int id); // 삭제
        Task<PagingResult<Category>> GetAllAsync(int pageIndex, int pageSize); // 페이징
    }
}