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
        public RequestController(ILogger<RequestController> logger, WaterCompanyDBContext _context)
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
            var Request = await _context.Requests.Include(r => r.Consumer).Include(r => r.Subscription).Include(r => r.CurrentDepartment).Where(r=>r.Id == id).FirstOrDefaultAsync();

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
            try
            {
                Request.Consumer = _context.Consumers.Find(Request.Consumer.Id);
                Request.Subscription = _context.Subscriptions.Find(Request.Subscription.Id);
                Request.CurrentDepartment = _context.Departments.Find(Request.CurrentDepartment.Id);
                _context.Requests.Add(Request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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

        [Route("/request/attach/{cid}/{sid}")]
        [HttpGet]
        public async Task<ActionResult<Boolean>> RequestAttach(int cid, int sid)
        {
            try
            {
                var consumer = _context.Consumers.Find(cid);
                if (consumer == null)
                {
                    return false;
                }
                else
                {
                    var subscription = _context.Subscriptions.Find(sid);
                    if (subscription == null)
                    {
                        return false;
                    }
                    else
                    {
                        Request req = new Request();
                        req.Subscription = subscription;
                        req.Consumer = consumer;
                        req.RequestDate = DateTime.Now;
                        req.RequestType = "attach";
                        req.RequestStatus = "submitted";
                        _context.Requests.Add(req);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("yms: " + ex.Message);
                return false;
            }
        }

        [Route("/request/getpending/{did}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests(int did)
        {
            List<string> status = new List<string>{"completed","rejected"};
            return await _context.Requests.Where(r => r.CurrentDepartment != null && r.CurrentDepartment.Id == did && ! status.Contains(r.RequestStatus)).ToListAsync();
        }


    }
}
