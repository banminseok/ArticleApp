using DotNetNote.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers.DotNetNote
{
    /// <summary>
    /// 최근 댓글 리스트를 반환하는 컨트롤러
    /// •	C# 12 이상에서는 Primary Constructor 문법
    /// </summary>
    /// <param name="repository"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class NoteCommentServiceController(INoteCommentRepository repository) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<NoteComment> Get()
        {
            // 최근 댓글 리스트 반환
            return repository.GetRecentComments();      // 캐싱 적용 전
            //return repository.GetRecentCommentsNoCache();   // 캐싱 적용 후
        }
    }
}
