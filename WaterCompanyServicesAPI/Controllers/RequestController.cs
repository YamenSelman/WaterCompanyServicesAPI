using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("Request")]
    public class RequestController : Controller
    {
        private readonly ILogger<RequestController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public RequestController(ILogger<RequestController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var Request = await _context.Requests.FindAsync(id);

            if (Request == null)
            {
                return NotFound();
            }

            return Request;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, [FromBody] Request Request)
        {
            if (id != Request.Id)
            {
                return BadRequest();
            }

            _context.Entry(Request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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
        public async Task<ActionResult<Request>> PostRequest([FromBody] Request Request)
        {
            _context.Requests.Add(Request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = Request.Id }, Request);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var Request = await _context.Requests.FindAsync(id);
            if (Request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(Request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }


    }
}
