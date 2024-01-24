namespace EliteBookings.DTOModels.AppUser
{
    public class DTOAppUser
    {
        public long? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsConfirmed { get; set; }
        //public virtual ICollection<Club> Clubs { get; set; }
        //public virtual ICollection<Reservation> Reservations { get; set; }
        public int? ReservationCount { get; set; }
    }
}
