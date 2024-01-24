using AutoMapper;

namespace EliteBookings.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.AppUser, DTOModels.AppUser.DTOAppUser>();
            CreateMap<Models.AppUser, DTOModels.AppUser.DTOAppUser>().ReverseMap();

            CreateMap<Models.Section, DTOModels.Section.DTOSection>();
            CreateMap<Models.Section, DTOModels.Section.DTOSection>().ReverseMap();

            //CreateMap<Models.Club, DTOModels.Club.DTOClub>();
            //CreateMap<Models.Club, DTOModels.Club.DTOClub>().ReverseMap();

            CreateMap<Models.Reservation, DTOModels.Reservation.DTOReservation>();
            CreateMap<Models.Reservation, DTOModels.Reservation.DTOReservation>().ReverseMap();
        }
    }
}
