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
            //이 메서드는 model 객체를 DbContext에 연결합니다. 이 작업은 엔터티가 현재 컨텍스트에서 추적되도록 합니다.Attach 메서드는 엔터티의 상태를 Unchanged로 설정합니다. 즉, 엔터티가 데이터베이스에서 로드되었지만 변경되지 않았음을 나타냅니다.
            _context.Notices.Attach(model);
            //이 줄은 model 객체의 상태를 Modified로 설정합니다. 이는 Entity Framework Core에 해당 엔터티가 변경되었음을 알리는 역할을 합니다. 이렇게 하면 SaveChangesAsync 메서드가 호출될 때, 해당 엔터티의 변경 사항이 데이터베이스에 반영됩니다.
            _context.Entry(model).State = EntityState.Modified;
            try
            {
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
        public async Task<PagingResult<Notice>> GetAllAsync(int pageIndex, int pageSize)
        {
            try
            {
                var totalRecords = await _context.Notices.CountAsync();
                var models = await _context.Notices
                    //.Include(m => m.NoticesComments)
                    .OrderByDescending(m => m.Id)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                _logger.LogInformation($"pageIndex:{pageIndex}, pageSize:{pageSize} , totalRecords : {totalRecords}, skip: {(pageIndex * pageSize)} ");

                return new PagingResult<Notice>(models, totalRecords);
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Error({nameof(GetAllAsync)}):{e.Message}");
            }
            return new PagingResult<Notice>(null, 0);
        }

        // 부모
        public async Task<PagingResult<Notice>> GetAllByParentIdAsync(int pageIndex, int pageSize, int parentId)
        {
            try
            {
                var totalRecords = await _context.Notices.Where(m=>m.ParentId == parentId).CountAsync();
                var models = await _context.Notices
                    .Where(m => m.ParentId == parentId)
                    //.Include(m => m.NoticesComments)
                    .OrderByDescending(m => m.Id)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagingResult<Notice>(models, totalRecords);

            }
            catch (Exception e)
            {
                _logger.LogError($"Error({nameof(GetAllByParentIdAsync)}):{e.Message}");
            }
            return new PagingResult<Notice>(null, 0);
        }

        // 검색
        public async Task<PagingResult<Notice>> SearchAllAsync(int pageIndex, int pageSize, string searchQuery)
        {
            try
            {
                var totalRecords = await _context.Notices
                    .Where(m =>  (m.Name.Contains(searchQuery) || m.Title.Contains(searchQuery) || m.Content.Contains(searchQuery)))
                    .CountAsync();
                var models = await _context.Notices
                    //.Include(m => m.NoticesComments)
                    .Where(m => (m.Name.Contains(searchQuery) || m.Title.Contains(searchQuery) || m.Content.Contains(searchQuery)))
                    .OrderByDescending(m => m.Id)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagingResult<Notice>(models, totalRecords);

            }
            catch (Exception e)
            {
                _logger.LogError($"Error({nameof(GetAllAsync)}):{e.Message}");
            }
            return new PagingResult<Notice>(null, 0);
        }

        public async Task<PagingResult<Notice>> SearchAllByParentIdAsync(int pageIndex, int pageSize, string searchQuery, int parentId)
        {
            try
            {
                var totalRecords = await _context.Notices
                    .Where(m => m.ParentId == parentId)
                    .Where(m => ( EF.Functions.Like(m.Name, $"%{searchQuery}%") || m.Title.Contains(searchQuery) || m.Content.Contains(searchQuery)))
                    .CountAsync();
                var models = await _context.Notices
                    //.Include(m => m.NoticesComments)
                    .Where(m => m.ParentId == parentId)
                    .Where(m => (m.Name.Contains(searchQuery) || m.Title.Contains(searchQuery) || m.Content.Contains(searchQuery)))
                    .OrderByDescending(m => m.Id)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagingResult<Notice>(models, totalRecords);

            }
            catch (Exception e)
            {
                _logger.LogError($"Error({nameof(GetAllAsync)}):{e.Message}");
            }
            return new PagingResult<Notice>(null, 0);
        }

        #region [*] 전체공지글 중 고정글갯수...
        //고정글 상태
        public async Task<Tuple<int, int>> GetStatus(int parentId)
        {
            var totalRecords = await _context.Notices.Where(m => m.ParentId == parentId).CountAsync();
            var pinnedRecords = await _context.Notices.Where(m => m.ParentId == parentId && m.IsPinned == true).CountAsync();

            return new Tuple<int, int>(pinnedRecords, totalRecords);
        }
        #endregion

        //부모글의 자식글 모두 삭제
        public async Task<bool> DeleteAllByParentId(int parentId)
        {
            try
            {
                var models = await _context.Notices.Where(m=>m.ParentId==parentId).ToListAsync();
                if (models != null)
                {
                    foreach (var model in models)
                    {
                        _context.Notices.Remove(model);                        
                    }
                    return (await _context.SaveChangesAsync() > 0 ? true : false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error({nameof(DeleteAllByParentId)}):{e.Message}");

            }
            return false;
        }

        public async Task<SortedList<int, double>> GetMonthlyCreateCountAsync()
        {
            SortedList<int, double> createCounts = new SortedList<int, double>();

            // 1월부터 12월까지 0.0으로 초기화  {3, 3.0},
            for (int i = 1; i <= 12; i++)
            {
                createCounts[i] = 0.0;
            }

            for (int i = 0; i < 12; i++)
            {
                // 현재 달부터 12개월 전까지 반복
                var current = DateTime.Now.AddMonths(-i);
                var cnt = _context.Notices.AsEnumerable().Where(
                    m => m.Created != null
                    &&
                    Convert.ToDateTime(m.Created).Month == current.Month
                    &&
                    Convert.ToDateTime(m.Created).Year == current.Year
                ).ToList().Count();
                createCounts[current.Month] = cnt;
            }

            return await Task.FromResult(createCounts);
        }

        public async Task<SortedList<int, double>> GetMonthlyCreateCountGroupByAsync()
        {
            SortedList<int, double> createCounts = new SortedList<int, double>();
            var current = DateTime.Now;
            var twelveMonthsAgo = DateTime.Now.AddMonths(-12);
            var monthlyCounts = await _context.Notices
                .Where(m=> m.Created != null && m.Created >= twelveMonthsAgo && m.Created <= current)
                .GroupBy(m => new { m.Created.Value.Year, m.Created.Value.Month })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Count = group.Count()
                }).ToListAsync();
            // 1월부터 12월까지 0.0으로 초기화  {3, 3.0},
            for (int i = 1; i <= 12; i++)
            {
                createCounts[i] = 0.0;
            }
            // 1월부터 12월까지 0.0으로 초기화  {3, 3.0},
            for (int i = 1; i <= 12; i++)
            {
                createCounts[i] = monthlyCounts.FirstOrDefault(x => x.Month == i).Count;
            }
            return await Task.FromResult(createCounts);
        }
    }
}
