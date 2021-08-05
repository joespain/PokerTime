using Microsoft.AspNetCore.Components;
using PokerTime.App.Interfaces;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public partial class Index
    {
        [Inject]
        public IHostDataService HostDataService { get; set; }



        protected override async Task OnInitializedAsync()
        {

        }

    }
}
