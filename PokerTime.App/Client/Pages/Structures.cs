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
    public partial class Structures
    {
        public Guid HostId { get; set; }
        public User Host { get; set; }
        public List<TournamentStructure> TournamentStructures { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public IStructureDataService StructureDataService { get; set; }

        [Inject]
        public ILogger<Structures> Logger { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {

                //Set the current host
                Host = await UserDataService.GetUser(Guid.Parse("fe578a4f-6b3e-49d6-b8ee-53b23ae61757"));
                HostId = Host.Id;

                Saved = false;
                
                TournamentStructures = (await StructureDataService.GetStructures(HostId)).ToList();

                if(TournamentStructures == null)
                {
                    TournamentStructures = new List<TournamentStructure>();
                }
                

            }
            catch (Exception e)
            {
                Logger.LogDebug(e, e.Message);
            }
        }

        protected void NewStructure()
        {
            NavigationManager.NavigateTo($"structures/");
        }

        protected void Edit(int structureId)
        {

            NavigationManager.NavigateTo($"structures/{structureId}");
        }

        protected void Delete(int structureId)
        {
            StructureDataService.DeleteStructure(structureId, HostId);
        }

    }
}
