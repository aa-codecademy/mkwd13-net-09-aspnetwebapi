using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OurFirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet] //https://localhost:44319/api/values
        public IEnumerable<string> GetStrings() 
        {
            return new[] { "value1", "value2" };
        }
    }
}
