namespace EliteBookings.DTOModels.Reservation
{
    public class DTOReservation
    {
        public long ReservationId { get; set; }
        public long? UserId { get; set; }
        public long? ClubId { get; set; }
        public long? SectionId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string? Status { get; set; }
        public long? BottleCount { get; set; }
        //public virtual Club? Club { get; set; }
        //public virtual Section? Section { get; set; }
        //public virtual AppUser? User { get; set; }
    }
}
