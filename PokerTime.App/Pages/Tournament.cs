using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string EventLinkId { get; set; }
        [Parameter]
        public int EventId { get; set; }
        public Event Event { get; set; }

        //Timer
        public TimeSpan TimeLeft { get; set; } = new TimeSpan();
        public bool IsTimerRunning { get; set; } = false;
        public string ButtonName { get; set; } = "Start";


        //Services
        [Inject]
        public IEventDataService EventDataService { get; set; }
        [Inject]
        public IStructureDataService StructureDataService { get; set; }
        [Inject]
        public IBlindLevelDataService BlindLevelDataService { get; set; }
        [Inject]
        public ILogger<EditEvent> Logger { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }




        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (EventId == 0)
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

                BlindLevels = (await BlindLevelDataService.GetBlindLevels(Structure.Id)).ToList();
                if (BlindLevels == null)
                {
                    //error
                    Logger.LogDebug("BlindLevels are null");
                }


                CurrentBlindLevel = BlindLevels.First();
                NextBlindLevel = BlindLevels.ElementAt(BlindLevels.IndexOf(CurrentBlindLevel)+1);
                ButtonName = "Start";

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
            IsTimerRunning = false;
            ButtonName = "Start";
        }


        public async Task Timer()
        {
            while ((TimeLeft > new TimeSpan()) && IsTimerRunning)
            {
                await Task.Delay(1000);
                TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));
                StateHasChanged();
            }

            if(TimeLeft == new TimeSpan())
            {
                //Time Ended
                await TimeExpired();
            }
            else
            {
                //Host stopped timer
            }

            StateHasChanged();
        }

        public async Task TimeExpired()
        {
            await PlaySound();
            IterateBlindLevel();
            await StartStopTimer();
        }

        public async Task StartStopTimer()
        {
            IsTimerRunning = !IsTimerRunning;
            if (IsTimerRunning)
            {
                ButtonName = "Stop";
            }
            else
            {
                ButtonName = "Start";
            }

            await Timer();
        }


        public void IterateBlindLevel()
        {
            if (NextBlindLevel == null)
            {
                //How to handle end of list
            }
            else
            {
                CurrentBlindLevel = NextBlindLevel;
                NextBlindLevel = BlindLevels.ElementAt(BlindLevels.IndexOf(CurrentBlindLevel) + 1);
                if (NextBlindLevel == null)
                {
                    //How to handle having no data in NextBlindLevel
                }
            }
            SetTimer();
            StateHasChanged();
        }

        public async Task PlaySound()
        {
            await _jsRuntime.InvokeAsync<string>("PlayAudio", "chime");
        }




    }
}
