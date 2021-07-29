using Microsoft.AspNetCore.Components;
using PokerTime.App.Client.Interfaces;
using System.Threading.Tasks;

namespace PokerTime.App.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public IHostDataService HostDataService { get; set; }

        //public List<Host> Users { get; set; }


        protected override async Task OnInitializedAsync()
        {
            //Users = (await UserDataService.GetAllUsers()).ToList();
        }

    }
}
