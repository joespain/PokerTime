using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PokerTime.App.Interfaces;
using PokerTime.App.Services;
using PokerTime.Shared.Entities;
using System;
using System.Linq;
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
        public TournamentStructure Structure { get; set; } = new TournamentStructure();
        public BlindLevel CurrentBlindLevel { get; set; } = new BlindLevel();
        public BlindLevel NextBlindLevel { get; set; } = new BlindLevel();
        public int CurrentBlindLevelIndex { get; set; } = 0;

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
        [Inject]
        IJSRuntime _jsRuntime { get; set; }

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

                //Initialize the Tournament Structure
                Structure = await TrackingDataService.GetTournamentStructure(TrackerId, Tracker.CurrentBlindLevel.TournamentStructureId);
                if(Structure == null)
                {
                    throw new Exception("Error loading Tournament Structure.");
                }
                CurrentBlindLevel = Structure.BlindLevels.ToList().ElementAt(CurrentBlindLevelIndex);
                NextBlindLevel = Structure.BlindLevels.ToList().ElementAt(CurrentBlindLevelIndex + 1);

                //This timer will go off every 5 seconds to call the CheckTracker function.
                FiveSecondTimer = new Timer(5000);
                FiveSecondTimer.Elapsed += new ElapsedEventHandler(CheckTracker);
                FiveSecondTimer.Start();

                SetTimer();
                Timer();
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

        public async void Timer()
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

                if (TimeLeft <= new TimeSpan()) //If timeleft has expired and the timer is still running (the blindlevel has ended)
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

            if (oldTimerState != newTimerState && Tracker != null) //If the timer state has changed or blind levels incremented
            {
                //if (BlindLevelsIncremented)//If the blindlevel incremented, update the timeleft.
                //{
                //    while(Tracker.TimeRemaining < new TimeSpan(0, 0, 5))
                //    {
                //        await Task.Delay(1000);
                //        Tracker = await TrackingDataService.GetTournamentTracking(TrackerId); //Update Tracker
                //    }
                //    if(DateTime.UtcNow - Tracker.Time > new TimeSpan())
                //    {
                //        TimeLeft = new TimeSpan(0, Tracker.CurrentBlindLevel.Minutes, 0) - (DateTime.UtcNow - Tracker.Time) + new TimeSpan(0,0,2);
                //    }
                //    else
                //    {
                //        //TimeLeft -= (DateTime.UtcNow - Tracker.Time) - new TimeSpan(0, 0, 1);
                //    }
                    
                //    BlindLevelsIncremented = false;
                //}
                if (Tracker.IsTimerRunning)//If the timer started , subtract the time the timer started from current time to adjust for lag
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
            //Increment the CurrentBlindLevelIndex and update the Current and Next BlindLevels.
            CurrentBlindLevelIndex++;
            CurrentBlindLevel = Structure.BlindLevels.ToList().ElementAt(CurrentBlindLevelIndex);
            NextBlindLevel = Structure.BlindLevels.ToList().ElementAt(CurrentBlindLevelIndex + 1);

            //Update TimeLeft with the CurrentBlindLevel's minutes property.
            TimeLeft = new TimeSpan(0, CurrentBlindLevel.Minutes, 0);
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
