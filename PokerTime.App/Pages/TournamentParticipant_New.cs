using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTime.App.Pages
{
    public class TournamentParticipant_New
    {
        private bool _isTimerRunning = true;

        public void OnLoad()
        {
            // Check if tournament has started
            if (TournamentIsRunning())
            {
                // Kick off checker on a background thread
                Checker();


                // Kick off timer
                // Get time first 
                //var currentTime = GetCurrentTime();
                Timer();
                

            }
                
            
        }

        public bool TournamentIsRunning()
        {
            return true;
        }

        public void Checker()
        {
            while (TournamentIsRunning())
            {
                // Go out and check if the host's timer is still running, if not toggle flag
                //if (!HostTimerIsRunning())
                //{
                //    _isTimerRunning = false;
                //}

                // Wait 5 seconds 
                Task.Delay(5000);
            }
        }

        public void Timer()
        {
            while(_isTimerRunning)
            {
                // Do countdown
            }
        }
    }
}
