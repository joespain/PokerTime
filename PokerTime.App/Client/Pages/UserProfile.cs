﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Client.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Pages
{
    public partial class UserProfile
    {
        public Guid HostId { get; set; }
        public User Host { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public ILogger<UsersOverview> Logger { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //set the current host.
                Host = await UserDataService.GetUser(Guid.Parse("fe578a4f-6b3e-49d6-b8ee-53b23ae61757"));
                HostId = Host.Id;

                Saved = false;
                
            }
            catch (Exception e)
            {
                Logger.LogDebug(e, e.Message);
            }
        }

        protected async Task HandleValidSubmit()
        {
                await UserDataService.UpdateUser(Host);
                StatusClass = "alert-success";
                Message = "User updated successfully.";
                Saved = true;
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are validation errors. Please try again.";
        }

        protected async Task DeleteUser()
        {
            await UserDataService.DeleteUser(Host.Id);

            StatusClass = "alert-success";
            Message = "User deleted successfully.";
            Saved = true;
        }
        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/users");
        }
    }
}
