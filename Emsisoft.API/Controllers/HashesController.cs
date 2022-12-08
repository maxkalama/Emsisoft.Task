using Emsisoft.Data;
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
        private readonly IHashesService _service;
        private readonly IDbHashService _dbService;

        public HashesController(IHashesService service,
            IDbHashService dbService)
        {
            _service = service;
            _dbService = dbService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var counts = _dbService.GetCounts();
            return Ok(counts.Select(c=> new {Date = c.Key.ToShortDateString(), Count = c.Value.ToString()}));
        }

        /// <summary>
        /// Generates and adds 40000 hashes to the RabbitMQ queue.
        /// </summary>
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            var hashes = Enumerable.Range(0, hashesCount).Select(i => _service.GetRandomHash()).ToList();
            var batches = hashes.Chunk(batchSize).ToList(); 

            batches.ForEach(batch =>
            {
                var hashesBytes = batch.Select(hash => _service.ToBinary(hash));
                RabbitMqClient.SendBatch(hashesBytes);
            });

           return Ok();
        }
    }
}
