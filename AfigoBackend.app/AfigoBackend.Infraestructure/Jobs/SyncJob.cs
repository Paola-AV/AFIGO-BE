using AfigoBackend.Aplication.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Jobs
{
    [DisallowConcurrentExecution]
    public class SyncJob : IJob
    {
        private readonly IExternalSyncInterface _syncService;
        private readonly ILogger<SyncJob> _logger;

        public SyncJob(IExternalSyncInterface syncService, ILogger<SyncJob> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("SyncJob iniciado: {time}", DateTimeOffset.Now);

            try
            {
                await _syncService.SyncAllAsync(context.CancellationToken);
                _logger.LogInformation("SyncJob completado exitosamente: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante SyncJob: {time}", DateTimeOffset.Now);
            }
        }
    }
}
