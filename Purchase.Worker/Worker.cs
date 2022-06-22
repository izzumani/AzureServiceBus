using EventBusServiceBus.Abstractions;
using EventBusServiceBus.EventDTOs;
using EventBusServiceBus.Events;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;
using Purchase.Application.Commands;
using Purchase.Application.DTO.Purchase;
using Purchase.Application.EventHandlers;
using System.Threading;
// checking
namespace Purchase.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private TelemetryClient _telemetryClient;
        private static HttpClient httpClient = new HttpClient();
        private static System.Timers.Timer BRTimer = null;
        private static int _serviceTimer = default(int);
        private static bool isRunningProcessEvent = false;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;
        Semaphore semaphoreObject;
        public Worker(ILogger<Worker> logger, TelemetryClient telemetryClient, IMediator mediator, IEventBus eventBus)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
            _serviceTimer = 1000;
            semaphoreObject = new Semaphore(initialCount: 1, maximumCount: 1);
            _mediator = mediator;
            _eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            //    await Task.Delay(1000000, stoppingToken);
            //}

            try
            {

                _logger.LogInformation("Service started");
                BRTimer = new System.Timers.Timer(_serviceTimer);
                BRTimer.Elapsed += new System.Timers.ElapsedEventHandler(BRTimerProcessRequest);
                BRTimer.Enabled = true;
                _logger.LogInformation($"Service are being traced after every {_serviceTimer.ToString()}  milli seconds");
                
                var tcs = new TaskCompletionSource<bool>();
                stoppingToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
                _logger.LogInformation($"Subscribe to the Airtime Purchase Intergration EVENT");
                //_eventBus.Subscribe<AirtimePurchaseIntegrationEvent, AirtimePurchaseIntergrationEventHandler>();
                 _eventBus.SubscribeDynamic<AirtimePurchaseIntergrationEventHandler>("airtime-purchase");
                await tcs.Task;

                _eventBus.Unsubscribe<AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>, IIntegrationEventHandler<AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>>>();
                _logger.LogInformation("Service stopped");
            }
            catch (Exception e)
            {

                _logger.LogInformation($"Error: ${e.Message.ToString()}");
                _eventBus.Unsubscribe<AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>, IIntegrationEventHandler<AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>>>();
            }
        }

        private async void BRTimerProcessRequest(object source, System.Timers.ElapsedEventArgs e)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            try
            {
            
                semaphoreObject.WaitOne();

                _logger.LogInformation("Executing the thread");
                
                

                var airtimePurchaseDTO = new AirtimePurchaseDTO()
                {
                    TransactionId = Guid.NewGuid(),
                    MobileNumber = "+27825555555",
                    MobileNetwork = "Vodacom",
                    TraderId = "1234",
                    TransactionAmount = 1000,
                    ProductId = 1,
                    CorrelationId = Guid.NewGuid(),
                    ParentId = null,
                    CreatedDateTime = DateTime.UtcNow


                };

                AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO> integrationEvent = new AirtimePurchaseIntegrationEvent<AirtimePurchaseDTO>(airtimePurchaseDTO);

                

                await _mediator.Publish((INotification)Activator.CreateInstance(
               typeof(IntegrationEventEventNotification<>).MakeGenericType(integrationEvent.GetType()), integrationEvent));
                

                semaphoreObject.Release();


            }
            catch (Exception ex)
            {

                _logger.LogInformation($"Error: ${ex.Message.ToString()}");
                isRunningProcessEvent = false;
                semaphoreObject.Release();
            }
        }
     }


}