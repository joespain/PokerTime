using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PokerTime.App.Interfaces;
using PokerTime.App.Services;
using PokerTime.Shared.Entities;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace PokerTime.App.Pages
{
    public partial class TournamentParticipant : ComponentBase
    {
        //TournamentTracker
        private TournamentTracking Tracker { get; set; } = null;
        [Parameter]
        public Guid TrackerId { get; set; }

        //Timer
        public TimeSpan TimeLeft { get; set; } = new TimeSpan();
        public Timer FiveSecondTimer;
        public bool BlindLevelsIncremented = false;
        public string Message { get; set; }
        public Event BlindLevelEnded { get; set; }

        //CSS Styles
        public string TimerStyle { get; set; }
        public string TimerColor { get; set; } = "white";


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
                //Initialize the Tracker
                Tracker = await TrackingDataService.GetTournamentTracking(TrackerId);

                if (Tracker == null) //Will only be null before tournament.
                {
                    Message = "Tournament has not started. Please wait.";
                }
                else if (!Tracker.IsTournamentRunning)
                {
                    Message = "Tournament Ended.";
                    throw new Exception("Tournament Ended.");
                }

                //This timer will go off every 5 seconds to call the CheckTracker function.
                FiveSecondTimer = new Timer(5000);
                FiveSecondTimer.Elapsed += new ElapsedEventHandler(CheckTracker);
                FiveSecondTimer.Start();

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
                while ((TimeLeft > new TimeSpan(0,0,0)) && Tracker.IsTimerRunning) //While timer is running and Timeleft > 0
                {
                    await Task.Delay(1000);
                    TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));
                    if (TimeLeft < new TimeSpan(0, 0, 31))
                    {
                        TimerColor = "red";
                    }
                    else
                    {
                        TimerColor = "white";
                    }
                    StateHasChanged();
                }

                if (Tracker.IsTimerRunning && (TimeLeft < new TimeSpan(0,0,1))) //If timeleft has expired and the timer is still running (the blindlevel has ended)
                {
                    TimeExpired();
                    IncrementBlindLevels();
                }
                else
                {
                    await Task.Delay(1000);
                }
                StateHasChanged();
            }
        }

        public async void CheckTracker(object sender, ElapsedEventArgs e)
        {
            //This function queries the server to get new data for the tracker. Is connected to the FiveSecondTimer and is called every 5 seconds. 

            bool oldTimerState = Tracker.IsTimerRunning; //Initial Timer State.

            Tracker = await TrackingDataService.GetTournamentTracking(TrackerId); //Update Tracker

            var newTimerState = Tracker.IsTimerRunning;

            if ((oldTimerState != newTimerState || BlindLevelsIncremented) && Tracker != null) //If the timer state has changed or blind levels incremented
            {
                if (BlindLevelsIncremented)//If the blindlevel incremented, update the timeleft.
                {
                    while(Tracker.TimeRemaining < new TimeSpan(0, 0, 2))
                    {
                        await Task.Delay(1000);
                        Tracker = await TrackingDataService.GetTournamentTracking(TrackerId); //Update Tracker
                    }
                    TimeLeft = Tracker.TimeRemaining;
                    BlindLevelsIncremented = false;
                }
                else if (Tracker.IsTimerRunning)//If the timer started , subtract the time the timer started from current time to adjust for lag
                {
                    TimeLeft -= (DateTime.UtcNow - Tracker.Time) - new TimeSpan(0,0,1);
                }
                else //If the timer stopped, reset it.
                {
                    SetTimer();
                }
                
            }
            StateHasChanged();
        }

        public void IncrementBlindLevels()
        {
            //This function queries the server to get new data for the tracker. Is only called at the end of a blind level

            Tracker.CurrentBlindLevel = Tracker.NextBlindLevel;
            TimeLeft = new TimeSpan(0, Tracker.CurrentBlindLevel.Minutes, 0);
            BlindLevelsIncremented = true;
            StateHasChanged();
        }


        public async void TimeExpired()
        {
            await PlaySound();
        }

        public async Task PlaySound()
        {
            await _jsRuntime.InvokeAsync<string>("PlayAudio", "chime");
        }

    }
}
