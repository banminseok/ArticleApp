using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// [3] Repository Interface
    /// </summary>
    public interface ICommonValueRepository : ICommonValueCrudRepository<CommonValue>
    {
        Task<Tuple<int, int>> GetStatus(int parentId);
        Task<bool> DeleteAllByParentId(int parentId);
        Task<SortedList<int, double>> GetMonthlyCreateCountAsync();

        Task<string> FindValueByTest(string text);
    }
}
