using AutoMapper;
using EliteBookings.DTOModels.Reservation;
using EliteBookings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EliteBookings.Controllers
{
    public class PutReservation
    {
        public long ReservationId { get; set; }
        public long? ClubId { get; set; }
        public long? SectionId { get; set; }
        public long? UserId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public long? NumberOfPeople { get; set; }
        public string? ReservationStatus { get; set; }
    }

    public class PostReservation
    {
        public long? ClubId { get; set; }
        public long? SectionId { get; set; }
        public long? UserId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public long? NumberOfPeople { get; set; }
        public string? ReservationStatus { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly postgresContext _db;
        private readonly IMapper _mapper;

        public ReservationsController(postgresContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOReservation>>> GetReservations()
        {
            var reservations = await _db.Reservations.ToListAsync();
            var reservationsDTO = _mapper.Map<IEnumerable<DTOReservation>>(reservations);
            return Ok(reservationsDTO);
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(long id)
        {
            var reservationDTO = await _db.Reservations.FindAsync(id);
            if (reservationDTO == null)
            {
                return NotFound();
            }
            var reservation = _mapper.Map<DTOReservation>(reservationDTO);
            return Ok(reservation);
        }

        //GET: api/Reservations/Club/5
        [HttpGet("Club/{id}")]
        public async Task<ActionResult<IEnumerable<DTOReservation>>> GetReservationsByClub(long id)
        {
            var reservations = await _db.Reservations.Where(r => r.ClubId == id).ToListAsync();
            var reservationsDTO = _mapper.Map<IEnumerable<DTOReservation>>(reservations);
            return Ok(reservationsDTO);
        }

        //GET: api/Reservations/Section/5
        [HttpGet("Section/{id}")]
        public async Task<ActionResult<IEnumerable<DTOReservation>>> GetReservationsBySection(long id)
        {
            var reservations = await _db.Reservations.Where(r => r.SectionId == id).ToListAsync();
            var reservationsDTO = _mapper.Map<IEnumerable<DTOReservation>>(reservations);
            return Ok(reservationsDTO);
        }

        // PUT: api/Reservations/5
        [HttpPut]
        public async Task<IActionResult> PutReservation(PutReservation reservation)
        {
            var reservationFromDb = await _db.Reservations.FindAsync(reservation.ReservationId);
            if (reservationFromDb == null)
            {
                return NotFound();
            }
            // if values  are null keep the old values in the database 
            if (reservation.ClubId != null)
            {
                reservationFromDb.ClubId = reservation.ClubId;
            }
            if (reservation.SectionId != null)
            {
                reservationFromDb.SectionId = reservation.SectionId;
            }
            if (reservation.UserId != null)
            {
                reservationFromDb.UserId = reservation.UserId;
            }
            if (reservation.ReservationDate != null)
            {
                reservationFromDb.ReservationDate = reservation.ReservationDate;
            }
            if (reservation.NumberOfPeople != null)
            {
                reservationFromDb.NumberOfPeople = reservation.NumberOfPeople;
            }
            if (reservation.ReservationStatus != null)
            {
                reservationFromDb.Status = reservation.ReservationStatus;
            }

            _db.Entry(reservationFromDb).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(reservation.ReservationId))
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

        // POST: api/Reservations
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(PostReservation reservation)
        {
            var reservationToDB = new Reservation
            {
                ClubId = reservation.ClubId,
                SectionId = reservation.SectionId,
                UserId = reservation.UserId,
                ReservationDate = reservation.ReservationDate,
                NumberOfPeople = reservation.NumberOfPeople,
                Status = "Pending"
            };
            _db.Reservations.Add(reservationToDB);
            await _db.SaveChangesAsync();
            return CreatedAtAction("GetReservation", new { id = reservationToDB.ReservationId }, reservationToDB);
        }

        // DELETE: api/Reservations/5
        [HttpDelete]
        public async Task<IActionResult> DeleteReservation(long id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        // DELETE: delete multiple reservations at once, takes in a list of reservation ids
        [HttpPost]
        [Route("DeleteReservations")]
        public async Task<IActionResult> DeleteReservations(IEnumerable<long> ids)
        {
            foreach (var id in ids)
            {
                var reservation = await _db.Reservations.FindAsync(id);
                if (reservation == null)
                {
                    return NotFound();
                }
                _db.Reservations.Remove(reservation);
            }
            await _db.SaveChangesAsync();
            return NoContent();
        }
        private bool ReservationExists(long id)
        {
            return _db.Reservations.Any(e => e.ReservationId == id);
        }
    }
}
