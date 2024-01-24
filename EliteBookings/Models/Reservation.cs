using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EliteBookings.Models
{
    /// <summary>
    /// reservations table
    /// </summary>
    public partial class Reservation
    {
        [Key]
        [Column("reservation_id")]
        public long ReservationId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("club_id")]
        public long? ClubId { get; set; }
        [Column("section_id")]
        public long? SectionId { get; set; }
        [Column("reservation_date")]
        public DateTime? ReservationDate { get; set; }
        [Column("status", TypeName = "character varying")]
        public string? Status { get; set; }
        [Column("bottle_count")]
        public long? BottleCount { get; set; }
        [Column("number_of_people")]
        public long? NumberOfPeople { get; set; }

        [ForeignKey(nameof(ClubId))]
        [InverseProperty("Reservations")]
        public virtual Club? Club { get; set; }
        [ForeignKey(nameof(SectionId))]
        [InverseProperty("Reservations")]
        public virtual Section? Section { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AppUser.Reservations))]
        public virtual AppUser? User { get; set; }
    }
}
