using BlazorStyled;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
    public partial class Tournament : ComponentBase, IDisposable
    {

        //TournamentStructure
        public TournamentStructure Structure { get; set; } = new();

        //BlindLevels
        public List<BlindLevel> BlindLevels { get; set; } = new();
        public BlindLevel CurrentBlindLevel { get; set; } = new();
        public BlindLevel NextBlindLevel { get; set; } = new();

        //Event
        [Parameter]
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        //Timer
        public Timer TournamentTimer { get; set; }
        public TimeSpan TimeLeft { get; set; } = new();
        public Timer UpdateTrackerTimer { get; set; }
        public string TimerStyle { get; set; }
        public string TimerColor { get; set; } = "White";
        public bool IsTimerRunning { get; set; } = false;
        public bool IsTournamentRunning { get; set; } = true;
        public string ButtonName { get; set; } = "Start";
        //PriorTimeLeft is being used to keep track of the time left whenever the timer is stopped.
        //It is used to calculate the current time left.
        public TimeSpan PriorTimeLeft { get; set; } = new();
        public DateTime TimeStateChanged { get; set; } = new();

        public string Message { get; set; } = "";

        //TournamentTracking
        public TournamentTracking Tracker { get; set; } = new();
        public Timer FiveSecondTimer { get; set; } = new();

        //User
        public bool IsUserAuthenticated { get; set; } 

        //Services
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public ITournamentTrackingDataService TrackingDataService { get; set; }
        [Inject]
        public IEventDataService EventDataService { get; set; }
        [Inject]
        public IStructureDataService StructureDataService { get; set; }
        [Inject]
        public IBlindLevelDataService BlindLevelDataService { get; set; }
        [Inject]
        public ITournamentEventDataService TournamentEventDataService { get; set; }
        [Inject]
        public ILogger<Tournament> Logger { get; set; }
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
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                //var userId = user.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier);

                if (EventId == new Guid()) //Must have EventId present in address bar
                {
                    throw new Exception("No event ID");
                }

                if (user.Identity.IsAuthenticated) //For Host users
                {
                    //Check to see if Tracking already exists.
                    Tracker = await TournamentEventDataService.GetTournamentTracking(EventId);

                    if (Tracker.Id != new Guid()) //If tracker already exists
                    {
                        if (!Tracker.IsTournamentRunning)
                        {
                            Message = "Tournament has concluded.";
                        }
                        else if (GetTimeDifference() > new TimeSpan(12, 0, 0)) //If more than 12 hours have passed, tournament's over
                        {
                            Message = "Tournament has concluded.";
                        }
                        else if (!Tracker.IsTimerRunning)
                        {
                            Event = await EventDataService.GetEvent(EventId);
                            if (Event is null)
                            {
                                //error
                                throw new Exception("Event is null");
                            }

                            Structure = await StructureDataService.GetStructure(Event.TournamentStructureId);
                            if (Structure is null)
                            {
                                //error
                                throw new Exception("Structure is null");
                            }

                            BlindLevels = Structure.BlindLevels.OrderBy(x => x.SequenceNum).ToList();
                            if (BlindLevels is null)
                            {
                                //error
                                throw new Exception("BlindLevels are null");
                            }
                            SetDataFromTracker();
                        }
                        else
                        {
                            Event = await EventDataService.GetEvent(EventId);
                            if (Event is null)
                            {
                                //error
                                throw new Exception("Event is null");
                            }

                            Structure = await StructureDataService.GetStructure(Event.TournamentStructureId);
                            if (Structure is null)
                            {
                                //error
                                throw new Exception("Structure is null");
                            }

                            BlindLevels = Structure.BlindLevels.OrderBy(x => x.SequenceNum).ToList();
                            if (BlindLevels is null)
                            {
                                //error
                                throw new Exception("BlindLevels are null");
                            }
                            SetDataFromTracker();
                            CalculateCurrentBlindLevel();
                        }
                        StateHasChanged();
                    }
                    else
                    {
                        Event = await EventDataService.GetEvent(EventId);
                        if (Event is null)
                        {
                            //error
                            throw new Exception("Event is null");
                        }

                        Structure = await StructureDataService.GetStructure(Event.TournamentStructureId);
                        if (Structure is null)
                        {
                            //error
                            throw new Exception("Structure is null");
                        }
                        else
                        {
                            //Increment the play count for the structure.
                            await StructureDataService.IncrementStructurePlayCount(Structure);
                        }

                        BlindLevels = (await BlindLevelDataService.GetBlindLevels(Structure.Id)).ToList();
                        if (BlindLevels is null)
                        {
                            //error
                            throw new Exception("BlindLevels are null");
                        }

                        CurrentBlindLevel = BlindLevels.First();
                        NextBlindLevel = BlindLevels.FirstOrDefault(x => x.SequenceNum == CurrentBlindLevel.SequenceNum + 1);
                        ButtonName = "Start";
                        TimerColor = "White";
                        SetTimer();
                        UpdateTournamentTracker();
                    }
                }
                else //For Invitees
                {
                    Tracker = await TrackingDataService.GetTournamentTracking(EventId);

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
                    Structure = await TrackingDataService.GetTournamentStructure(EventId, Tracker.CurrentBlindLevel.TournamentStructureId);
                    if (Structure is null)
                    {
                        throw new Exception("Error loading Tournament Structure.");
                    }

                    BlindLevels = Structure.BlindLevels.OrderBy(x => x.SequenceNum).ToList();
                    
                    CurrentBlindLevel = Tracker.CurrentBlindLevel;
                    NextBlindLevel = Tracker.NextBlindLevel;
                    TimeLeft = Tracker.TimeRemaining;
                    PriorTimeLeft = TimeLeft;
                    TimeStateChanged = Tracker.Time;
                    
                    if (Tracker.IsTimerRunning)
                    {
                        CalculateCurrentBlindLevel();
                    }

                    //This timer will go off every 5 seconds to call the CheckTracker function.
                    FiveSecondTimer = new Timer(5000);
                    FiveSecondTimer.Elapsed += new ElapsedEventHandler(CheckTracker);
                    FiveSecondTimer.Start();
                }

                //This timer updates TimeLeft if the Timer is running
                TournamentTimer = new Timer(1000);
                TournamentTimer.Elapsed += new ElapsedEventHandler(StartTournamentTimer);
                TournamentTimer.Start();

                //Start AuthenticationStateChanged task
                Task<AuthenticationState> currentAuthenticationStateTask;
                AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
                currentAuthenticationStateTask = AuthenticationStateProvider.GetAuthenticationStateAsync();
                OnAuthenticationStateChanged(currentAuthenticationStateTask);

            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
        }

        private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
        {
            InvokeAsync(async () => 
            {
                var authState = await authenticationStateTask;
                var user = authState.User;

                IsUserAuthenticated = user.Identity?.IsAuthenticated ?? false;
                StateHasChanged();
            });
        }

        public void SetDataFromTracker()
        {
            TimeLeft = Tracker.TimeRemaining;
            CurrentBlindLevel = Tracker.CurrentBlindLevel;
            NextBlindLevel = Tracker.NextBlindLevel;
            PriorTimeLeft = TimeLeft;
        }

        public async void GetEventDataFromServer()
        {
            Event = await EventDataService.GetEvent(EventId);
            if (Event is null)
            {
                //error
                Logger.LogError("Event is null");
            }

            Structure = await StructureDataService.GetStructure(Event.TournamentStructureId);
            if (Structure is null)
            {
                //error
                Logger.LogError("Structure is null");
            }

            BlindLevels = Structure.BlindLevels.OrderBy(x => x.SequenceNum).ToList();
            if (BlindLevels is null)
            {
                //error
                Logger.LogError("BlindLevels are null");
            }
        }

        public void SetTimer()
        {
            TimeLeft = new TimeSpan(0, CurrentBlindLevel.Minutes, 0);
            PriorTimeLeft = TimeLeft;
        }

        public void CalculateCurrentBlindLevel()
        {
            TimeSpan timeSinceLastAction = GetTimeDifference(); //Get the amount of time we need to account for

            if(timeSinceLastAction < TimeLeft) //If time is still within the current Blind Level
            {
                TimeLeft -= timeSinceLastAction;
                TimeStateChanged = Tracker.Time;
            }
            else
            {
                timeSinceLastAction -= TimeLeft;
                CurrentBlindLevel = NextBlindLevel;
                NextBlindLevel = BlindLevels.FirstOrDefault(x => x.SequenceNum == CurrentBlindLevel.SequenceNum + 1);

                if(NextBlindLevel is null)
                {
                    CreateNextBlindLevel(CurrentBlindLevel);
                }

                while (timeSinceLastAction > new TimeSpan()) //Loop through the Blind Levels, subtracting the times until we get to the current Blind Level
                {
                    if(new TimeSpan(0, CurrentBlindLevel.Minutes, 0) > timeSinceLastAction)
                    {
                        TimeLeft = new TimeSpan(0, CurrentBlindLevel.Minutes, 0) - timeSinceLastAction;
                        timeSinceLastAction = new TimeSpan();
                    }
                    else
                    {
                        timeSinceLastAction -= new TimeSpan(0, CurrentBlindLevel.Minutes, 0);
                        CurrentBlindLevel = NextBlindLevel;
                        NextBlindLevel = BlindLevels.FirstOrDefault(x => x.SequenceNum == CurrentBlindLevel.SequenceNum + 1);
                    }
                }
                TimeStateChanged = DateTime.UtcNow;
                PriorTimeLeft = TimeLeft;
            }
            

            IsTournamentRunning = Tracker.IsTournamentRunning;
            IsTimerRunning = Tracker.IsTimerRunning;
            if (IsTimerRunning)
            {
                ButtonName = "Stop";
            }
            else
            {
                ButtonName = "Start";
            }
        }

        public TimeSpan GetTimeDifference()
        {
            return DateTime.UtcNow - Tracker.Time;
        }

        public async void TimeExpired()
        {
            await PlaySound();
            IncrementBlindLevel();
        }

        public void StartStopTimer()
        {
            IsTimerRunning = !IsTimerRunning;
            if (IsTimerRunning) 
            {
                Tracker.Time = DateTime.UtcNow; //Save the time the timer was started at
                TimeStateChanged = Tracker.Time;
                ButtonName = "Stop";
                UpdateTournamentTracker();
            }
            else //Timer is stopped
            {
                Tracker.Time = DateTime.UtcNow; //Save the time the timer was stopped at
                ButtonName = "Start";
                PriorTimeLeft = TimeLeft;
                UpdateTournamentTracker();
            }
        }

        public void CheckTimerColor()
        {
            if (TimeLeft < new TimeSpan(0, 0, 31))
            {
                TimerColor = "Red";
            }
            else
            {
                TimerColor = "White";
            }
        }

        public void IncrementBlindLevel()
        {
            try
            {
                if (NextBlindLevel is null) //Null test
                {
                    NextBlindLevel = CreateNextBlindLevel(CurrentBlindLevel);
                }
                else
                {
                    CurrentBlindLevel = NextBlindLevel;
                    PriorTimeLeft = new TimeSpan(0, CurrentBlindLevel.Minutes, 0);
                    NextBlindLevel = BlindLevels.FirstOrDefault(x => x.SequenceNum == CurrentBlindLevel.SequenceNum + 1);
                    TimerColor = "white";

                    if (NextBlindLevel is null)
                    {
                        NextBlindLevel = CreateNextBlindLevel(CurrentBlindLevel);
                    }
                }

                if (IsTournamentRunning)
                {
                    TimeLeft = new TimeSpan(0, CurrentBlindLevel.Minutes, 0);
                    PriorTimeLeft = TimeLeft;
                    Tracker.Time = DateTime.UtcNow;
                    TimeStateChanged = Tracker.Time;

                    if (IsUserAuthenticated)
                    {
                        UpdateTournamentTracker();
                    }
                    StateHasChanged();
                }
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
            }
            
        }

        public async Task PlaySound()
        {
            await _jsRuntime.InvokeAsync<string>("PlayAudio", "chime");
        }

        public async void UpdateTournamentTracker()
        {
            Tracker.Id = EventId;
            Tracker.IsTimerRunning = IsTimerRunning;
            Tracker.IsTournamentRunning = IsTournamentRunning;

            if(CurrentBlindLevel.Id == NextBlindLevel.Id) //Make sure the blind levels are not the same
            {
                NextBlindLevel = BlindLevels.FirstOrDefault(x => x.SequenceNum == CurrentBlindLevel.SequenceNum + 1);
            }
            Tracker.CurrentBlindLevel = CurrentBlindLevel;
            Tracker.NextBlindLevel = NextBlindLevel;
            if(TimeLeft < new TimeSpan())
            {
                Tracker.TimeRemaining = new TimeSpan();
            }
            else
            {
                Tracker.TimeRemaining = TimeLeft;
            }
            

            await TournamentEventDataService.UpdateTournamentTracking(Tracker);
        }

        public async void EndTournament()
        {
            IsTournamentRunning = false;
            IsTimerRunning = false;
            await TournamentEventDataService.EndTournamentTracking(Tracker);

            NavigationManager.NavigateTo("/");
        }

        public async void CheckTracker(object sender, ElapsedEventArgs e)
        {
            //This function queries the server to get new data for the tracker. Is connected to the FiveSecondTimer and is called every 5 seconds. 

            bool oldTimerState = Tracker.IsTimerRunning; //Initial Timer State.

            Tracker = await TrackingDataService.GetTournamentTracking(EventId); //Update Tracker

            bool newTimerState = Tracker.IsTimerRunning;

            if (oldTimerState != newTimerState && Tracker != null) //If the timer state has changed
            {
                if (Tracker.IsTimerRunning)//If the timer is running, save the TimeStateChanged
                {
                    TimeStateChanged = Tracker.Time;
                    UpdateBlindLevels();
                    IsTimerRunning = true;
                }
                else //If the timer stopped, reset it.
                {
                    IsTimerRunning = false;
                    TimeLeft = Tracker.TimeRemaining;
                    PriorTimeLeft = TimeLeft;
                    TimeStateChanged = Tracker.Time;
                }
            }

            //Update the TimeStateChanged if the blind levels are the same but the Times are off.
            if(Tracker.Time != TimeStateChanged && Tracker.CurrentBlindLevel.Id == CurrentBlindLevel.Id)
            {
                TimeStateChanged = Tracker.Time;
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

        public BlindLevel CreateNextBlindLevel(BlindLevel level)
        {
            BlindLevel newBlindLevel = new()
            {
                SmallBlind = level.SmallBlind * 2,
                BigBlind = level.BigBlind * 2,
                Ante = level.Ante,
                Minutes = level.Minutes * 2,
                SequenceNum = level.SequenceNum + 1
            };

            return newBlindLevel;
        }

        public void Dispose()
        {
            FiveSecondTimer?.Dispose();
            TournamentTimer?.Dispose();
            //AuthenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }

        public void StartTournamentTimer(object sender, ElapsedEventArgs e)
        {
            if (IsTimerRunning)
            {
                if (TimeLeft > new TimeSpan())
                {
                    TimeLeft = PriorTimeLeft - (DateTime.UtcNow - TimeStateChanged);
                    CheckTimerColor();
                }

                if (TimeLeft <= new TimeSpan())
                {
                    //Timer ended
                    TimeExpired();
                }
            }
            StateHasChanged();
        }
    }
}
