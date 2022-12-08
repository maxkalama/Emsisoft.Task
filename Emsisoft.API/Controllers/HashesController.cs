using Emsisoft.HashesService;
using Emsisoft.Models;
using Emsisoft.RabbitMQ.Client;
using Microsoft.AspNetCore.Mvc;

namespace Emsisoft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashesController : ControllerBase
    {
        const int hashesCount = 40000;
        const int batchSize = 100;

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            var rnd = new Random();
            IHashesService service = new Sha1HashesService();
            var hashes = Enumerable.Range(0, hashesCount).Select(i => service.GetRandomHash()).ToList();


            IEnumerable<byte[]> hashesBatch = null;
            RabbitMqClient.Send(hashesBatch);
        }
    }
}
