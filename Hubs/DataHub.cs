using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Hubs
{
    public class DataHub: Hub
    {
        public async Task Send(string data)
        {
            await Clients.All.SendAsync("ReceiveData", data);
        }
    }
}
