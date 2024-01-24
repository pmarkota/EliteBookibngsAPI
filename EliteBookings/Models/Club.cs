using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EliteBookings.Models
{
    /// <summary>
    /// clubs table
    /// </summary>
    public partial class Club
    {
        public Club()
        {
            Reservations = new HashSet<Reservation>();
            Sections = new HashSet<Section>();
        }

        [Key]
        [Column("club_id")]
        public long ClubId { get; set; }
        [Column("club_name", TypeName = "character varying")]
        public string? ClubName { get; set; }
        [Column("admin_user_id")]
        public long? AdminUserId { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("address")]
        public string? Address { get; set; }

        [ForeignKey(nameof(AdminUserId))]
        [InverseProperty(nameof(AppUser.Clubs))]
        public virtual AppUser? AdminUser { get; set; }
        [InverseProperty(nameof(Reservation.Club))]
        public virtual ICollection<Reservation> Reservations { get; set; }
        [InverseProperty(nameof(Section.Club))]
        public virtual ICollection<Section> Sections { get; set; }
    }
}
