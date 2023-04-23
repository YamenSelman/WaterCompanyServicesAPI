using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterCompanyServicesAPI.Models;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("RequestDocument")]
    public class RequestDocumentController : Controller
    {
        private readonly ILogger<RequestDocumentController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public RequestDocumentController(ILogger<RequestDocumentController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDocument>>> GetRequests()
        {
            return await _context.RequestDocuments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDocument>> GetRequest(int id)
        {
            var Request = await _context.RequestDocuments.FindAsync(id);

            if (Request == null)
            {
                return NotFound();
            }

            return Request;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, [FromBody] RequestDocument RequestDocument)
        {
            if (id != RequestDocument.Id)
            {
                return BadRequest();
            }

            _context.Entry(RequestDocument).State = EntityState.Modified;

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
        public async Task<ActionResult<RequestDocument>> PostRequest([FromBody] RequestDocument RequestDocument)
        {
            _context.RequestDocuments.Add(RequestDocument);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = RequestDocument.Id }, RequestDocument);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var RequestDocument = await _context.RequestDocuments.FindAsync(id);
            if (RequestDocument == null)
            {
                return NotFound();
            }

            _context.RequestDocuments.Remove(RequestDocument);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.RequestDocuments.Any(e => e.Id == id);
        }


    }
}
