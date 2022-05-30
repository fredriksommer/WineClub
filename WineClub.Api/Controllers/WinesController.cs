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
    public class WinesController : ControllerBase
    {
        private readonly WineDbContext _context;

        public WinesController(WineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all wines with parameters to change query to database.
        /// </summary>
        /// <param name="includeRegions"></param>
        /// <param name="includeGrapes"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wine>>> GetWines([FromQuery(Name = "includeRegions")] bool includeRegions, [FromQuery(Name = "includeGrapes")] bool includeGrapes)
        {
            if (includeGrapes && includeRegions)
            {
                return await _context.Wines
                    .Include(wine => wine.Regions)
                    .Include(wine => wine.Grapes)
                    .Include(wine => wine.Winery)
                    .Include(wine => wine.Ratings)
                        .ThenInclude(w => w.RatedBy)
                    .OrderBy(x => x.Name).ToListAsync();
            }
            else if (includeGrapes)
            {
                return await _context.Wines.Include(wine => wine.Grapes).ToListAsync();
            }
            else if (includeRegions)
            {
                return await _context.Wines.Include(wine => wine.Regions).ToListAsync();
            }
            return await _context.Wines.ToListAsync();
        }

        /// <summary>
        /// Get all wines rated by user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("RatedByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<Wine>>> GetWinesRatedByUser(int userId)
        {
            return await _context.Wines
                .Include(wine => wine.Winery)
                .Include(wine => wine.Ratings)
                .Where(r => r.Ratings.Any(u => u.RatedBy.UserId == userId)).ToListAsync();
        }

        /// <summary>
        /// Get top 3 highest rated wines.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Top3")]
        public async Task<ActionResult<IEnumerable<Wine>>> GetTop3Wines()
        {
            return await _context.Wines
                   .Include(wine => wine.Winery)
                   .Include(wine => wine.Ratings)
                   .OrderByDescending(r => r.Ratings.Average(x => x.Score)).Take(3).ToListAsync();
        }

        // GET: api/Wines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wine>> GetWine(int id)
        {
            Wine wine = await _context.Wines.FindAsync(id);

            if (wine == null)
            {
                return NotFound();
            }

            return wine;
        }

        // PUT: api/Wines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWine(int id, Wine wine)
        {
            if (id != wine.WineId)
            {
                return BadRequest();
            }

            _context.Wines.Update(wine);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WineExists(id))
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

        // POST: api/Wines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wine>> PostWine(Wine wine)
        {
            _context.Wines.Add(wine);
            Winery winery = await _context.Wineries.FindAsync(wine.Winery.WineryId);
            if (winery != null)
            {
                _context.Wineries.Update(wine.Winery);
            }

            User user = await _context.Users.FindAsync(wine.AddedBy.UserId);
            if (user != null)
            {
                _context.Users.Update(wine.AddedBy);
            }

            if (wine.Grapes != null)
            {
                foreach (Grape grape in wine.Grapes)
                {
                    _context.Grapes.Update(grape);
                }
            }

            if (wine.Regions != null)
            {
                foreach (Region region in wine.Regions)
                {
                    _context.Regions.Update(region);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWine", new { id = wine.WineId }, wine);
        }

        /// <summary>
        /// Adds many-to-many relationship Wine - Grape.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/Grape")]
        public async Task<ActionResult<Wine>> AddWineGrape(int id, Grape request)
        {
            Wine wine = await _context.Wines
                .Where(w => w.WineId == id)
                .Include(w => w.Grapes)
                .FirstOrDefaultAsync();
            if (wine == null)
                return NotFound();

            Grape grape = await _context.Grapes.FindAsync(request.GrapeId);
            if (grape == null)
                return NotFound();

            wine.Grapes.Add(grape);
            await _context.SaveChangesAsync();

            return wine;
        }

        /// <summary>
        /// Deletes many-to-many relationship Wine - Grape.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="grapeId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/Grape/{grapeId}")]
        public async Task<IActionResult> DeleteWineGrape(int id, int grapeId)
        {
            Wine wine = await _context.Wines
                .Where(w => w.WineId == id)
                .Include(w => w.Grapes)
                .FirstOrDefaultAsync();
            if (wine == null)
                return NotFound();

            Grape grape = await _context.Grapes.FindAsync(grapeId);
            if (grape == null)
                return NotFound();

            wine.Grapes.Remove(grape);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Adds many-to-many relationship Wine - Region
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/Region")]
        public async Task<ActionResult<Wine>> AddWineRegion(int id, Region request)
        {
            Wine wine = await _context.Wines
                .Where(w => w.WineId == id)
                .Include(w => w.Regions)
                .FirstOrDefaultAsync();
            if (wine == null)
                return NotFound();

            Region region = await _context.Regions.FindAsync(request.RegionId);
            if (region == null)
                return NotFound();

            wine.Regions.Add(region);
            await _context.SaveChangesAsync();

            return wine;
        }

        /// <summary>
        /// Deletes many-to-many relationship Wine - Region
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/Region/{regionId}")]
        public async Task<IActionResult> DeleteWineRegion(int id, int regionId)
        {
            Wine wine = await _context.Wines
                .Where(w => w.WineId == id)
                .Include(w => w.Regions)
                .FirstOrDefaultAsync();
            if (wine == null)
                return NotFound();

            Region region = await _context.Regions.FindAsync(regionId);
            if (region == null)
                return NotFound();

            wine.Regions.Remove(region);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Adds Rating relationship to Wine.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/Rating")]
        public async Task<ActionResult<Wine>> AddWineRating(int id, Rating request)
        {
            Wine wine = await _context.Wines
                .Where(w => w.WineId == id)
                .Include(w => w.Ratings)
                .FirstOrDefaultAsync();
            if (wine == null)
                return NotFound();

            Rating rating = await _context.Ratings.FindAsync(request.RatingId);
            if (rating == null)
                return NotFound();

            wine.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return wine;
        }

        // DELETE: api/Wines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWine(int id)
        {
            Wine wine = await _context.Wines.FindAsync(id);
            if (wine == null)
            {
                return NotFound();
            }

            _context.Wines.Remove(wine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WineExists(int id)
        {
            return _context.Wines.Any(e => e.WineId == id);
        }
    }
}
