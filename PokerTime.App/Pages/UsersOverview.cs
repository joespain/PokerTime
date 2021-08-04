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
    public partial class UsersOverview 
    {
        [Inject]
        public IHostDataService UserDataService { get; set; }

        [Inject]
        public ILogger<UsersOverview> Logger { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<Host> AllUsers { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                //AllUsers = (await UserDataService.GetAllHosts()).ToList();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e, e.Message);
            }
        }

        protected void NavigateToUser(int userId)
        {
            NavigationManager.NavigateTo($"/userprofile/{userId}");
        }
    }
}
