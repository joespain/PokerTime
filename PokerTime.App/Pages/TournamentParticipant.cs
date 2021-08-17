using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public partial class TournamentParticipant : ComponentBase
    {
        //TournamentTracker
        public TournamentTracking Tracker { get; set; } = new TournamentTracking();
        [Parameter]
        public Guid TrackerId { get; set; }

        //Timer
        public TimeSpan TimeLeft { get; set; } = new TimeSpan();

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
                await CheckTracker();

                if (Tracker == null)
                {
                    //error
                    Logger.LogDebug("TournamentTracker not found.");
                }

                SetTimer();
                await Timer();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);
            }
        }

        public void SetTimer()
        {
            TimeLeft = Tracker.TimeRemaining;
        }

        public async Task Timer()
        {
            while (Tracker.IsTournamentRunning)
            {
                int counter = 1;
                while ((TimeLeft > new TimeSpan()) && Tracker.IsTimerRunning)
                {
                    await Task.Delay(1000);
                    TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));
                    StateHasChanged();
                    counter++;
                    if(counter == 5)
                    {
                        await CheckTracker();
                        counter = 1;
                    }
                }
                if(TimeLeft == new TimeSpan())
                {
                    await TimeExpired();
                }
                StateHasChanged();
            }
        }

        public async Task CheckTracker()
        {
            Tracker = await TrackingDataService.GetTournamentTracking(TrackerId);
            //if ((Tracker.TimeRemaining - TimeLeft) > new TimeSpan(0, 0, 5)) //If the timer is off by more than 5 seconds, we reset it.
            //{
                TimeLeft = Tracker.TimeRemaining;
                StateHasChanged();
            //}
        }

        public async Task TimeExpired()
        {
            await PlaySound();
            await CheckTracker();
            await Timer();
        }

        public async Task PlaySound()
        {
            await _jsRuntime.InvokeAsync<string>("PlayAudio", "chime");
        }

    }
}
