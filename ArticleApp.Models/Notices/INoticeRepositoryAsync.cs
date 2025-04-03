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
    public interface INoticeRepositoryAsync: ICrudRepositoryAsync<Notice>
    {
        Task<Tuple<int, int>> GetStatus(int parentId);
        Task<bool> DeleteAllByParentId(int parentId);
        Task<SortedList<int, double>> GetMonthlyCreateCountAsync();
        Task<SortedList<int, double>> GetMonthlyCreateCountGroupByAsync();
    }
}
