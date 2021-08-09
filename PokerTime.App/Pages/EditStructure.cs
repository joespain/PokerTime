using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;
using System;
using System.Collections.Generic;
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
        public TournamentStructureModel TSModel { get; set; } = new TournamentStructureModel();


        ////Blind Levels
        //public List<BlindLevelModel> BlindLevels { get; set; } = new List<BlindLevelModel>();

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
        [Inject]
        public IMapper Mapper { get; set; }

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

                    TSModel.HostId = HostId;
                    TSModel.DateCreated = DateTime.Today;

                    TSModel.BlindLevels.Add(new BlindLevelModel());
                }
                else
                {
                    //Get existing Structure & Blind Levels
                    var existingStructure = await StructureDataService.GetStructure(TournamentStructureId);
                    TSModel = Mapper.Map<TournamentStructureModel>(existingStructure);

                    if(TSModel.BlindLevels == null)
                    {
                        //Add an initial blind level if the structure is new.
                        TSModel.BlindLevels.Add(new BlindLevelModel());
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
            //Adds a new tournament structure

            TSModel = Mapper.Map<TournamentStructureModel>(
                await StructureDataService.AddStructure(
                    Mapper.Map<TournamentStructure>(TSModel)));

            TournamentStructureId = TSModel.Id;
        }

        public void AddBlindLevel(BlindLevelModel blindLevel)
        {
            TSModel.BlindLevels.Insert(TSModel.BlindLevels.IndexOf(blindLevel)+1, new BlindLevelModel());
        }

        public void AddBlindLevel()
        {
            TSModel.BlindLevels.Add(new BlindLevelModel());
        }

        public async Task DeleteStructure()
        {
            if (TournamentStructureId != 0)
            {
                await StructureDataService.DeleteStructure(TournamentStructureId);
            }
            NavigateToStructures();
        }

        public async Task HandleValidSubmit()
        {

            if (TSModel.Id == 0)
            {
                //New TournamentStructure
                int sequenceNum = 1;
                foreach (var blindLevel in TSModel.BlindLevels)
                {
                    blindLevel.SequenceNum = sequenceNum;
                    sequenceNum++;
                }
                //Convert from the Model to
                TSModel = Mapper.Map<TournamentStructureModel>(
                    await StructureDataService.AddStructure(
                        Mapper.Map<TournamentStructure>(TSModel)));
            }
            else
            {
                //Update existing TournamentStructure
                int sequenceNum = 1;
                foreach (var blindLevel in TSModel.BlindLevels)
                {
                    blindLevel.TournamentStructureId = TSModel.Id;
                    blindLevel.SequenceNum = sequenceNum;
                    sequenceNum++;
                }
                await StructureDataService.UpdateStructure(
                    Mapper.Map<TournamentStructure>(TSModel));
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

        public void DeleteBlindLevel(BlindLevelModel blindLevel)
        {
            TSModel.BlindLevels.Remove(blindLevel);
        }

    }
}
