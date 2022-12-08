using Emsisoft.RabbitMQ.Client;
using Microsoft.AspNetCore.Mvc;

namespace Emsisoft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            RabbitMqClient.Send();
        }
    }
}
