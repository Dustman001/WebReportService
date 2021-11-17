using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReportServiceWeb02.DbModels;
using ReportServiceWeb02.Hubs;
using ReportServiceWeb02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public class BackTask : IHostedService, IDisposable
    {
        private readonly IHubContext<DataHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BackTask> _logger;
        private readonly StatusTask _statusTask;

        private Timer timerRepD, timerRepE, timerRepR, timerMailer;
        private int numberRepD, numberRepR, numberRepE;

        private List<Mailer> _mailers = new List<Mailer>();
        private List<string> _messages = new List<string>();
        public string RepD { get; set; }

        public string RepE { get; set; }

        public string RepR { get; set; }

        public BackTask(IServiceScopeFactory scopeFactory, 
                        ILogger<BackTask> logger, 
                        StatusTask statusTask, 
                        IHubContext<DataHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _statusTask = statusTask;
            _hubContext = hubContext;
        }

        public void Dispose()
        {
            timerRepD?.Dispose();
            timerRepE?.Dispose();
            timerRepR?.Dispose();
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timerRepD = new Timer(o =>
            {
                var status = _statusTask.Reportes.Where(w => w.Ids == "RepD").FirstOrDefault().Status;

                if (status == "Habilitado")
                {
                    Interlocked.Increment(ref numberRepD);
                    _messages.Add($"Printing the Reporte Diario worker number {numberRepD}");
                }
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1));

            timerRepE = new Timer(o =>
            {
                if (timerRepE.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan) == true)
                {
                    var status = _statusTask.Reportes.Where(w => w.Ids == "RepE").FirstOrDefault().Status;

                    if (status == "Habilitado")
                    {
                        Interlocked.Increment(ref numberRepE);
                        _messages.Add($"Printing the Reporte EDAC worker number {numberRepE}");
                    }
                }

                timerRepE.Change(TimeSpan.FromSeconds(45), TimeSpan.FromSeconds(45));
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

            timerRepR = new Timer(o =>
            {
                var status = _statusTask.Reportes.Where(w => w.Ids == "RepR").FirstOrDefault().Status;

                if (status == "Habilitado")
                {
                    Interlocked.Increment(ref numberRepR);
                    _messages.Add($"Printing the Reporte RTU worker number {numberRepR}");
                }
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(10));

            timerMailer = new Timer(async o =>
            {
                await PrintDataAsync();
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async Task PrintDataAsync()
        {
            var dats = new {
                status = 200,
                reports = _statusTask.Reportes,
                message = _messages
            };

            string data = JsonSerializer.Serialize(dats);
            _messages.Clear();

            await _hubContext.Clients.All.SendAsync("ReceiveData", data);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
