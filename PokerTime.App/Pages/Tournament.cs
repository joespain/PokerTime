using BlazorStyled;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace PokerTime.App.Pages
{
    public partial class Tournament : ComponentBase
    {

        //TournamentStructure
        public TournamentStructure Structure { get; set; } = new TournamentStructure();

        //BlindLevels
        public List<BlindLevel> BlindLevels { get; set; } = new List<BlindLevel>();
        public BlindLevel CurrentBlindLevel { get; set; } = new BlindLevel();
        public BlindLevel NextBlindLevel { get; set; } = new BlindLevel();

        //Event
        [Parameter]
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        //Timer
        public TimeSpan TimeLeft { get; set; } = new TimeSpan();
        public Timer FiveSecondTimer { get; set; }
        public string TimerStyle { get; set; }
        public string TimerColor { get; set; }
        public bool IsTimerRunning { get; set; } = false;
        public bool IsTournamentRunning { get; set; } = true;
        public string ButtonName { get; set; } = "Start";

        //TournamentTracking
        public TournamentTracking TournamentTracker { get; set; } = new TournamentTracking();


        //Services
        [Inject]
        public IEventDataService EventDataService { get; set; }
        [Inject]
        public IStructureDataService StructureDataService { get; set; }
        [Inject]
        public IBlindLevelDataService BlindLevelDataService { get; set; }
        [Inject]
        public ITournamentTrackingDataService TournamentTrackingDataService { get; set; }
        [Inject]
        public ILogger<EditEvent> Logger { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject] 
        public IStyled Styled { get; set; }
        [Inject]
        IJSRuntime _jsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (EventId == new Guid())
                {
                    //error
                    Logger.LogDebug("No event ID");
                }
                Event = await EventDataService.GetEvent(EventId);
                if (Event == null)
                {
                    //error
                    Logger.LogDebug("Event is null");
                }

                Structure = await StructureDataService.GetStructure(Event.TournamentStructureId);
                if (Structure == null)
                {
                    //error
                    Logger.LogDebug("Structure is null");
                }
                else
                {
                    //Increment the play count for the structure.
                    await StructureDataService.IncrementStructurePlayCount(Structure);
                }

                BlindLevels = (await BlindLevelDataService.GetBlindLevels(Structure.Id)).ToList();
                if (BlindLevels == null)
                {
                    //error
                    Logger.LogDebug("BlindLevels are null");
                }

                FiveSecondTimer = new Timer(5000);
                FiveSecondTimer.Elapsed += new ElapsedEventHandler(AutoUpdateTournamentTracker);
                FiveSecondTimer.Start();

                CurrentBlindLevel = BlindLevels.First();
                NextBlindLevel = BlindLevels.ElementAt(BlindLevels.IndexOf(CurrentBlindLevel)+1);
                ButtonName = "Start";
                TimerColor = "White";

                SetTimer();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);
            }
        }

        public void SetTimer()
        {
            TimeLeft = new TimeSpan(0, CurrentBlindLevel.Minutes, 0);
        }

        public async void Timer()
        {
            while ((TimeLeft > new TimeSpan()) && IsTimerRunning)
            {
                await Task.Delay(1000);
                TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));
                if (TimeLeft < new TimeSpan(0, 0, 31))
                {
                    TimerColor = "Red";
                }
                StateHasChanged();
            }

            if (TimeLeft == new TimeSpan())
            {
                //Time Ended
                TimeExpired();
            }
            else
            {
                //Host stopped timer
                UpdateTournamentTracker();
            }
            StateHasChanged();
        }

        public async void TimeExpired()
        {
            await PlaySound();
            IterateBlindLevel();
        }

        public void StartStopTimer()
        {
            IsTimerRunning = !IsTimerRunning;
            if (IsTimerRunning)
            {
                TournamentTracker.Time = DateTime.UtcNow;
                ButtonName = "Stop";
            }
            else
            {
                TournamentTracker.Time = DateTime.UtcNow;
                ButtonName = "Start";
            }
            Timer();
        }


        public void IterateBlindLevel()
        {
            if (NextBlindLevel == new BlindLevel())
            {
                //End of Tournament
                IsTournamentRunning = false;
                UpdateTournamentTracker();
            }
            else
            {
                CurrentBlindLevel = NextBlindLevel;
                NextBlindLevel = BlindLevels.ElementAt(BlindLevels.IndexOf(CurrentBlindLevel) + 1);
                if (NextBlindLevel == null)
                {
                    NextBlindLevel = new BlindLevel();
                }
                TimerColor = "white";
            }

            if (IsTournamentRunning)
            {
                SetTimer();
                TournamentTracker.Time = DateTime.UtcNow;
                Timer();
                UpdateTournamentTracker();
                StateHasChanged();
            }
        }

        public async Task PlaySound()
        {
            await _jsRuntime.InvokeAsync<string>("PlayAudio", "chime");
        }
        public async void AutoUpdateTournamentTracker(object sender, ElapsedEventArgs e)
        {
            TournamentTracker.Id = Event.Id;
            TournamentTracker.IsTimerRunning = IsTimerRunning;
            TournamentTracker.IsTournamentRunning = IsTournamentRunning;
            TournamentTracker.CurrentBlindLevel = CurrentBlindLevel;
            TournamentTracker.NextBlindLevel = NextBlindLevel;
            TournamentTracker.TimeRemaining = TimeLeft;
            
            await TournamentTrackingDataService.UpdateTournamentTracking(TournamentTracker);
        }

        public async void UpdateTournamentTracker()
        {
            TournamentTracker.Id = Event.Id;
            TournamentTracker.IsTimerRunning = IsTimerRunning;
            TournamentTracker.IsTournamentRunning = IsTournamentRunning;
            TournamentTracker.CurrentBlindLevel = CurrentBlindLevel;
            TournamentTracker.NextBlindLevel = NextBlindLevel;
            TournamentTracker.TimeRemaining = TimeLeft;

            await TournamentTrackingDataService.UpdateTournamentTracking(TournamentTracker);
        }
    }
}
