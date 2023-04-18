using Application.Activities;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Mapping for Edit
            CreateMap<Activity, Activity>();

            //AutoMapper Queryable extensions - Mapping for Projection Host DTO
            CreateMap<Activity, ActivityDTO>()
                //Map from host
                .ForMember(d => d.HostUserName, o => o.MapFrom(s => s.Attendees
                .FirstOrDefault(x => x.IsHost).AppUser.UserName));

            //Map from ActivityAttendee TO Profile Object
            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.AppUser.Email));
        }
    }
}
