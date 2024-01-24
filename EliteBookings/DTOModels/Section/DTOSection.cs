namespace EliteBookings.DTOModels.Section
{
    public class DTOSection
    {
        public long SectionId { get; set; }
        public long? ClubId { get; set; }
        public string? TableName { get; set; }
        public long? Capacity { get; set; }
        //public virtual Club? Club { get; set; }
        //public virtual ICollection<Condition> Conditions { get; set; }
        //public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
