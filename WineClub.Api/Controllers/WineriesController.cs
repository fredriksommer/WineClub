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
    public class WineriesController : ControllerBase
    {
        private readonly WineDbContext _context;

        public WineriesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/Wineries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Winery>>> GetWineries()
        {
            return await _context.Wineries.ToListAsync();
        }

        // GET: api/Wineries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Winery>> GetWinery(int id)
        {
            Winery winery = await _context.Wineries.FindAsync(id);

            if (winery == null)
            {
                return NotFound();
            }

            return winery;
        }

        // PUT: api/Wineries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWinery(int id, Winery winery)
        {
            if (id != winery.WineryId)
            {
                return BadRequest();
            }

            _context.Entry(winery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WineryExists(id))
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

        // POST: api/Wineries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Winery>> PostWinery(Winery winery)
        {
            _context.Wineries.Add(winery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWinery", new { id = winery.WineryId }, winery);
        }

        // DELETE: api/Wineries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWinery(int id)
        {
            Winery winery = await _context.Wineries.FindAsync(id);
            if (winery == null)
            {
                return NotFound();
            }

            _context.Wineries.Remove(winery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WineryExists(int id)
        {
            return _context.Wineries.Any(e => e.WineryId == id);
        }
    }
}
