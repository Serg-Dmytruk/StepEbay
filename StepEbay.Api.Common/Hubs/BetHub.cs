using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Api.Common.Hubs
{
    public class BetHub : Hub
    {
        public BetHub()
        {

        }

        public async Task MyBetClosed(List<int> users)
        {
            //implent logic
        }
    }
}
