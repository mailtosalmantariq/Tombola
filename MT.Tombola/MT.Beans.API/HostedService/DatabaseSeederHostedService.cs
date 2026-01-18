
namespace MT.Beans.API.HostedService
{
    public class DatabaseSeederHostedService : IHostedService
    {
        private readonly IServiceProvider _services;

        public DatabaseSeederHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            await SeedData.SeedAsync(scope.ServiceProvider);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
