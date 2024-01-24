using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EliteBookings.Models
{
    /// <summary>
    /// sections table
    /// </summary>
    public partial class Section
    {
        public Section()
        {
            Conditions = new HashSet<Condition>();
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        [Column("section_id")]
        public long SectionId { get; set; }
        [Column("club_id")]
        public long? ClubId { get; set; }
        [Column("table_name", TypeName = "character varying")]
        public string? TableName { get; set; }
        [Column("capacity")]
        public long? Capacity { get; set; }

        [ForeignKey(nameof(ClubId))]
        [InverseProperty("Sections")]
        public virtual Club? Club { get; set; }
        [InverseProperty(nameof(Condition.Section))]
        public virtual ICollection<Condition> Conditions { get; set; }
        [InverseProperty(nameof(Reservation.Section))]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
