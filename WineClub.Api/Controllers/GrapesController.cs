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
    public class GrapesController : ControllerBase
    {
        private readonly WineDbContext _context;

        public GrapesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/Grapes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grape>>> GetGrapes()
        {
            return await _context.Grapes.ToListAsync();
        }

        // GET: api/Grapes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grape>> GetGrape(int id)
        {
            Grape grape = await _context.Grapes.FindAsync(id);

            if (grape == null)
            {
                return NotFound();
            }

            return grape;
        }

        // PUT: api/Grapes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrape(int id, Grape grape)
        {
            if (id != grape.GrapeId)
            {
                return BadRequest();
            }

            _context.Entry(grape).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrapeExists(id))
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

        // POST: api/Grapes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Grape>> PostGrape(Grape grape)
        {
            _context.Grapes.Add(grape);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrape", new { id = grape.GrapeId }, grape);
        }

        // DELETE: api/Grapes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrape(int id)
        {
            Grape grape = await _context.Grapes.FindAsync(id);
            if (grape == null)
            {
                return NotFound();
            }

            _context.Grapes.Remove(grape);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GrapeExists(int id)
        {
            return _context.Grapes.Any(e => e.GrapeId == id);
        }
    }
}
