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
        const int hashesCount = 500;//40000;
        const int batchSize = 100;
        private IHashesService _service;

        public HashesController(IHashesService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Generates and adds 40000 hashes to the RabbitMQ queue.
        /// </summary>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var hashes = Enumerable.Range(0, hashesCount).Select(i => _service.GetRandomHash()).ToList();
            var batches = hashes.Chunk(batchSize).ToList(); 

            batches.ForEach(batch =>
            {
                var hashesBytes = batch.Select(hash => _service.ToBinary(hash));
                RabbitMqClient.SendBatch(hashesBytes);
            });
        }
    }
}
