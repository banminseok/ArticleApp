using DotNetNote.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace DotNetNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //•	C# 12 이상에서 지원하는 "Primary Constructor" 문법입니다.
    //public class NoteServiceController(INoteRepository repository) : Controller
    public class NoteServiceController : ControllerBase
    {
        private readonly INoteRepository _repository;

        public NoteServiceController(INoteRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IEnumerable<Note> Get()
        {
            // 최근 글 리스트 반환
            //return _repository.GetRecentPosts();      // 캐싱 적용 전
            return _repository.GetRecentPostsCache();   // 캐싱 적용 후
        }
    }
}
