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
    public partial class EditStructure : ComponentBase
    {
        //Host
        public Guid HostId { get; set; }
        public Host Host { get; set; }

        //TournamentStructure
        [Parameter]
        public int TournamentStructureId { get; set; }
        public TournamentStructure TournamentStructure { get; set; } = new TournamentStructure();

        //Blind Levels
        public LinkedList<BlindLevel> BlindLevels { get; set; } = new LinkedList<BlindLevel>();

        //Services
        [Inject]
        public IHostDataService UserDataService { get; set; }
        [Inject]
        public IStructureDataService StructureDataService { get; set; }
        [Inject]
        public IBlindLevelDataService BlindLevelDataService { get; set; }
        [Inject]
        public ILogger<EditStructure> Logger { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Saved = false;

                Host = await UserDataService.GetHost(Guid.Parse("48b51074-220e-4275-b3f6-ed41b8319832"));
                HostId = Host.Id;
                if(TournamentStructureId == 0)
                {
                    //Create new Structure & Blind Levels
                    await AddNewTournamentStructure();

                    TournamentStructure.HostId = HostId;
                    TournamentStructure.DateCreated = DateTime.Today;

                    //TournamentStructureId = TournamentStructure.Id;

                    BlindLevels.AddFirst(new BlindLevel());
                }
                else
                {
                    //Get existing Structure & Blind Levels
                    TournamentStructure = await StructureDataService.GetStructure(TournamentStructureId, HostId);

                    if(TournamentStructure.BlindLevels == null)
                    {
                        BlindLevels.AddFirst(new BlindLevel() { TournamentStructureId = TournamentStructureId});
                    }
                    else
                    {
                        BlindLevels = (LinkedList<BlindLevel>)TournamentStructure.BlindLevels;
                    }

                }
                
            }
            catch(Exception e)
            {
                Logger.LogDebug(e.Message);
            }
        }

        public async Task AddNewTournamentStructure()
        {
            //Adds a blank tournament structure
            TournamentStructure = await StructureDataService.AddStructure(new TournamentStructure(), HostId);

            TournamentStructureId = TournamentStructure.Id;
        }

        public void AddBlindLevel(int smallBlind = 0, int bigBlind = 0, int ante = 0, int minutes = 0)
        {
            var newBlindLevel = new BlindLevel()
            {
                TournamentStructureId = TournamentStructureId,
                SmallBlind = smallBlind == 0 ? 0 : smallBlind,
                BigBlind = bigBlind == 0 ? 0 : bigBlind,
                Ante = ante == 0 ? 0 : ante,
                Minutes = minutes == 0 ? 0 : minutes
            };
            BlindLevels.AddLast(newBlindLevel);
        }

        public async Task DeleteStructure()
        {
            if (TournamentStructureId != 0)
            {
                await StructureDataService.DeleteStructure(TournamentStructureId, HostId);
            }
        }

        public async Task HandleValidSubmit()
        {
            TournamentStructure.BlindLevels = BlindLevels;
            await StructureDataService.UpdateStructure(TournamentStructure);
            
        }

        public async Task HandleInvalidSubmit()
        {

        }

        public void NavigateToStructures()
        {
            NavigationManager.NavigateTo("/structures");
        }

    }
}
