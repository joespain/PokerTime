using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Client.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Pages
{
    public partial class AddEvent
    {
        //Host
        public Guid HostId { get; set; }
        public Host Host { get; set; }

        //TournamentStructure
        public List<TournamentStructure> TournamentStructures { get; set; } = new List<TournamentStructure>();

        //Invitees
        public List<Invitee> Invitees { get; set; } = new List<Invitee>();

        //Event
        public Event NewEvent { get; set; } = new Event();

        //Services
        [Inject]
        public IHostDataService UserDataService { get; set; }
        [Inject]
        public IInviteeDataService InviteeDataService { get; set; }
        [Inject]
        public IEventDataService EventDataService { get; set; }
        [Inject]
        public IStructureDataService StructureDataService { get; set; }
        [Inject]
        public IBlindLevelDataService BlindLevelDataService { get; set; }
        [Inject]
        public ILogger<AddEvent> Logger { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Saved = false;

                Host = await UserDataService.GetHost(Guid.Parse("48b51074-220e-4275-b3f6-ed41b8319832"));
                HostId = Host.Id;

                TournamentStructures = (await StructureDataService.GetStructures(HostId)).ToList();

                if(TournamentStructures == null)
                {
                    //Error, must add new structure
                }

                Invitees = (await InviteeDataService.GetInvitees(HostId)).ToList();

                if(Invitees == null)
                {
                    AddInvitee();
                }

            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);
            }
        }

        public void AddInvitee()
        {
            Invitees.Add(new Invitee() { HostId = HostId });
        }

        public async Task HandleValidSubmit()
        {
            

        }

        public async Task HandleInvalidSubmit()
        {

        }

        public void NavigateToStructure()
        {

        }
    }
}
