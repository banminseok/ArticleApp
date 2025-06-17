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
        /// Get all replies 출력
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
                    return new NoContentResult();  //참고용 코드
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

    }
}
