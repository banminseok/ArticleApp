using Dul.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArticleApp.Models
{
    /// <summary>
    /// [6] Repository Class: ADO.NET or Dapper or Entity Framework Core
    /// </summary>
    public class NoticeRepositoryAsync : INoticeRepositoryAsync
    {
        private readonly ArticleAppDbContext _context;
        private readonly ILogger _logger;

        public NoticeRepositoryAsync(ArticleAppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            this._logger = loggerFactory.CreateLogger(nameof(NoticeRepositoryAsync));
        }
        //[6][1] 입력
        public async Task<Notice> AddAsync(Notice model)
        {
            try
            {
                _context.Notices.Add(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                _logger.LogError($"Error ({nameof(AddAsync)}):{e.Message}");
            }
            return model;
        }

        //[6][2] 출력
        public async Task<List<Notice>> GetAllAsync()
        {
            try
            {
                return await _context.Notices
                        //.Include(m => m.NoticesComments)
                        .OrderByDescending(m => m.Id)
                        .ToListAsync();
            }
            catch (Exception e)
            {

                _logger.LogError($"Error ({nameof(GetAllAsync)}):{e.Message}");
            }
            return null;

        }

        //[6][3] 상세
        public async Task<Notice> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Notices
                    //.Include(m => m.NoticesComments)
                    .SingleOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error({nameof(GetByIdAsync)}):{e.Message}");
            }
            return null; // Add this line to ensure a return value in case of an exception
        }

        //[6][4] 수정
        public async Task<bool> EditAsync(Notice model)
        {
            try
            {
                //이 메서드는 model 객체를 DbContext에 연결합니다. 이 작업은 엔터티가 현재 컨텍스트에서 추적되도록 합니다.Attach 메서드는 엔터티의 상태를 Unchanged로 설정합니다. 즉, 엔터티가 데이터베이스에서 로드되었지만 변경되지 않았음을 나타냅니다.
                _context.Notices.Attach(model);
                //이 줄은 model 객체의 상태를 Modified로 설정합니다. 이는 Entity Framework Core에 해당 엔터티가 변경되었음을 알리는 역할을 합니다. 이렇게 하면 SaveChangesAsync 메서드가 호출될 때, 해당 엔터티의 변경 사항이 데이터베이스에 반영됩니다.
                _context.Entry(model).State = EntityState.Modified;
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error({nameof(EditAsync)}):{e.Message}");
                
            }
            return false;
        }

        //[6][5] 삭제
        public async Task<bool> DeleteAsync(int id)
        {
            //var model = await _context.Notices.SingleOrDefaultAsync(m => m.Id == id);
            try
            {
                var model = await _context.Notices.FindAsync(id);
                if (model != null)
                {
                    _context.Notices.Remove(model);
                    return (await _context.SaveChangesAsync() > 0 ? true : false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error({nameof(DeleteAsync)}):{e.Message}");

            }
            return false;
        }


        //[6][6] 페이징
        public Task<PagingResult<Notice>> GetAllAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        // 부모
        public Task<PagingResult<Notice>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<Notice>> SearchAllAsync(int pageIndex, int pageSize, string searchQuery)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<Notice>> SearchAllByParentIdAsync(int pageIndex, int pageSize, string searchQuery, int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<int, int>> GetStatus(int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAllByParentId(int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<SortedList<int, double>> GetMonthlyCreateCountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
