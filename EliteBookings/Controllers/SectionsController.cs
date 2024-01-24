using AutoMapper;
using EliteBookings.DTOModels.Section;
using EliteBookings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EliteBookings.Controllers
{

    public class PutSection
    {
        public long SectionId { get; set; }
        public long? ClubId { get; set; }
        public string? TableName { get; set; }
        public long? Capacity { get; set; }
    }
    public class PostSection
    {
        public long? ClubId { get; set; }
        public string? TableName { get; set; }
        public long? Capacity { get; set; }

    }
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly postgresContext _db;
        private readonly IMapper _mapper;

        public SectionsController(postgresContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // GET: api/Sections and for club
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOSection>>> GetSections()
        {
            var sections = await _db.Sections.ToListAsync();
            var sectionsDTO = _mapper.Map<IEnumerable<DTOSection>>(sections);
            return Ok(sectionsDTO);
        }
        // GET: api/Sections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Section>> GetSection(long id)
        {
            var sectionDTO = await _db.Sections.FindAsync(id);
            if (sectionDTO == null)
            {
                return NotFound();
            }
            var section = _mapper.Map<DTOSection>(sectionDTO);
            return Ok(section);
        }

        //GET: api/Sections/Club/5
        [HttpGet("Club/{id}")]
        public async Task<ActionResult<IEnumerable<DTOSection>>> GetSectionsByClub(long id)
        {
            var sections = await _db.Sections.Where(s => s.ClubId == id).ToListAsync();
            var sectionsDTO = _mapper.Map<IEnumerable<DTOSection>>(sections);
            return Ok(sectionsDTO);
        }

        // PUT: api/Sections/5

        [HttpPut]
        public async Task<IActionResult> PutSection(PutSection section)
        {
            var sectionDB = new Section
            {
                SectionId = section.SectionId,
                ClubId = section.ClubId,
                TableName = section.TableName,
                Capacity = section.Capacity
            };
            _db.Entry(sectionDB).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionExists(section.SectionId))
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

        // POST: api/Sections
        [HttpPost]
        public async Task<ActionResult<Section>> PostSection(PostSection section)
        {
            var sectionDB = new Section
            {
                ClubId = section.ClubId,
                TableName = section.TableName,
                Capacity = section.Capacity
            };
            _db.Sections.Add(sectionDB);
            await _db.SaveChangesAsync();
            return CreatedAtAction("GetSection", new { id = sectionDB.SectionId }, sectionDB);
        }

        // DELETE: api/Sections/5   
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSection(long id)
        {
            var section = await _db.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            _db.Sections.Remove(section);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Route("DeleteSections")]
        public async Task<IActionResult> DeleteSections(IEnumerable<long> ids)
        {
            var sections = await _db.Sections.Where(s => ids.Contains(s.SectionId)).ToListAsync();
            if (sections == null)
            {
                return NotFound();
            }
            _db.Sections.RemoveRange(sections);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        private bool SectionExists(long sectionId)
        {
            return _db.Sections.Any(e => e.SectionId == sectionId);
        }
    }
}