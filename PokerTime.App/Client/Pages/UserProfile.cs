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
    public partial class UserProfile
    {
        [Parameter]
        public string UserId { get; set; }
        public User user { get; set; } = new User();

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
                Saved = false;
                int.TryParse(UserId, out var userId);
            
                if (userId == 0)
                {
                    user = new User();
                }
                else
                {
                    user = await UserDataService.GetUser(userId);
                }
            }
            catch (Exception e)
            {
                Logger.LogDebug(e, e.Message);
            }
        }

        protected async Task HandleValidSubmit()
        {
            if(user.Id == 0)
            {
                var addedUser = await UserDataService.AddUser(user);
                if(addedUser != null)
                {
                    StatusClass = "alert-success";
                    Message = "New user added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                await UserDataService.UpdateUser(user);
                StatusClass = "alert-success";
                Message = "User updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are validation errors. Please try again.";
        }

        protected async Task DeleteUser()
        {
            await UserDataService.DeleteUser(user.Id);

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
