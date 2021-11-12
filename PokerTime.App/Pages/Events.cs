using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public partial class Events : ComponentBase
    {
        //TournamentStructures
        public TournamentStructure TournamentStructure { get; set; } = new TournamentStructure();

        //Events
        public List<Event> FutureEvents { get; set; } = new List<Event>();
        public List<Event> PastEvents { get; set; } = new List<Event>();

        //Other
        public bool CanShowScreen { get; set; } = false;

        //Services
        [Inject]
        public IStructureDataService StructureDataService { get; set; }
        [Inject]
        public IEventDataService EventDataService { get; set; }
        [Inject]
        public ILogger<EditEvent> Logger { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }




        protected override async Task OnInitializedAsync()
        {
            try
            {
                var listOfEvents = (await EventDataService.GetEvents()).ToList();

                if(listOfEvents != null)
                {
                    PastEvents = listOfEvents.Where(e => e.Date < DateTime.Now)
                        .OrderBy(e => e.Date).ToList();
                    FutureEvents = listOfEvents.Where(e => e.Date > DateTime.Now)
                        .OrderBy(e => e.Date).ToList();
                }
                else
                {
                    //No events
                }
                CanShowScreen = true;
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);
            }

        }

        public async Task DeleteEvent(Event eventToDelete)
        {
            if (PastEvents.Contains(eventToDelete))
            {
                PastEvents.Remove(eventToDelete);
            }
            else
            {
                FutureEvents.Remove(eventToDelete);
            }
            await EventDataService.DeleteEvent(eventToDelete.Id);
        }

        public void StartEvent(Event theEvent) 
        {
            NavigationManager.NavigateTo($"/tournament/{theEvent.Id}");
        }

        public void NewEvent()
        {
            NavigationManager.NavigateTo($"/events/{new Guid()}");
        }

        public void EditEvent(Event theEvent)
        {
            NavigationManager.NavigateTo($"/events/{theEvent.Id}");
        }


    }
}
