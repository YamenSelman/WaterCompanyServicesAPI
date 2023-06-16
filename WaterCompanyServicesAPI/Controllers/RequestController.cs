using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLibrary;

namespace WaterCompanyServicesAPI.Controllers
{
    public class SbyteDocument
    {
        public Request request { get; set; }
        public sbyte[] document { get; set; }
    }    
    
    public class RequestVM
    {
        public Request request { get; set; }
        public DateTime? FinishDate { get; set; }
        public Department? RejectedBy { get; set; }
        public string? RejectNotes { get; set; }
    }

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
        [Route("getRequestsByBarcode/{barcode}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> getRequestsByBarcode(string barcode)
        {
            return await _context.Requests.Where(r => r.Subscription.ConsumerBarCode.Equals(barcode)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var Request = await _context.Requests.Include(r => r.Consumer).Include(r => r.Subscription).Include(r => r.CurrentDepartment).Include(r=>r.Details).Include(r=>r.Result).Where(r => r.Id == id).FirstOrDefaultAsync();

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
                if(Request.Consumer != null)
                {
                    Request.Consumer = _context.Consumers.Find(Request.Consumer.Id);
                }
                if (Request.Subscription != null) 
                {
                    Request.Subscription = _context.Subscriptions.Find(Request.Subscription.Id);
                }
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
        
        [HttpPost]
        [Route("/request/postSbyte")]
        public async Task<ActionResult<int>> PostRequestSbyte([FromBody] SbyteDocument content)
        {
            try
            {
                content.request.Details.Document = Array.ConvertAll(content.document, (a) => (byte)a);
                if (content.request.Consumer != null)
                {
                    content.request.Consumer = _context.Consumers.Find(content.request.Consumer.Id);
                }
                if (content.request.Subscription != null) 
                {
                    content.request.Subscription = _context.Subscriptions.Find(content.request.Subscription.Id);
                }
                content.request.CurrentDepartment = _context.Departments.Find(content.request.CurrentDepartment.Id);
                _context.Requests.Add(content.request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return   content.request.Id;
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
            List<string> status = new List<string> { "completed", "rejected" };
            return await _context.Requests.Include(r=>r.Consumer).Include(r=>r.Subscription).Where(r => r.CurrentDepartment != null && r.CurrentDepartment.Id == did && !status.Contains(r.RequestStatus)).ToListAsync();
        }
        
        [Route("/request/getByConsumer/{cid}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestVM>>> GetByConsumer(int cid)
        {
            List<RequestVM> result = new List<RequestVM>();
            var requests = await _context.Requests.Include(r=>r.Subscription).Where(r=>r.Consumer.Id == cid).ToListAsync();
            foreach(var item in requests)
            {
                RequestVM r = new RequestVM();
                r.request = item;
                if(item.RequestStatus.ToLower() != "onprogress")
                {
                    var log = await _context.RequestsLogs.Include(l=>l.Department).Where(l => l.Request.Id == item.Id).OrderBy(r=>r.Id).LastAsync();
                    r.FinishDate = log.DateTime;
                    if(item.RequestStatus.ToLower().Equals("rejected"))
                    {
                        r.RejectedBy = log.Department;
                        r.RejectNotes = log.Notes;
                    }
                }
                result.Add(r);
            }
            return result;
        }

        [Route("/request/accept/{rid}/{eid}/{notes?}")]
        [HttpGet]
        public async Task<ActionResult<bool>> AcceptRequest(int rid, int eid, string notes = "")
        {
            Request? req = await _context.Requests.Include(r => r.Consumer).Include(r => r.Subscription).Include(r=>r.Details).Include(r=>r.CurrentDepartment).Where(r => r.Id == rid).FirstOrDefaultAsync();
            Employee? emp = await _context.Employees.Include(e => e.Department).Where(e => e.Id == eid).FirstOrDefaultAsync();
            if (req != null && emp != null && !req.RequestStatus.Equals("completed") && !req.RequestStatus.Equals("rejected"))
            {
                if (req.CurrentDepartment == emp.Department)
                {
                    switch (req.RequestType)
                    {
                        case "attach":
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                try
                                {
                                    req.RequestStatus = "completed";
                                    req.CurrentDepartment = null;
                                    req.Subscription.Consumer = req.Consumer;
                                    _context.SaveChanges();

                                    RequestsLog log = new RequestsLog();
                                    log.DateTime = DateTime.Now;
                                    log.Department = emp.Department;
                                    log.Employee = emp;
                                    log.Decision = true;
                                    log.Request = req;
                                    log.Notes = notes;
                                    _context.RequestsLogs.Add(log);
                                    _context.SaveChanges();

                                    transaction.Commit();
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        case "clearance":
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                try
                                {
                                    if(emp.Department.Id == 2)
                                    {
                                        req.CurrentDepartment = _context.Departments.Where(d => d.Id == 3).FirstOrDefault();
                                    }
                                    else
                                    {
                                        req.CurrentDepartment = null;
                                        req.RequestStatus = "completed";
                                    }
                                    _context.SaveChanges();

                                    RequestsLog log = new RequestsLog();
                                    log.DateTime = DateTime.Now;
                                    log.Department = emp.Department;
                                    log.Employee = emp;
                                    log.Decision = true;
                                    log.Request = req;
                                    log.Notes = notes;
                                    _context.RequestsLogs.Add(log);
                                    _context.SaveChanges();

                                    transaction.Commit();
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        case "new":
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                try
                                {
                                    if (emp.Department.Id == 1)
                                    {
                                        req.CurrentDepartment = _context.Departments.Where(d => d.Id == 2).FirstOrDefault();
                                    }
                                    else if(emp.Department.Id == 2)
                                    {
                                        req.CurrentDepartment = _context.Departments.Where(d => d.Id == 4).FirstOrDefault();
                                    }                
                                    else if(emp.Department.Id == 4)
                                    {
                                        req.CurrentDepartment = _context.Departments.Where(d => d.Id == 3).FirstOrDefault();
                                    }
                                    else
                                    {
                                        Subscription sub = new Subscription();
                                        sub.SubscriptionStatus = "active";
                                        sub.ConsumerSubscriptionNo = await GetNextSubscriptionNo();
                                        sub.ConsumerBarCode = await GetNextBarcode();
                                        sub.SubscriptionAddress = req.Details.NewSubAddress;
                                        sub.SubscriptionUsingType = req.Details.NewSubType;
                                        sub.Consumer = req.Consumer;

                                        _context.Subscriptions.Add(sub);

                                        req.Result = new RequestResult();
                                        req.Result.Document = Helper.GenerateNewSubscriptionDocument(sub, req.Consumer);

                                        req.CurrentDepartment = null;
                                        req.RequestStatus = "completed";
                                    }
                                    _context.SaveChanges();

                                    RequestsLog log = new RequestsLog();
                                    log.DateTime = DateTime.Now;
                                    log.Department = emp.Department;
                                    log.Employee = emp;
                                    log.Decision = true;
                                    log.Request = req;
                                    log.Notes = notes;
                                    _context.RequestsLogs.Add(log);
                                    _context.SaveChanges();

                                    transaction.Commit();
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        case "repair":
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                try
                                {
                                    if (emp.Department.Id == 2)
                                    {
                                        req.CurrentDepartment = _context.Departments.Where(d => d.Id == 3).FirstOrDefault();
                                    }
                                    else if (emp.Department.Id == 3)
                                    {
                                        req.CurrentDepartment = _context.Departments.Where(d => d.Id == 5).FirstOrDefault();
                                    }
                                    else
                                    {
                                        req.Result = new RequestResult();
                                        req.Result.Document = Helper.GenerateRepairResult(notes, req.Consumer);

                                        req.CurrentDepartment = null;
                                        req.RequestStatus = "completed";
                                    }
                                    _context.SaveChanges();

                                    RequestsLog log = new RequestsLog();
                                    log.DateTime = DateTime.Now;
                                    log.Department = emp.Department;
                                    log.Employee = emp;
                                    log.Decision = true;
                                    log.Request = req;
                                    log.Notes = notes;
                                    _context.RequestsLogs.Add(log);
                                    _context.SaveChanges();

                                    transaction.Commit();
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        case "transfer":
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                try
                                {
                                    if (emp.Department.Id == 1)
                                    {
                                        req.CurrentDepartment = _context.Departments.Where(d => d.Id == 2).FirstOrDefault();
                                    }
                                    else if (emp.Department.Id == 2)
                                    {
                                        req.CurrentDepartment = _context.Departments.Where(d => d.Id == 3).FirstOrDefault();
                                    }
                                    else
                                    {
                                        req.RequestStatus = "completed";
                                        req.CurrentDepartment = null;
                                        req.Subscription.Consumer = req.Consumer;

                                        _context.SaveChanges();
                                    }
                                    _context.SaveChanges();

                                    RequestsLog log = new RequestsLog();
                                    log.DateTime = DateTime.Now;
                                    log.Department = emp.Department;
                                    log.Employee = emp;
                                    log.Decision = true;
                                    log.Request = req;
                                    log.Notes = notes;
                                    _context.RequestsLogs.Add(log);
                                    _context.SaveChanges();

                                    transaction.Commit();
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                    }
                }
            }
            return false;
        }

        

        [Route("/request/reject/{rid}/{eid}/{notes?}")]
        [HttpGet]
        public async Task<ActionResult<bool>> RejectRequest(int rid, int eid, string notes = "")
        {
            Request? req = await _context.Requests.Include(r => r.Consumer).Include(r => r.Subscription).Where(r => r.Id == rid).FirstOrDefaultAsync();
            Employee? emp = await _context.Employees.Include(e => e.Department).Where(e => e.Id == eid).FirstOrDefaultAsync();
            if (req != null && emp != null)
            {
                if (req.CurrentDepartment == emp.Department)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            req.RequestStatus = "rejected";
                            req.CurrentDepartment = null;
                            _context.SaveChanges();

                            RequestsLog log = new RequestsLog();
                            log.DateTime = DateTime.Now;
                            log.Department = emp.Department;
                            log.Employee = emp;
                            log.Decision = false;
                            log.Request = req;
                            log.Notes = notes;
                            _context.RequestsLogs.Add(log);
                            _context.SaveChanges();

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                            transaction.Rollback();
                            return false;
                        }
                    }
                }

            }
            return false;
        }

        public async Task<string> GetNextSubscriptionNo()
        {
            List<string> subscriptionNos = await _context.Subscriptions.Select(s => s.ConsumerSubscriptionNo).ToListAsync();
            if (subscriptionNos != null && subscriptionNos.Count > 0)
            {
                int max = Int32.Parse(subscriptionNos.Max());
                return (max + 1).ToString("D6");
            }
            else
            {
                return "000001";
            }
        }

        public async Task<string> GetNextBarcode()
        {
            List<string> barcodes = await _context.Subscriptions.Select(s => s.ConsumerBarCode).ToListAsync();
            if (barcodes != null && barcodes.Count > 0)
            {
                int max = Int32.Parse(barcodes.Max());
                return (max + 1).ToString("D6");
            }
            else
            {
                return "000001";
            }
        }
    }
}
