using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineClub.DataAccess;
using WineClub.DataAccess.Model;

namespace WineClub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WineEventsController : ControllerBase
    {
        private readonly WineDbContext _context;

        public WineEventsController(WineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all wine events and include multiple many-to-many relationships
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WineEvent>>> GetWineEvents()
        {
            return await _context.WineEvents.Include(x => x.Wines).ThenInclude(w => w.Ratings).Include(x => x.Attendees).ToListAsync();
        }

        /// <summary>
        /// Get next wine event. Made to avoid memory issues in app.
        /// </summary>
        /// <returns></returns>
        [HttpGet("NextEvent")]
        public async Task<ActionResult<IEnumerable<WineEvent>>> GetNextWineEvent()
        {
            return await _context.WineEvents.Include(x => x.Wines)
                .ThenInclude(w => w.Ratings)
                .Include(x => x.Attendees)
                .Where(x => x.DateAndTime > System.DateTimeOffset.Now)
                .OrderBy(x => x.DateAndTime)
                .Take(1)
                .ToListAsync();
        }

        /// <summary>
        /// Get wine event by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WineEvent>> GetWineEvent(int id)
        {
            WineEvent wineEvent = await _context.WineEvents.FindAsync(id);

            if (wineEvent == null)
            {
                return NotFound();
            }

            return wineEvent;
        }

        /// <summary>
        /// Put request for wine event.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wineEvent"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWineEvent(int id, WineEvent wineEvent)
        {
            if (id != wineEvent.WineEventId)
            {
                return BadRequest();
            }

            _context.Entry(wineEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WineEventExists(id))
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


        /// <summary>
        /// Post request for wine event
        /// </summary>
        /// <param name="wineEvent"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<WineEvent>> PostWineEvent(WineEvent wineEvent)
        {
            _context.WineEvents.Add(wineEvent);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWineEvent", new { id = wineEvent.WineEventId }, wineEvent);
        }

        /// <summary>
        /// Add wine to wine event - many to many relationship.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/Wine")]
        public async Task<ActionResult<WineEvent>> AddWineEventWine(int id, Wine request)
        {
            WineEvent wineEvent = await _context.WineEvents
                .Where(w => w.WineEventId == id)
                .Include(w => w.Wines)
                .FirstOrDefaultAsync();
            if (wineEvent == null)
                return NotFound();

            Wine wine = await _context.Wines.FindAsync(request.WineId);
            if (wine == null)
                return NotFound();

            wineEvent.Wines.Add(wine);
            await _context.SaveChangesAsync();

            return wineEvent;
        }

        /// <summary>
        /// Delete wine from wine event - many to many relationship.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wineId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/Wine/{wineId}")]
        public async Task<IActionResult> DeleteWineEventWine(int id, int wineId)
        {
            WineEvent wineEvent = await _context.WineEvents
                .Where(w => w.WineEventId == id)
                .Include(w => w.Wines)
                .FirstOrDefaultAsync();
            if (wineEvent == null)
                return NotFound();

            Wine wine = await _context.Wines.FindAsync(wineId);
            if (wine == null)
                return NotFound();

            wineEvent.Wines.Remove(wine);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Adds user to an event.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/User")]
        public async Task<ActionResult<WineEvent>> AddWineEventUser(int id, User request)
        {
            WineEvent wineEvent = await _context.WineEvents
                .Where(w => w.WineEventId == id)
                .Include(w => w.Wines)
                .Include(w => w.Attendees)
                .FirstOrDefaultAsync();
            if (wineEvent == null)
                return NotFound();

            User user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound();

            wineEvent.Attendees.Add(user);
            await _context.SaveChangesAsync();

            return wineEvent;
        }

        /// <summary>
        /// Delete user from an event.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/User/{userId}")]
        public async Task<IActionResult> DeleteWineEventUser(int id, int userId)
        {
            WineEvent wineEvent = await _context.WineEvents
                .Where(w => w.WineEventId == id)
                .Include(w => w.Wines)
                .Include(w => w.Attendees)
                .FirstOrDefaultAsync();
            if (wineEvent == null)
                return NotFound();

            User user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            wineEvent.Attendees.Remove(user);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete wine event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWineEvent(int id)
        {
            WineEvent wineEvent = await _context.WineEvents.FindAsync(id);
            if (wineEvent == null)
            {
                return NotFound();
            }

            _context.WineEvents.Remove(wineEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WineEventExists(int id)
        {
            return _context.WineEvents.Any(e => e.WineEventId == id);
        }
    }
}
