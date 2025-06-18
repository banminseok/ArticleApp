using DotNetNote.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ReplysController : ControllerBase
    {
        private readonly IReplyRepository _repository;
        private readonly ILogger _logger;

        public ReplysController(IReplyRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = loggerFactory.CreateLogger(nameof(ReplysController));
        }

        #region 출력
        /// <summary>
        /// Get all replies 출력, 전체 레코드를 반환합니다.
        /// </summary>
        /// <returns>List of replies</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var relies = await _repository.GetAllAsync();
                if (relies == null || !relies.Any())
                {
                    _logger.LogError("No replies found.");
                    return new NoContentResult();  //204 No Content 참고용 코드
                }
                return Ok(relies); // 200 OK 응답과 함께 replies 반환
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
        #endregion


        #region 상세
        // 상세
        // GET api/replys/{id}, api/replys/123
        /// <summary>
        [HttpGet("{id:int}", Name = "GetReplyById")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var reply = await _repository.GetByIdAsync(id);
                if (reply == null)
                {
                    _logger.LogError($"Reply with ID {id} not found.");
                    //return NotFound(); // 404 Not Found 응답
                    return new NoContentResult();  //204 No Content 참고용 코드
                }
                return Ok(reply); // 200 OK 응답과 함께 reply 반환
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); // 400 Bad Request 응답
            }
        }
        #endregion


        //페이징
        // Get api/Replys/Page/{pageIndex:int}/Size/{pageSize:int}, api/Replys/Page/1/10
        [HttpGet("Page/{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageNumber=1, [FromRoute] int pageSize=10)
        {
            try
            {
                // 페이지 번호는 1,2,3 사용, 리포지토리에서는 0,1,2, 사용
                int pageIndex = (pageNumber>0) ? pageNumber - 1 : 0;
                var resultSet = await _repository.GetAllAsync(pageIndex, pageSize);
                if (resultSet.Records == null)
                {
                    _logger.LogError($"No replies found for page {pageIndex} with size {pageSize}.");
                    return NotFound($"아무런 데이터가 없습니다.");  
                }
                // 응답헤더에 총레코드 수를 담아서 출려
                Response.Headers.Add("X-TotalRecordCount", resultSet.TotalRecords.ToString());
                Response.Headers.Add("Access-Control-Expose-Headers", "X-TotalRecordCount");

                return Ok(resultSet.Records); // 200 OK 응답과 함께 result 반환
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); // 400 Bad Request 응답
            }
        }
    }
}
