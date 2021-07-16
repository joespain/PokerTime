using AutoMapper;
using PokerTime.API.Data.Entities;
using PokerTime.API.Models;

namespace PokerTime.API.Data
{
    public class PTProfile : Profile
    {
        public PTProfile()
        {
            this.CreateMap<TournamentStructure, TournamentStructureModel>()
                .ReverseMap();
            this.CreateMap<User, UserModel>()
                .ReverseMap();
            this.CreateMap<BlindLevel, BlindLevelModel>()
                .ReverseMap();
            this.CreateMap<Invitee, EventModel>()
                .ReverseMap();
            this.CreateMap<Invitee, InviteeModel>()
                .ReverseMap();
        }
    }
}
