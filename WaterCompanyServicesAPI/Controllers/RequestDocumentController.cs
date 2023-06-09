using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLibrary;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("RequestDetails")]
    public class RequestDetailsController : Controller
    {
        private readonly ILogger<RequestDetailsController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public RequestDetailsController(ILogger<RequestDetailsController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDetails>>> GetRequests()
        {
            return await _context.RequestsDetails.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDetails>> GetRequest(int id)
        {
            var Request = await _context.RequestsDetails.FindAsync(id);

            if (Request == null)
            {
                return NotFound();
            }

            return Request;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, [FromBody] RequestDetails RequestDetails)
        {
            if (id != RequestDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(RequestDetails).State = EntityState.Modified;

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
        public async Task<ActionResult<RequestDetails>> PostRequest([FromBody] RequestDetails RequestDetails)
        {
            _context.RequestsDetails.Add(RequestDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = RequestDetails.Id }, RequestDetails);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var RequestDetails = await _context.RequestsDetails.FindAsync(id);
            if (RequestDetails == null)
            {
                return NotFound();
            }

            _context.RequestsDetails.Remove(RequestDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.RequestsDetails.Any(e => e.Id == id);
        }


    }
}
