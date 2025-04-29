using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// [4][1][2] 리포지토리 클래스(비동기 방식): 순수 ADO.NET을 사용하여 CRUD 구현
    /// </summary>
    public class VideoRepositoryAdoNet : IVideoRepository
    {
        public Task<Video> AddVideoAsync(Video model)
        {
            throw new NotImplementedException();
        }

        public Task<Video> GetVideoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Video>> GetVideosAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveVideoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Video> UpdateVideoAsync(Video model)
        {
            throw new NotImplementedException();
        }
    }

}
