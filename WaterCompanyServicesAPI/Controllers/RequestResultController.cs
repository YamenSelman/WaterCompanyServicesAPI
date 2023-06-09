using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLibrary;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("RequestResult")]
    public class RequestResultController : Controller
    {
        private readonly ILogger<RequestResultController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public RequestResultController(ILogger<RequestResultController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestResult>>> GetRequests()
        {
            return await _context.RequestsResults.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestResult>> GetRequest(int id)
        {
            var Request = await _context.RequestsResults.FindAsync(id);

            if (Request == null)
            {
                return NotFound();
            }

            return Request;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, [FromBody] RequestResult RequestResult)
        {
            if (id != RequestResult.Id)
            {
                return BadRequest();
            }

            _context.Entry(RequestResult).State = EntityState.Modified;

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
        public async Task<ActionResult<RequestResult>> PostRequest([FromBody] RequestResult RequestResult)
        {
            _context.RequestsResults.Add(RequestResult);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = RequestResult.Id }, RequestResult);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var RequestResult = await _context.RequestsDetails.FindAsync(id);
            if (RequestResult == null)
            {
                return NotFound();
            }

            _context.RequestsDetails.Remove(RequestResult);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.RequestsDetails.Any(e => e.Id == id);
        }


    }
}
