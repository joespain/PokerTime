using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PokerTime.App.Interfaces;
using PokerTime.App.Services;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
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
        public TournamentStructure Structure { get; set; } = new();
        public BlindLevel CurrentBlindLevel { get; set; } = new();
        public BlindLevel NextBlindLevel { get; set; } = new();
        public int CurrentBlindLevelIndex { get; set; } = 0;
        public DateTime TimeStateChanged { get; set; } = new();
        public List<BlindLevel> BlindLevels { get; set; } = new();

        //Timer
        public TimeSpan TimeLeft { get; set; } = new();
        public Timer FiveSecondTimer { get; set; }
        public bool BlindLevelsIncremented { get; set; } = false;
        public TimeSpan PriorTimeLeft { get; set; } = new();
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
                if (TrackerId == new Guid()) //Parameter TrackerId must exist
                {
                    throw new Exception("No Tracker Id");
                }

                //Initialize the Tracker
                Tracker = await TrackingDataService.GetTournamentTracking(TrackerId);

                if (Tracker.Id == new Guid()) //Will be new Guid before host has opened tournament page.
                {
                    Message = "Tournament has not started. Please wait...";
                }
                else if (!Tracker.IsTournamentRunning)
                {
                    Message = "Tournament Ended.";
                    throw new Exception("Tournament Ended.");
                }

                //Initialize the Tournament Structure, save BlindLevels to local property
                Structure = await TrackingDataService.GetTournamentStructure(TrackerId, Tracker.CurrentBlindLevel.TournamentStructureId);
                BlindLevels = Structure.BlindLevels.OrderBy(x => x.SequenceNum).ToList();
                
                if(Structure is null)
                {
                    throw new Exception("Error loading Tournament Structure.");
                }
                CurrentBlindLevel = Tracker.CurrentBlindLevel;
                NextBlindLevel = Tracker.NextBlindLevel;
                SetTimer();

                if (Tracker.IsTimerRunning)
                {
                    CalculateCurrentBlindLevel();
                }

                //This timer will go off every 5 seconds to call the CheckTracker function.
                FiveSecondTimer = new Timer(5000);
                FiveSecondTimer.Elapsed += new ElapsedEventHandler(CheckTracker);
                FiveSecondTimer.Start();

                Timer();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
        }

        public void CalculateCurrentBlindLevel()
        {
            TimeSpan timeSinceLastAction = GetTimeDifference();
            if (timeSinceLastAction < TimeLeft) //If time is still within the current Blind Level
            {
                TimeLeft -= timeSinceLastAction;
            }
            else
            {
                timeSinceLastAction -= TimeLeft;
                CurrentBlindLevel = NextBlindLevel;
                NextBlindLevel = BlindLevels.ElementAt(BlindLevels.IndexOf(CurrentBlindLevel) + 1);

                while (timeSinceLastAction > new TimeSpan()) //Loop through the Blind Levels, subtracting the times until we get to the current Blind Level
                {
                    if (new TimeSpan(0, CurrentBlindLevel.Minutes, 0) > timeSinceLastAction)
                    {
                        TimeLeft = new TimeSpan(0, CurrentBlindLevel.Minutes, 0) - timeSinceLastAction;
                        timeSinceLastAction = new TimeSpan();
                    }
                    else
                    {
                        timeSinceLastAction -= new TimeSpan(0, CurrentBlindLevel.Minutes, 0);
                        CurrentBlindLevel = NextBlindLevel;
                        NextBlindLevel = BlindLevels.ElementAt(BlindLevels.IndexOf(CurrentBlindLevel) + 1);
                    }
                }
            }
        }

        public TimeSpan GetTimeDifference()
        {
            return DateTime.UtcNow - Tracker.Time;
        }

        public void SetTimer()
        {
            TimeLeft = Tracker.TimeRemaining;
            PriorTimeLeft = TimeLeft;
            TimeStateChanged = Tracker.Time;
        }

        public async void Timer()
        {
            while (Tracker.IsTournamentRunning)
            {
                while ((TimeLeft > new TimeSpan(0,0,0)) && Tracker.IsTimerRunning) //While timer is running and Timeleft > 0
                {
                    TimeLeft = PriorTimeLeft - (DateTime.UtcNow - TimeStateChanged);
                    UpdateTimerColor();
                    StateHasChanged();
                    await Task.Delay(1000);
                }

                if (TimeLeft <= new TimeSpan()) //If timeleft has expired and the timer is still running (the blindlevel has ended)
                {
                    TimeExpired();
                }
                else
                {
                    await Task.Delay(1000);
                }
                StateHasChanged();
            }
        }

        public void UpdateTimerColor()
        {
            if (TimeLeft < new TimeSpan(0, 0, 31))
            {
                TimerColor = "red";
            }
            else
            {
                TimerColor = "white";
            }
        }

        public async void CheckTracker(object sender, ElapsedEventArgs e)
        {
            //This function queries the server to get new data for the tracker. Is connected to the FiveSecondTimer and is called every 5 seconds. 

            bool oldTimerState = Tracker.IsTimerRunning; //Initial Timer State.

            Tracker = await TrackingDataService.GetTournamentTracking(TrackerId); //Update Tracker

            bool newTimerState = Tracker.IsTimerRunning;

            if (oldTimerState != newTimerState && Tracker != null) //If the timer state has changed
            {
                if (Tracker.IsTimerRunning)//If the timer is running, save the TimeStateChanged
                {
                    TimeStateChanged = Tracker.Time;
                    UpdateBlindLevels();
                }
                else //If the timer stopped, reset it.
                {
                    SetTimer();
                }
            }

            StateHasChanged();
        }

        public void UpdateBlindLevels()
        {
            //update the blind levels if the ID's don't match the tracking Id's
            if ((CurrentBlindLevel.Id != Tracker.CurrentBlindLevel.Id) || (NextBlindLevel.Id != Tracker.NextBlindLevel.Id))
            {
                CurrentBlindLevel = Tracker.CurrentBlindLevel;
                NextBlindLevel = Tracker.NextBlindLevel;
            }
        }

        public void IncrementBlindLevels()
        {
            //Set the blindlevels
            CurrentBlindLevel = NextBlindLevel;
            NextBlindLevel = BlindLevels.FirstOrDefault(x => x.SequenceNum == CurrentBlindLevel.SequenceNum + 1);

            //Update TimeLeft with the CurrentBlindLevel's minutes property.
            TimeLeft = new TimeSpan(0, CurrentBlindLevel.Minutes, 0);
            PriorTimeLeft = TimeLeft;
            TimeStateChanged = DateTime.UtcNow;
            StateHasChanged();
            Timer();
        }


        public async void TimeExpired()
        {
            await PlaySound();
            IncrementBlindLevels();
        }

        public async Task PlaySound()
        {
            await _jsRuntime.InvokeAsync<string>("PlayAudio", "chime");
        }

    }
}
