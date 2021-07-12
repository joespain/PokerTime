using AutoMapper;
using PokerTime.API.Data.Entities;
using PokerTime.API.Models;

namespace PokerTime.API.Data
{
    public class PTProfile : Profile
    {
        public PTProfile()
        {
            this.CreateMap<Tournament, TournamentModel>()
                .ReverseMap();
            this.CreateMap<Guest, GuestModel>()
                .ReverseMap();
        }
    }
}
