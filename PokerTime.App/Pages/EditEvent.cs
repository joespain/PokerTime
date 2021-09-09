using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Interfaces;
using PokerTime.Shared.Email;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public partial class EditEvent : ComponentBase
    {
        //Host
        public Guid HostId { get; set; }
        public Host Host { get; set; }

        //TournamentStructure
        public List<TournamentStructureModel> TournamentStructures { get; set; } = new List<TournamentStructureModel>();

        //Invitees
        public List<InviteeModel> PriorInvitees { get; set; } = new List<InviteeModel>();

        //Event
        [Parameter]
        public Guid EventId { get; set; }
        public int NewEventId { get; set; }
        public EventModel Event { get; set; } = new EventModel();
        public bool IsNewEvent { get; set; }
        public bool IsSave { get; set; } = false;

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
        public IEmailDataService EmailDataService { get; set; }
        [Inject]
        public ITournamentTrackingDataService TournamentTrackingDataService { get; set; }
        [Inject]
        public ILogger<EditEvent> Logger { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }

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

                if (EventId != new Guid()) //If existing Event
                {
                    IsNewEvent = false;
                    Event = Mapper.Map<EventModel>(await EventDataService.GetEvent(EventId));

                    if (Event.Invitees == null)
                    {
                        Event.Invitees = Mapper.Map<List<InviteeModel>>(await InviteeDataService.GetInvitees());
                    }
                    foreach (var invitee in Event.Invitees)
                    {
                        invitee.IsDisabled = true;
                    }

                    
                }
                else //If new Event
                {
                    IsNewEvent = true;
                    Event.Id = Guid.NewGuid();
                    Event.Date = DateTime.Today;
                    Event.Time = DateTime.Now;
                    PriorInvitees = Mapper.Map<List<InviteeModel>>(await InviteeDataService.GetInvitees()).ToList();
                    Event.Invitees = new List<InviteeModel>();
                    Event.Invitees.Add(new InviteeModel()
                    {
                        HostId = HostId,
                        IsDisabled = false
                    });
                }

                Event.Invitees = Event.Invitees.OrderBy(x => x.IsDisabled).ThenBy(x => x.Name).ToList();
                TournamentStructures = Mapper.Map<List<TournamentStructureModel>>(await StructureDataService.GetStructures());

                await GetOtherInvitees();

                

                if (TournamentStructures == null)
                {
                    //Error, must add new structure
                }
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);
            }
        }

        public async Task HandleValidSubmit()
        {
            try
            {
                var invitees = Event.Invitees;

                if(Event.HostId == new Guid())
                {
                    Event.HostId = HostId;
                }

                if (IsNewEvent)
                {
                    Event = Mapper.Map<EventModel>(
                        await EventDataService.AddEvent(
                            Mapper.Map<Event>(Event)));
                    IsNewEvent = false;
                }
                else
                {
                    await EventDataService.UpdateEvent(Mapper.Map<Event>(Event));
                }
                
                await EmailInvitees();

                if (IsSave)
                {
                    NavigationManager.NavigateTo("/events");
                }
                else
                {
                    BeginEvent();
                }
                
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
            }
        }

        public async Task GetOtherInvitees()
        {
            //This method populates the PriorInvitees property, which contains all other invitees the Host has
            //that have not been used in the current event.
            PriorInvitees = Mapper.Map<List<InviteeModel>>(await InviteeDataService.GetInvitees());
            foreach(var thisEventInvitee in Event.Invitees.ToList())
            {
                foreach(var priorInvitee in PriorInvitees.ToList())
                {
                    if(thisEventInvitee.Id == priorInvitee.Id)
                    {
                        PriorInvitees.Remove(priorInvitee);
                    }
                    else
                    {
                        priorInvitee.IsDisabled = true;
                    }
                }
            }
        }

        public void AddPriorInvitee(InviteeModel Invitee)
        {
            Event.Invitees.Add(Invitee);
            PriorInvitees.Remove(Invitee);
            StateHasChanged();
        }

        public async Task UpdateInvitees(List<InviteeModel> invitees)
        {
            foreach (var invitee in invitees)
            {
                if (invitee.Id == 0)
                {
                    invitee.HostId = HostId;
                    await InviteeDataService.AddInvitee(Mapper.Map<Invitee>(invitee));
                }
                else
                {
                    await InviteeDataService.UpdateInvitee(Mapper.Map<Invitee>(invitee));
                }
                Event.Invitees.Add(invitee);
            }
        }

        public void AddInvitee()
        {
            var newInvitee = new InviteeModel()
            {
                HostId = HostId,
                IsDisabled = false
            };

            Event.Invitees.Add(newInvitee);
            Event.Invitees = Event.Invitees.OrderBy(x => x.IsDisabled).ThenBy(x => x.Name).ToList();
            StateHasChanged();
        }

        public void EditInvitee(InviteeModel invitee)
        {
            //This disables/enables the text boxes to be edited.
            invitee.IsDisabled = !invitee.IsDisabled;
            StateHasChanged();
        }

        public async Task UpdateStructure()
        {
            //Updating the structure with the number of events run. This will probably change.
            var structure = TournamentStructures.FirstOrDefault(t => t.Id == Event.TournamentStructureId);
            structure.NumberOfEvents += 1;
            await StructureDataService.UpdateStructure(Mapper.Map<TournamentStructure>(structure));
        }

        public async Task EmailInvitee(InviteeModel invitee)
        {
            var email = new MailRequest();
            email.ToEmail = invitee.Email;
            email.Subject = "Join the PokerTime Tournament";
            email.Body = $"Join our tournament by clicking the following link: https://localhost:5015/tournament/{Event.Id}";

            await EmailDataService.SendEmail(email);
        }

        public async Task EmailInvitees()
        {
            foreach(var invitee in Event.Invitees)
            {
                await EmailInvitee(invitee);
            }
        }

        public void NavigateToEvents()
        {
            NavigationManager.NavigateTo("/events");
        }

        public void RemoveInvitee(InviteeModel invitee)
        {
            Event.Invitees.Remove(invitee);
            if(invitee.Id != 0)
            {
                PriorInvitees.Add(invitee);
            }
        }

        public void BeginEvent()
        {
            NavigationManager.NavigateTo($"/events/inprogress/{Event.Id}");
        }

       
    }
}
