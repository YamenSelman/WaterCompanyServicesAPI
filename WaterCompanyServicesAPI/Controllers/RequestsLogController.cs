using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterCompanyServicesAPI.Models;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("RequestsLog")]
    public class RequestsLogController : Controller
    {
        private readonly ILogger<RequestsLogController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public RequestsLogController(ILogger<RequestsLogController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestsLog>>> GetRequests()
        {
            return await _context.RequestsLog.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestsLog>> GetRequest(int id)
        {
            var Request = await _context.RequestsLog.FindAsync(id);

            if (Request == null)
            {
                return NotFound();
            }

            return Request;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, [FromBody] RequestsLog RequestsLog)
        {
            if (id != RequestsLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(RequestsLog).State = EntityState.Modified;

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
        public async Task<ActionResult<RequestsLog>> PostRequest([FromBody] RequestsLog RequestsLog)
        {
            _context.RequestsLog.Add(RequestsLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = RequestsLog.Id }, RequestsLog);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var RequestsLog = await _context.RequestsLog.FindAsync(id);
            if (RequestsLog == null)
            {
                return NotFound();
            }

            _context.RequestsLog.Remove(RequestsLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.RequestsLog.Any(e => e.Id == id);
        }


    }
}
