using AutoMapper;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;

namespace PokerTime.API.Data
{
    public class PTProfile : Profile
    {
        public PTProfile()
        {
            this.CreateMap<Host, HostModel>()
                .ReverseMap();

            this.CreateMap<TournamentStructure, TournamentStructureModel>()
                .ReverseMap();

            this.CreateMap<BlindLevel, BlindLevelModel>()
                .ReverseMap();

            this.CreateMap<Event, EventModel>()
                .ReverseMap();

            this.CreateMap<Invitee, InviteeModel>()
                .ReverseMap();

            this.CreateMap<TournamentTracking, TournamentTrackingModel>()
                .ReverseMap();


        }
    }
}
