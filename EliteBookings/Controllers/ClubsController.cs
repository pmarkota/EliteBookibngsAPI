using EliteBookings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EliteBookings.Controllers
{
    public class EditClubRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int ClubId { get; set; }

    }
    public class PostClubRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int AdminUserId { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly postgresContext _db;

        public ClubsController(postgresContext db)
        {
            _db = db;
        }

        // GET: api/Clubs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Club>>> GetClubs()
        {
            return Ok(await _db.Clubs.ToListAsync());
        }

        // GET: api/Clubs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Club>> GetClub(int id)
        {
            var club = await _db.Clubs.FindAsync(id);

            if (club == null)
            {
                return NotFound();
            }

            return Ok(club);
        }

        // PUT: api/Clubs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClub(int id, EditClubRequest club)
        {
            if (id != club.ClubId)
            {
                return BadRequest();
            }

            var clubToEdit = await _db.Clubs.FindAsync(id);
            clubToEdit.ClubName = club.Name;
            clubToEdit.Address = club.Address;
            clubToEdit.Description = club.Description;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(clubToEdit);
        }
        // POST: api/Clubs
        [HttpPost]
        public async Task<ActionResult<Club>> PostClub(PostClubRequest club)
        {
            var newClub = new Club
            {
                ClubName = club.Name,
                Address = club.Address,
                Description = club.Description,
                AdminUserId = club.AdminUserId
            };
            _db.Clubs.Add(newClub);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClub), new { id = newClub.ClubId }, newClub);
        }

        // DELETE: api/Clubs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await _db.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }

            _db.Clubs.Remove(club);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool ClubExists(int id)
        {
            return _db.Clubs.Any(e => e.ClubId == id);
        }
    }
}
