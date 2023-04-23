using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterCompanyServicesAPI.Models;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("RequestType")]
    public class RequestTypeController : Controller
    {
        private readonly ILogger<RequestTypeController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public RequestTypeController(ILogger<RequestTypeController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestType>>> GetRequests()
        {
            return await _context.RequestTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestType>> GetRequest(int id)
        {
            var Request = await _context.RequestTypes.FindAsync(id);

            if (Request == null)
            {
                return NotFound();
            }

            return Request;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, [FromBody] RequestType RequestType)
        {
            if (id != RequestType.Id)
            {
                return BadRequest();
            }

            _context.Entry(RequestType).State = EntityState.Modified;

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
        public async Task<ActionResult<RequestType>> PostRequest([FromBody] RequestType RequestType)
        {
            _context.RequestTypes.Add(RequestType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = RequestType.Id }, RequestType);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var RequestType = await _context.RequestTypes.FindAsync(id);
            if (RequestType == null)
            {
                return NotFound();
            }

            _context.RequestTypes.Remove(RequestType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.RequestTypes.Any(e => e.Id == id);
        }


    }
}
