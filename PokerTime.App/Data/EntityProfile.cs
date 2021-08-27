using AutoMapper;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;

namespace PokerTime.App.Data
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
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
                .ForMember(im => im.IsDisabled, i => i.Ignore()) //Do not map the IsDisabled property to the Invitee.
                .ReverseMap();
                

        }
    }
}
