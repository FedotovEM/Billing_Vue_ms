using Microsoft.Extensions.Hosting;
using WorkerService_Nachislenie.Controllers;

namespace WorkerService_Nachislenie
{
    public class NachislWorker : BackgroundService
    {
        private readonly ILogger<NachislWorker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public NachislWorker(ILogger<NachislWorker> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var dayOfMonth = DateTime.Now.Day;
                if (dayOfMonth == 1)
                {
                    var CalcNachislDone = CalcNachisl.CalculateToNorm(_logger);
                    if (CalcNachislDone)
                        _logger.LogInformation("Начисление по нормативу завершено: {time}", DateTimeOffset.Now);
                    else
                        _logger.LogError("Начисление по нормативу завершено с ошибкой: {time}", DateTimeOffset.Now);
                }

                var RecalcNachislDone = CalcNachisl.ReCalculateToNorm(_logger);
                if (RecalcNachislDone)
                    _logger.LogInformation("Перерасчет начисления по нормативу завершен: {time}", DateTimeOffset.Now);
                else
                    _logger.LogError("Перерасчет начисления по нормативу завершен с ошибкой: {time}", DateTimeOffset.Now);

                int minute = 60000;
                int timeout = minute * 1;
                _logger.LogInformation($"Ожидание {timeout / minute} минут: {DateTimeOffset.Now}");
                Thread.Sleep(timeout);
            }
        }
        private void StopWorkerServiceExecution()
        {
            _hostApplicationLifetime.StopApplication();
        }
    }
}