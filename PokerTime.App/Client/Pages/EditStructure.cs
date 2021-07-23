using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Client.Interfaces;
using PokerTime.App.Client.Services;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Pages
{
    public partial class EditStructure
    {
        //Host (User)
        public Guid HostId { get; set; }
        public User Host { get; set; }

        //TournamentStructure
        [Parameter]
        public int TournamentStructureId { get; set; }
        public TournamentStructure TournamentStructure { get; set; }

        //Blind Levels
        public List<BlindLevel> BlindLevels { get; set; }

        //Services
        [Inject]
        public IUserDataService UserDataService { get; set; }
        [Inject]
        public IStructureDataService StructureDataService { get; set; }
        [Inject]
        public IBlindLevelDataService BlindLevelDataService { get; set; }
        [Inject]
        public ILogger<UsersOverview> Logger { get; set; }
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

                Host = await UserDataService.GetUser(Guid.Parse("fe578a4f-6b3e-49d6-b8ee-53b23ae61757"));
                HostId = Host.Id;
                if(TournamentStructureId == 0)
                {
                    //Create new Structure & Blind Levels
                    await AddNewTournamentStructure();
                    await AddBlindLevel(TournamentStructureId, 25, 50, 0, 30);
                    await AddBlindLevel(TournamentStructureId, 50, 100, 0, 30);
                    await AddBlindLevel(TournamentStructureId, 100, 200, 0, 30);
                }
                else
                {
                    //Get existing Structure
                    TournamentStructure = await StructureDataService.GetStructure(TournamentStructureId, HostId);
                    BlindLevels = (await BlindLevelDataService.GetBlindLevels(TournamentStructureId, HostId)).ToList();
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
            TournamentStructure = await StructureDataService.AddStructure(new TournamentStructure
            {
                Name = "",
                DateCreated = DateTime.Today,
                NumberOfEvents = 0,
                HostId = HostId,
            });
            TournamentStructureId = TournamentStructure.Id;
        }

        public async Task AddBlindLevel(int structureId, int smallBlind = 0, int bigBlind = 0, int ante = 0, int minutes = 0)
        {
            var newBlindLevel = new BlindLevel
            {
                TournamentStructureId = structureId,
                SmallBlind = smallBlind == 0 ? 0 : smallBlind,
                BigBlind = bigBlind == 0 ? 0 : bigBlind,
                Ante = ante == 0 ? 0 : ante,
                Minutes = minutes == 0 ? 0 : minutes
            };
            BlindLevels.Add(await BlindLevelDataService.AddBlindLevel(newBlindLevel, HostId));
        }

    }
}
