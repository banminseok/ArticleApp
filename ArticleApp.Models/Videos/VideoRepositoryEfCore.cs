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
    /// [4][3][2] 리포지토리 클래스(비동기 방식): Full ORM인 EF Core를 사용하여 CRUD 구현
    /// </summary>
    public class VideoRepositoryEfCoreAsync : IVideoRepository
    {
        private readonly ArticleAppDbContext _context;

        public VideoRepositoryEfCoreAsync(ArticleAppDbContext context) => _context = context;

        // 입력: Add
        public async Task<Video> AddVideoAsync(Video model)
        {
            _context.Videos.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        // 출력: GetAll 
        public async Task<List<Video>> GetVideosAsync() => await _context.Videos.ToListAsync();

        // 상세보기: GetById
        public async Task<Video> GetVideoByIdAsync(int id) => await _context.Videos.Where(v => v.Id == id).SingleOrDefaultAsync();

        // 수정: Update, Edit
        public async Task<Video> UpdateVideoAsync(Video model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return model;
        }

        // 삭제: Delete, Remove
        public async Task RemoveVideoAsync(int id)
        {
            var video = await _context.Videos.Where(v => v.Id == id).SingleOrDefaultAsync();
            if (video != null)
            {
                _context.Videos.Remove(video);
                await _context.SaveChangesAsync();
            }
        }

        // 출력(페이징): GetAll
        public async Task<List<Video>> GetAllPageAsync(int pageIndex, int pageSize = 5)
        {
            return await _context.Videos
                .OrderBy(m=>m.Id)
                .Skip(pageIndex*pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<PagingResult<Video>> GetVideosByPageAsync(int pageIndex, int pageSize)
        {
            var totalRecordCount = await _context.Videos.CountAsync();
            var videos = await _context.Videos
                .OrderBy(m => m.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagingResult<Video>(videos, totalRecordCount);
        }
    }
}
