using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WaterCompanyServicesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly WaterCompanyDBContext _context;

        public InvoiceController(ILogger<InvoiceController> logger, WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var Invoice = await _context.Invoices.FindAsync(id);

            if (Invoice == null)
            {
                return NotFound();
            }

            return Invoice;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, [FromBody] Invoice Invoice)
        {
            if (id != Invoice.Id)
            {
                return BadRequest();
            }

            _context.Entry(Invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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
        public async Task<ActionResult<Invoice>> PostInvoice([FromBody] Invoice Invoice)
        {
            _context.Invoices.Add(Invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = Invoice.Id }, Invoice);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var Invoice = await _context.Invoices.FindAsync(id);
            if (Invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(Invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
