using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public UserController(ILogger<ConsumerController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [Route("/user/exists/{userName}")]
        public bool UserExists(string userName)
        {
            List<User> users = _context.Users.Where(e => e.UserName == userName).ToList();
            foreach(User usr in users)
            {
                if(usr.UserName == userName) return true;
            }
            return false;
        }

        [Route("/user/login")]
        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody] User user)
        {
            List<User> users = await _context.Users.Where(u => u.UserName == user.UserName && u.Password == user.Password).ToListAsync();

            foreach (User usr in users)
            {
                if(usr.UserName == user.UserName && usr.Password == user.Password)
                {
                    return usr;
                }
            }
            return NotFound("user not found");
        }

    }
}
