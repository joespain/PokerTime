using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PokerTime.App.Client.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Pages
{
    public partial class HostProfile 
    {
        public Guid HostId { get; set; }
        public Host Host { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        [Inject]
        public IHostDataService HostDataService { get; set; }

        [Inject]
        public ILogger<UsersOverview> Logger { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //set the current host.
                Host = await HostDataService.GetHost(Guid.Parse("8c13e4c0-43d8-4e44-855b-0d6683cac1aa"));
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
                await HostDataService.UpdateHost(Host);
                StatusClass = "alert-success";
                Message = "User updated successfully.";
                Saved = true;
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are validation errors. Please try again.";
        }

        protected async Task DeleteHost()
        {
            await HostDataService.DeleteHost(Host.Id);

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
