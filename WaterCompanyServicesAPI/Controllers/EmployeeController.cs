using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLibrary;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("Employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public EmployeeController(ILogger<EmployeeController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var Employee = await _context.Employees.Include(e=>e.Department).Include(e=>e.User).Where(e=>e.Id==id).FirstOrDefaultAsync();

            if (Employee == null)
            {
                return NotFound();
            }

            return Employee;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody] Employee Employee)
        {
            if (id != Employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(Employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] Employee Employee)
        {
            var dep = _context.Departments.Find(Employee.Department.Id);
            Employee.Department = dep;
            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = Employee.Id }, Employee);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var Employee = await _context.Employees.FindAsync(id);
            if (Employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(Employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        [Route("/employee/getbyuser/{id}")]
        public async Task<ActionResult<Employee>> Login(int id)
        {
            Employee employee = await _context.Employees.Include(c => c.User).Include(c=>c.Department).Where(c => c.User.Id == id).FirstOrDefaultAsync();
            return employee;
        }


    }
}
