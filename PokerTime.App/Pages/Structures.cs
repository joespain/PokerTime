using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public partial class Structures : ComponentBase
    {
        //public Guid HostId { get; set; }
        //public Host Host { get; set; }
        public List<TournamentStructure> TournamentStructures { get; set; }
        [Parameter]
        public EventCallback<int> OnButtonClick { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        //[Inject]
        //public IHostDataService UserDataService { get; set; }

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
                //Host = await UserDataService.GetHost();
                //HostId = Host.Id;

                Saved = false;
                
                TournamentStructures = (await StructureDataService.GetStructures()).ToList();

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
            NavigationManager.NavigateTo($"structures/0");
        }

        private void EditStructure(int structureId)
        {
            NavigationManager.NavigateTo($"structures/{structureId}");
        }

        protected void Delete(int structureId)
        {
            StructureDataService.DeleteStructure(structureId);
        }
        

    }
}
