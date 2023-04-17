namespace instock_server_application.Businesses.Services; 

// https://stacksecrets.com/dot-net-core/scheduled-repeating-task-with-net-core#Introduction
public class ExternalShopPollingService : IHostedService {
    private IBusinessConnectionService _businessConnectionService;
    private Timer? _syncOrderstimer;
    private Timer? _syncStocktimer;

    public ExternalShopPollingService(IServiceScopeFactory scopeFactory) {
        using var scope = scopeFactory.CreateScope();
        _businessConnectionService = scope.ServiceProvider.GetRequiredService<IBusinessConnectionService>();
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _syncOrderstimer = new Timer(
            _businessConnectionService.SyncAllBusinessesItemOrders,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(1000));

        _syncStocktimer = new Timer(
            _businessConnectionService.SyncAllBusinessesItemStock,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(1000));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _syncOrderstimer?.Change(Timeout.Infinite, 0);
        _syncStocktimer?.Change(Timeout.Infinite, 0);
        
        return Task.CompletedTask;
    }
}