using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PokerTime.App.Interfaces;
using PokerTime.App.Services;
using PokerTime.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public partial class TournamentParticipant : ComponentBase
    {
        //TournamentTracker
        public TournamentTracking Tracker { get; set; } = null;
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
                await CheckTracker(true);

                if (Tracker == null)
                {
                    //error
                    Logger.LogDebug("TournamentTracker not found.");
                }

                SetTimer();
                await Timer();
                StateHasChanged();
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
                int fiveSecondTimer = 0;
                while ((TimeLeft > new TimeSpan()) && Tracker.IsTimerRunning)
                {
                    await Task.Delay(1000);
                    TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));
                    StateHasChanged();
                    fiveSecondTimer++;
                    if(fiveSecondTimer == 5 && TimeLeft > new TimeSpan())
                    {
                        await CheckTracker();
                        fiveSecondTimer = 0;
                    }
                }
                if(Tracker.IsTimerRunning)
                {
                    await TimeExpired();
                    await CheckTracker();
                }
                StateHasChanged();
                await CheckTracker();
                await Task.Delay(5000);
            }
        }

        public async Task CheckTracker(bool isInitial = false)
        {
            bool TimerState = true; //Whether timer is running or not. 
            BlindLevel currentBL = new();

            if (!isInitial) //Skip these steps in the initial setup
            {
                TimerState = Tracker.IsTimerRunning;
                currentBL = Tracker.CurrentBlindLevel;
            }

            Tracker = await TrackingDataService.GetTournamentTracking(TrackerId);

            if (TimerState != Tracker.IsTimerRunning && Tracker != null) //If the timer state has changed
            {
                if (Tracker.IsTimerRunning) //If the timer is running, subtract the time the timer started from current time to adjust for lag
                {
                    TimeLeft -= (DateTime.UtcNow - Tracker.Time) - new TimeSpan(0,0,1);
                    await Timer();
                }
                else
                {
                    TimeLeft = Tracker.TimeRemaining;
                }
                
                //if(currentBL != Tracker.CurrentBlindLevel)
                //{
                //    await TimeExpired();
                //}
                StateHasChanged();
            }
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
