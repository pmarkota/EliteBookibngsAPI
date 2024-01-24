using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EliteBookings.Models
{
    /// <summary>
    /// users table
    /// </summary>
    public partial class AppUser
    {
        public AppUser()
        {
            Clubs = new HashSet<Club>();
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        [Column("user_id")]
        public long UserId { get; set; }
        [Column("username", TypeName = "character varying")]
        public string? Username { get; set; }
        [Column("password_hash")]
        public string? PasswordHash { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("role", TypeName = "character varying")]
        public string? Role { get; set; }
        [Column("phone_number", TypeName = "character varying")]
        public string? PhoneNumber { get; set; }
        [Column("is_confirmed")]
        public bool? IsConfirmed { get; set; }

        [InverseProperty(nameof(Club.AdminUser))]
        public virtual ICollection<Club> Clubs { get; set; }
        [InverseProperty(nameof(Reservation.User))]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
