using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Client.Interfaces;
using PokerTime.Shared.Entities;
using PokerTime.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Pages
{
    public partial class UsersOverview 
    {
        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public ILogger<UsersOverview> Logger { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<User> AllUsers { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                AllUsers = (await UserDataService.GetAllUsers()).ToList();
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
