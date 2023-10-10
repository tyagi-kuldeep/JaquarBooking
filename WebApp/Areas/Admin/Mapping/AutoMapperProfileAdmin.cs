using AutoMapper;
using Domain.Entities;
using WebApp.Areas.Admin.Models.Booking;
using WebApp.Areas.Admin.Models.Master;
using WebApp.Areas.Admin.Models.Participant;
using WebApp.Areas.Admin.Models.Profile;
using WebApp.Areas.Admin.Models.Role;
using WebApp.Areas.Admin.Models.User;

namespace WebApp.Areas.Admin.Mapping
{
    public class AutoMapperProfileAdmin : Profile
    {
        public AutoMapperProfileAdmin()
        {
            CreateMap<JB_MasterCountry, AddCountryVm>().ReverseMap();
            CreateMap<JB_MasterState, AddStateVm>().ReverseMap();
            CreateMap<Users, AddUserVm>().ReverseMap();
            CreateMap<AuthRole, AddRoleVm>().ReverseMap();
            CreateMap<Users, AddProfileVm>().ReverseMap();
            CreateMap<JB_Booking, AddBookingVm>().ReverseMap();
            CreateMap<JB_Participant, WebApp.Areas.Admin.Models.Booking.AddParticipantVm>().ReverseMap();
            //CreateMap<JB_Participant, AddParticipantVm>().ReverseMap();
        }
    }
}
