using Emsisoft.Data;
using Emsisoft.HashesService;
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
        public async Task<ActionResult> GetAsync()
        {
            var counts = await _dbService.GetCountsAsync();
            return Ok(counts.Select(c=> new {Date = c.Key.ToShortDateString(), Count = c.Value.ToString()}));
        }

        /// <summary>
        /// Generates and adds 40000 hashes to the RabbitMQ queue.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] string value)
        {
            var hashes = Enumerable.Range(0, hashesCount).Select(i => _service.GetRandomHash()).ToList();
            var batches = hashes.Chunk(batchSize).ToList();

            await Task.Run(() => batches.ForEach(batch => //takes some time so awaitable
                {
                    var hashesBytes = batch.Select(hash => _service.ToBinary(hash));
                    RabbitMqClient.SendBatch(hashesBytes);
                })
            );

            return Ok();
        }
    }
}
