using Microsoft.AspNetCore.Components;
using PokerTime.App.Interfaces;

namespace PokerTime.App.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public IHostDataService HostDataService { get; set; }


    }
}
