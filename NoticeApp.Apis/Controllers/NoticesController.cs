using ArticleApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace NoticeApp.Apis.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Route("api/Notices")]
    [ApiController]
    public class NoticesController : ControllerBase
    {
        private readonly INoticeRepositoryAsync _noticeRepository;
        private readonly ILogger _logger;

        public NoticesController(INoticeRepositoryAsync noticeRepository, ILoggerFactory loggerFactory)
        {
            _noticeRepository = noticeRepository;
            _logger = loggerFactory.CreateLogger(nameof(NoticesController));
        }

        // 입력
        // POST api/Notices
        [HttpPost]        
        public async Task<IActionResult> AddAsync([FromBody] Notice model)
        {
            // model.Id = 0
            var tmpModel = new Notice();
            tmpModel.Name = model.Name;
            tmpModel.Title = model.Title;
            tmpModel.Content = model.Content;
            tmpModel.ParentId = model.ParentId;
            tmpModel.Created = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var newModel = await _noticeRepository.AddAsync(tmpModel);
                if (newModel == null)
                {
                    return BadRequest();
                }
                //return Ok(newModel); // 200 OK
                var uri = Url.Link("GetNoticeById", new { id = newModel.Id });
                return Created(uri, newModel); // 201 Created
            }
            catch (Exception e)
            {
                _logger.LogError($"Error ({nameof(AddAsync)}):{e.Message}");
                return BadRequest();
            }
        }

        // 출력
        // GET api/Notices
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var models = await _noticeRepository.GetAllAsync();
                if(!models.Any())
                {
                    return new NoContentResult(); //참고용 코드
                }
                return Ok(models);

            }
            catch (Exception e)
            {
                _logger.LogError($"Error ({nameof(GetAll)}):{e.Message}");
                return BadRequest();
            }
        }

        // 상세
        // GET api/Notices/1
        [HttpGet("{id}", Name ="GetNoticeById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _noticeRepository.GetByIdAsync(id);
                if (model == null)
                {
                    return new NoContentResult(); //참고용 코드
                }
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error ({nameof(GetById)}):{e.Message}");
                return BadRequest();
            }
        }

        // 수정
        // PUT api/Notices/1
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id, [FromBody] Notice model)
        {
            model.Id = id;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var status = await _noticeRepository.EditAsync(model);
                if (!status)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error ({nameof(EditAsync)}):{e.Message}");
                return BadRequest();
            }
        }

        // 삭제
        // DELETE api/Notices/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var status = await _noticeRepository.DeleteAsync(id);
                if (!status)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error ({nameof(DeleteAsync)}):{e.Message}");
                return BadRequest();
            }
        }

        // 페이징
        // GET api/Notices/Page/0/10
        [HttpGet("Page/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetAll(int pageIndex, int pageSize)
        {
            try
            {
                var resultsSet = await _noticeRepository.GetAllAsync(pageIndex, pageSize);

                // 응답 헤더에 총 레코드 수를 담아서 출력 
                Response.Headers.Add("X-TotalRecordCount", resultsSet.TotalRecords.ToString());
                Response.Headers.Add("Access-Control-Expose-Headers", "X-TotalRecordCount");

                return Ok(resultsSet.Records);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error ({nameof(GetAll)}):{e.Message}");
                return BadRequest();
            }
        }

    }
}
