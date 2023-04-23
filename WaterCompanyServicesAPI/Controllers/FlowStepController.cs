using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterCompanyServicesAPI.Models;

namespace WaterCompanyServicesAPI.Controllers
{

    [Route("FlowStep")]
    public class FlowStepController : Controller
    {
        private readonly ILogger<FlowStepController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public FlowStepController(ILogger<FlowStepController> logger, WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlowStep>>> GetFlowSteps()
        {
            return await _context.FlowSteps.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlowStep>> GetFlowStep(int id)
        {
            var FlowStep = await _context.FlowSteps.FindAsync(id);

            if (FlowStep == null)
            {
                return NotFound();
            }

            return FlowStep;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlowStep(int id, [FromBody] FlowStep FlowStep)
        {
            if (id != FlowStep.Id)
            {
                return BadRequest();
            }

            _context.Entry(FlowStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowStepExists(id))
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
        public async Task<ActionResult<FlowStep>> PostFlowStep([FromBody] FlowStep FlowStep)
        {
            _context.FlowSteps.Add(FlowStep);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlowStep", new { id = FlowStep.Id }, FlowStep);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlowStep(int id)
        {
            var FlowStep = await _context.FlowSteps.FindAsync(id);
            if (FlowStep == null)
            {
                return NotFound();
            }

            _context.FlowSteps.Remove(FlowStep);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlowStepExists(int id)
        {
            return _context.FlowSteps.Any(e => e.Id == id);
        }
    }
}
