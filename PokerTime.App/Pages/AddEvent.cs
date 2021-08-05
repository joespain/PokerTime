﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public partial class AddEvent : ComponentBase
    {
        //Host
        public Guid HostId { get; set; }
        public Host Host { get; set; }

        //TournamentStructure
        
        public int TournamentStructureId { get; set; }
        public List<TournamentStructure> TournamentStructures { get; set; } = new List<TournamentStructure>();

        //Invitees
        public List<Invitee> Invitees { get; set; } = new List<Invitee>();

        //Event
        [Parameter]
        public int EventId { get; set; }
        public int NewEventId { get; set; }
        public Event Event { get; set; } = new Event();

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
                Host = await UserDataService.GetHost();
                HostId = Host.Id;

                if(EventId != 0)
                {
                    Event = await EventDataService.GetEvent(EventId);
                }
                else
                {
                    Event = new Event();
                }

                TournamentStructures = (await StructureDataService.GetStructures()).ToList();

                if (TournamentStructures == null)
                {
                    //Error, must add new structure
                }

                Invitees = (await InviteeDataService.GetInvitees()).ToList();

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
            try
            {
                Event.Date = DateTime.Today;
                Event.TournamentStructureId = TournamentStructureId;
                
                Event.HostId = HostId;
                Event.EventLinkId = Guid.NewGuid();

                await UpdateInvitees();
                await EmailInvitees();

                if (Event.Id == 0)
                {
                    Event = await EventDataService.AddEvent(Event);
                }
                else
                {
                    await EventDataService.UpdateEvent(Event);
                }
                BeginEvent();
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
            }
        }

        public async Task UpdateInvitees()
        {
            foreach (var invitee in Invitees)
            {
                if (invitee.Id == 0)
                {
                    invitee.HostId = HostId;
                    await InviteeDataService.AddInvitee(invitee);
                }
                else
                {
                    await InviteeDataService.UpdateInvitee(invitee);
                }
            }
        }

        public async Task EmailInvitees()
        {
            //Email invites
            
        }

        public async Task HandleInvalidSubmit()
        {

        }

        public void NavigateToStructure()
        {

        }

        public void RemoveInvitee(Invitee invitee)
        {
            Invitees.Remove(invitee);
        }

        public void BeginEvent()
        {
            NavigationManager.NavigateTo($"/events/{Event.Id}/{Event.EventLinkId}");
        }
    }
}
