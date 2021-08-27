using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PokerTime.App.Services
{
    public class TimerService : BackgroundService
    {
        public static event Func<Task> UpdateEvent;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000);
                await UpdateEvent?.Invoke();
            }
        }
    }

}
