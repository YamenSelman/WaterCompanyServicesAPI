﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WaterCompanyServicesAPI.Controllers
{
    [Route("Subscription")]
    public class SubscriptionController : Controller
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly WaterCompanyDBContext _context;

        public IActionResult Index()
        {
            return View();
        }
        public SubscriptionController(ILogger<SubscriptionController> logger,WaterCompanyDBContext _context)
        {
            this._logger = logger;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
        {
            return await _context.Subscriptions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var Subscription = await _context.Subscriptions.FindAsync(id);

            if (Subscription == null)
            {
                return NotFound();
            }

            return Subscription;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(int id, [FromBody] Subscription Subscription)
        {
            if (id != Subscription.Id)
            {
                return BadRequest();
            }

            _context.Entry(Subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
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
        public async Task<ActionResult<Subscription>> PostSubscription([FromBody] Subscription Subscription)
        {
            _context.Subscriptions.Add(Subscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscription", new { id = Subscription.Id }, Subscription);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var Subscription = await _context.Subscriptions.FindAsync(id);
            if (Subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(Subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.Id == id);
        }


    }
}
