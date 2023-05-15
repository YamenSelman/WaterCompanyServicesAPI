using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("Consumer")]
    public class ConsumerController : Controller
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public ConsumerController(ILogger<ConsumerController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consumer>>> GetConsumers()
        {
            return await _context.Consumers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Consumer>> GetConsumer(int id)
        {
            var Consumer = await _context.Consumers.FindAsync(id);

            if (Consumer == null)
            {
                return NotFound();
            }

            return Consumer;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsumer(int id, [FromBody] Consumer Consumer)
        {
            if (id != Consumer.Id)
            {
                return BadRequest();
            }

            _context.Entry(Consumer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsumerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Consumer>> PostConsumer([FromBody] Consumer Consumer)
        {
            _context.Consumers.Add(Consumer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConsumer", new { id = Consumer.Id }, Consumer);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsumer(int id)
        {
            var Consumer = await _context.Consumers.FindAsync(id);
            if (Consumer == null)
            {
                return NotFound();
            }

            _context.Consumers.Remove(Consumer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsumerExists(int id)
        {
            return _context.Consumers.Any(e => e.Id == id);
        }

        [Route("/consumer/getbyuser/{id}")]
        public async Task<ActionResult<Consumer>> Login(int id)
        {
            Consumer consumer = await _context.Consumers.Include(c=>c.User).Where(c => c.User.Id == id).FirstOrDefaultAsync();
            return consumer;
        }


    }
}
