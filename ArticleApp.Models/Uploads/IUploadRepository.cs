using Dul.Articles;
using Dul.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// [4] Repository Interface
    /// </summary>
    public interface IUploadRepository : ICrudRepositoryAsync<Upload>
    {
        Task<Tuple<int, int>> GetStatus(int parentId);
        Task<bool> DeleteAllByParentId(int parentId);
        Task<SortedList<int, double>> GetMonthlyCreateCountAsync();
        Task<PagingResult<Upload>> GetAllByParentKeyAsync(int pageIndex, int pageSize, string parentKey); // 부모
        Task<PagingResult<Upload>> SearchAllByParentKeyAsync(int pageIndex, int pageSize, string searchQuery, string parentKey); // 검색+부모
        Task<ArticleSet<Upload, int>> GetArticles<TParentIdentifier>(int pageIndex, int pageSize, string searchField, string searchQuery, string sortOrder, TParentIdentifier parentIdentifier);
        
    }
}
