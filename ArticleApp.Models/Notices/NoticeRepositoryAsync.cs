using Dul.Domain.Common;

namespace ArticleApp.Models
{
    /// <summary>
    /// [6] Repository Class: ADO.NET or Dapper or Entity Framework Core
    /// </summary>
    public class NoticeRepositoryAsync : INoticeRepositoryAsync
    {
        public Task<Notice> AddAsync(Notice model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(Notice model)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notice>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<Notice>> GetAllAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<Notice>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<Notice> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
