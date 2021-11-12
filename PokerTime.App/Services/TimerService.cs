using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PokerTime.App.Services
{
    public class TimerService : IHostedService, IDisposable
    {
        private readonly ILogger<TimerService> _logger;
        private Timer _timer;

        public TimerService(ILogger<TimerService> logger)
        {
            _logger = logger;
        }

        private void OnTimer(object state)
        {
            _logger.LogInformation("OnTimer event called.");
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new(OnTimer, cancellationToken, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timer Stopped.");
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;

        }

        public void Dispose()
        {
            _logger.LogInformation("Timer Disposed.");
            _timer?.Dispose();
        }

    }

}
