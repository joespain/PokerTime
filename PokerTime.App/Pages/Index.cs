using Microsoft.AspNetCore.Components;
using PokerTime.App.Interfaces;
using PokerTime.App.Interfaces;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
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
