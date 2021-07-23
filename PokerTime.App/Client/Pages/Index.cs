using Microsoft.AspNetCore.Components;
using PokerTime.App.Client.Interfaces;
using PokerTime.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public IUserDataService UserDataService { get; set; }

        public List<User> Users { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Users = (await UserDataService.GetAllUsers()).ToList();
        }

    }
}
