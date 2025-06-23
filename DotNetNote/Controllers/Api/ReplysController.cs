using Asp.Versioning;
using DotNetNote.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Controllers
{
    [ApiVersion("1.0")]
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/Replys")]
    //[Route("api/v{v:apiVersion}/[controller]")]
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


        #region 페이징

        //페이징
        // Get api/Replys/Page/{pageIndex:int}/Size/{pageSize:int}, api/Replys/Page/1/10
        [HttpGet("Page/{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 10)
        {
            try
            {
                // 페이지 번호는 1,2,3 사용, 리포지토리에서는 0,1,2, 사용
                int pageIndex = (pageNumber > 0) ? pageNumber - 1 : 0;
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
        #endregion


        #region 입력
        //입력
        // POST api/replys
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Reply model)
        {
            if (model == null)
            {
                _logger.LogError("Reply model is null.");
                return BadRequest("Reply model cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state.");
                return BadRequest(ModelState); // 400 Bad Request 응답
            }

            var temp = new Reply();
            temp.Id = 0; // Id는 자동 생성되므로 클라이언트에서 지정하지 않도록 합니다.
            temp.Name = model.Name;
            temp.Title = model.Title;
            temp.Content = model.Content;
            temp.Created = DateTime.Now;

            try
            {
                var createdReply = await _repository.AddAsync(temp);
                if (createdReply == null)
                {
                    _logger.LogError("Failed to create reply.");
                    return BadRequest("Failed to create reply.");
                }

                // CreatedAtRoute는 생성된 리소스의 URI를 반환합니다.
                return CreatedAtRoute("GetReplyById", new { id = createdReply.Id }, createdReply);
                //return Ok(model); // 200 OK 응답과 함께 생성된 reply 반환
                //var url = Url.Link("GetReplyById", new { id = createdReply.Id });
                //return Created(Url, model); // 201 Created 응답과 함께 생성된 reply 반환
                //return CreatedAtAction(nameof(GetReplyById), new { id = createdReply.Id }, createdReply); // 201 Created 응답과 함께 생성된 reply 반환
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); // 400 Bad Request 응답
            }
        }
        #endregion

        #region 수정
        //수정
        // PUT api/replys/{id:int}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] Reply model)
        {
            if (model == null)
            {
                _logger.LogError("Reply model is null.");
                return BadRequest("Reply model cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state.");
                return BadRequest(ModelState); // 400 Bad Request 응답
            }
            try
            {
                var existingReply = await _repository.GetByIdAsync(id);
                if (existingReply == null)
                {
                    _logger.LogError($"Reply with ID {id} not found.");
                    return NotFound($"Reply with ID {id} not found."); // 404 Not Found 응답
                }
                existingReply.Name = model.Name;
                existingReply.Title = model.Title;
                existingReply.Content = model.Content;
                existingReply.Created = DateTime.Now;
                var status = await _repository.EditAsync(existingReply);
                if (!status)
                {
                    _logger.LogError("Failed to update reply.");
                    return BadRequest("Failed to update reply.");
                }
                //return Ok(); // 200 OK 응답과 함께 업데이트된 reply 반환
                return NoContent(); // 204 No Content 응답 //이미 전송한 정보에 모든 값이 가지고 있기에
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); // 400 Bad Request 응답
            }
        }
        #endregion


        #region 삭제
        //삭제
        // DELETE api/replys/{id:int}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var existingReply = await _repository.GetByIdAsync(id);
                if (existingReply == null)
                {
                    _logger.LogError($"Reply with ID {id} not found.");
                    return NotFound($"Reply with ID {id} not found."); // 404 Not Found 응답
                }
                var status = await _repository.DeleteAsync(id);
                if (!status)
                {
                    _logger.LogError("Failed to delete reply.");
                    return BadRequest("Failed to delete reply.");
                }
                return NoContent(); // 204 No Content 응답
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); // 400 Bad Request 응답
            }
        }
        #endregion
    }

    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/v{v:apiVersion}/Replys")]
    //[Route("api/Replys")]
    //[Route("api/v{v:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ReplysV2_0Controller : ControllerBase
    {
        private readonly IReplyRepository _repository;
        private readonly ILogger _logger;

        public ReplysV2_0Controller(IReplyRepository repository, ILoggerFactory loggerFactory)
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
                return Ok(relies.OrderBy(m=>m.Id)); // 200 OK 응답과 함께 replies 반환
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
        [HttpGet("{id:int}", Name = "GetReplyByIdV2")]
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


        #region 페이징

        //페이징
        // Get api/Replys/Page/{pageIndex:int}/Size/{pageSize:int}, api/Replys/Page/1/10
        [HttpGet("Page/{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 10)
        {
            try
            {
                // 페이지 번호는 1,2,3 사용, 리포지토리에서는 0,1,2, 사용
                int pageIndex = (pageNumber > 0) ? pageNumber - 1 : 0;
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
        #endregion


        #region 입력
        //입력
        // POST api/replys
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Reply model)
        {
            if (model == null)
            {
                _logger.LogError("Reply model is null.");
                return BadRequest("Reply model cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state.");
                return BadRequest(ModelState); // 400 Bad Request 응답
            }

            var temp = new Reply();
            temp.Id = 0; // Id는 자동 생성되므로 클라이언트에서 지정하지 않도록 합니다.
            temp.Name = model.Name;
            temp.Title = model.Title;
            temp.Content = model.Content;
            temp.Created = DateTime.Now;

            try
            {
                var createdReply = await _repository.AddAsync(temp);
                if (createdReply == null)
                {
                    _logger.LogError("Failed to create reply.");
                    return BadRequest("Failed to create reply.");
                }

                // CreatedAtRoute는 생성된 리소스의 URI를 반환합니다.
                return CreatedAtRoute("GetReplyById", new { id = createdReply.Id }, createdReply);
                //return Ok(model); // 200 OK 응답과 함께 생성된 reply 반환
                //var url = Url.Link("GetReplyById", new { id = createdReply.Id });
                //return Created(Url, model); // 201 Created 응답과 함께 생성된 reply 반환
                //return CreatedAtAction(nameof(GetReplyById), new { id = createdReply.Id }, createdReply); // 201 Created 응답과 함께 생성된 reply 반환
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); // 400 Bad Request 응답
            }
        }
        #endregion

        #region 수정
        //수정
        // PUT api/replys/{id:int}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] Reply model)
        {
            if (model == null)
            {
                _logger.LogError("Reply model is null.");
                return BadRequest("Reply model cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state.");
                return BadRequest(ModelState); // 400 Bad Request 응답
            }
            try
            {
                var existingReply = await _repository.GetByIdAsync(id);
                if (existingReply == null)
                {
                    _logger.LogError($"Reply with ID {id} not found.");
                    return NotFound($"Reply with ID {id} not found."); // 404 Not Found 응답
                }
                existingReply.Name = model.Name;
                existingReply.Title = model.Title;
                existingReply.Content = model.Content;
                existingReply.Created = DateTime.Now;
                var status = await _repository.EditAsync(existingReply);
                if (!status)
                {
                    _logger.LogError("Failed to update reply.");
                    return BadRequest("Failed to update reply.");
                }
                //return Ok(); // 200 OK 응답과 함께 업데이트된 reply 반환
                return NoContent(); // 204 No Content 응답 //이미 전송한 정보에 모든 값이 가지고 있기에
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); // 400 Bad Request 응답
            }
        }
        #endregion


        #region 삭제
        //삭제
        // DELETE api/replys/{id:int}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var existingReply = await _repository.GetByIdAsync(id);
                if (existingReply == null)
                {
                    _logger.LogError($"Reply with ID {id} not found.");
                    return NotFound($"Reply with ID {id} not found."); // 404 Not Found 응답
                }
                var status = await _repository.DeleteAsync(id);
                if (!status)
                {
                    _logger.LogError("Failed to delete reply.");
                    return BadRequest("Failed to delete reply.");
                }
                return NoContent(); // 204 No Content 응답
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(); // 400 Bad Request 응답
            }
        }
        #endregion
    }
}
