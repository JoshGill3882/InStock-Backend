namespace instock_server_application.Businesses.Services; 

// https://stacksecrets.com/dot-net-core/scheduled-repeating-task-with-net-core#Introduction
public class ExternalShopPollingService : IHostedService {
    private IBusinessConnectionService _businessConnectionService;
    private Timer? _syncTimer;

    public ExternalShopPollingService(IServiceScopeFactory scopeFactory) {
        using var scope = scopeFactory.CreateScope();
        _businessConnectionService = scope.ServiceProvider.GetRequiredService<IBusinessConnectionService>();
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _syncTimer = new Timer(
            _businessConnectionService.SyncAllBusinessesItemsToConnections,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(1000));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _syncTimer?.Change(Timeout.Infinite, 0);
        
        return Task.CompletedTask;
    }
}