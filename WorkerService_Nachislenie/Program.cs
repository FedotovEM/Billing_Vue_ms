using WorkerService_Nachislenie;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<NachislWorker>();
    })
    .Build();

host.Run();
