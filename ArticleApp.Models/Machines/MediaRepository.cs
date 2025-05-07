using Dul.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// [5] Repository Class
    /// </summary>
    public class MediaRepository : IMediaRepository
    {
        private readonly ArticleAppDbContext _context;

        public MediaRepository(ArticleAppDbContext context)
        {
            this._context = context;
        }

        // 입력
        public async Task<Media> AddMediaAsync(Media machine)
        {
            //_context.Add(machine);
            _context.Medias.Add(machine);
            await _context.SaveChangesAsync();
            return machine;
        }

        // 출력
        public async Task<List<Media>> GetMediasAsync()
        {
            return await _context.Medias.OrderByDescending(m => m.Id).ToListAsync();
        }

        // 상세보기
        public async Task<Media> GetMediaByIdAsync(int id)
        {
            //return await _context.Medias.Where(m => m.Id == id).SingleOrDefaultAsync();
            return await _context.Medias.FindAsync(id);
        }

        // 수정
        public async Task<Media> EditMediaAsync(Media machine)
        {
            _context.Entry(machine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return machine;
        }

        // 삭제
        public async Task DeleteMediaAsync(int id)
        {
            //var machine = await _context.Medias.Where(m => m.Id == id).SingleOrDefaultAsync();
            var machine = await _context.Medias.FindAsync(id);
            if (machine != null)
            {
                _context.Medias.Remove(machine);
                await _context.SaveChangesAsync();
            }
        }

        //[!] 페이징: Index + Paging
        public async Task<PagingResult<Media>> GetMediasPageAsync(int pageIndex, int pageSize)
        {
            var totalRecords = await _context.Medias.CountAsync();
            var machines = await _context.Medias
                .OrderByDescending(m => m.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResult<Media>(machines, totalRecords);
        }
    }
}
