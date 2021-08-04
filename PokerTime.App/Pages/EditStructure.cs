using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Interfaces;
using PokerTime.App.Services;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<BlindLevel> BlindLevels { get; set; } = new List<BlindLevel>();

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

                Host = await UserDataService.GetHost();
                HostId = Host.Id;
                if(TournamentStructureId == 0)
                {
                    //Create new Structure & Blind Levels
                    //await AddNewTournamentStructure();


                    TournamentStructure.HostId = HostId;
                    TournamentStructure.DateCreated = DateTime.Today;

                    //TournamentStructureId = TournamentStructure.Id;

                    BlindLevels.Add(new BlindLevel());
                }
                else
                {
                    //Get existing Structure & Blind Levels
                    TournamentStructure = await StructureDataService.GetStructure(TournamentStructureId);

                    if(TournamentStructure.BlindLevels == null)
                    {
                        BlindLevels.Add(new BlindLevel());
                    }
                    else
                    {
                        BlindLevels = (List<BlindLevel>)TournamentStructure.BlindLevels;

                    }

                }
                
            }
            catch(Exception e)
            {
                Logger.LogDebug(e.Message);
            }
        }

        public async Task AddTournamentStructure()
        {
            //Adds a blank tournament structure
            TournamentStructure = await StructureDataService.AddStructure(TournamentStructure);

            TournamentStructureId = TournamentStructure.Id;
        }

        public void AddBlindLevel(BlindLevel blindLevel)
        {
            BlindLevels.Insert(BlindLevels.IndexOf(blindLevel)+1, new BlindLevel());
        }

        public async Task DeleteStructure()
        {
            if (TournamentStructureId != 0)
            {
                await StructureDataService.DeleteStructure(TournamentStructureId);
            }
        }

        public async Task HandleValidSubmit()
        {
            //Add the SequenceNumbers for the blindlevels to ensure they are saved in order
            int sequenceNum = 1;
            foreach (var blindLevel in BlindLevels)
            {
                blindLevel.SequenceNum = sequenceNum;
                sequenceNum++;
            }
            TournamentStructure.BlindLevels = BlindLevels;

            if (TournamentStructure.Id == 0)
            {
                TournamentStructure = await StructureDataService.AddStructure(TournamentStructure);
            }
            else
            {
                await StructureDataService.UpdateStructure(TournamentStructure);
            }
            //Go back to the structures overview screen
            NavigateToStructures();
        }

        public async Task HandleInvalidSubmit()
        {

        }

        public void NavigateToStructures()
        {
            NavigationManager.NavigateTo("/structures");
        }

        public void DeleteBlindLevel(BlindLevel blindLevel)
        {
            BlindLevels.Remove(blindLevel);
        }

    }
}
