using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[!] 애트리뷰트(어트리뷰트) 라우팅
    public class ApiHelloWorldController : Controller
    {
        // GET: api/<ApiHelloWorldController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "안녕하세요.", "반갑습니다." };
        }

        // api/ApiHelloWorld/id
        //[!] 라우트 매개변수
        //[HttpGet("{id}")]
        //[!] 모델 바인딩 + 인라인 제약 조건(:)
        [HttpGet("{id:int}")]
        public string Get(int id)
        {
            return $"넘어온 값: {id}";
        }

        // POST api/<ApiHelloWorldController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApiHelloWorldController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiHelloWorldController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
