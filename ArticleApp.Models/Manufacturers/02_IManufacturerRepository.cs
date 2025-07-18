using Dul.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    public interface IManufacturerRepository
    {
        Task<Manufacturer> AddManufacturerAsync(Manufacturer manufacturer);  // 입력
        Task<List<Manufacturer>> GetManufacturersAsync();                    // 출력
        Task<Manufacturer> GetManufacturerAsync(int id);                     // 상세
        Task<Manufacturer> EditManufacturerAsync(Manufacturer manufacturer); // 수정
        Task DeleteManufacturerAsync(int id);                                // 삭제

        Task<List<Manufacturer>> GetAllAsync(int pageIndex, int pageSize);   // 페이징
        Task<PagingResult<Manufacturer>> GetAllByPageAsync(int pageIndex, int pageSize);   // 페이징
    }
}
