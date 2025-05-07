using Dul.Domain.Common;
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
    public interface IMediaRepository
    {
        Task<Media> AddMediaAsync(Media media);     // 입력
        Task<List<Media>> GetMediasAsync();             // 출력: GetAll(), GetAllMachines()
        Task<Media> GetMediaByIdAsync(int id);          // 상세: GetById(), FindById()
        Task<Media> EditMediaAsync(Media media);    // 수정: Update()
        Task DeleteMediaAsync(int id);                    // 삭제: Remove()

        Task<PagingResult<Media>> GetMediasPageAsync(int pageIndex, int pageSize);                   // 출력: 페이징이 처리된
    }
}
