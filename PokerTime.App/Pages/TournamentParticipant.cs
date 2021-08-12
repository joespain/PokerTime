using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Interfaces;
using PokerTime.App.Services;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public partial class TournamentParticipant : ComponentBase
    {


        //TournamentStructure
        public TournamentTracking Tracker { get; set; } = new TournamentTracking();

        //BlindLevels
        public BlindLevel CurrentBlindLevel { get; set; } = new BlindLevel();
        public BlindLevel NextBlindLevel { get; set; } = new BlindLevel();

        //Event
        [Parameter]
        public Guid TrackerId { get; set; }
        public Event Event { get; set; }

        //Timer
        public TimeSpan TimeLeft { get; set; } = new TimeSpan();
        public bool IsTimerRunning { get; set; } = false;
        public string ButtonName { get; set; } = "Start";


        //Services
        [Inject]
        public ITournamentTrackingDataService TrackingDataService { get; set; }
        [Inject]
        public ILogger<EditEvent> Logger { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Tracker = await TrackingDataService.GetTournamentTracking(TrackerId);

                if (Tracker == null)
                {
                    //error
                    Logger.LogDebug("TournamentTracker not found.");
                }
                SetTimer();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);
            }

        }

    }
}
