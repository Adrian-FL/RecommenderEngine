using Microsoft.Extensions.Hosting;

namespace RecommenderUtils.Engine
{
    public class ClusterBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await RecommenderEngine.GenerateClustersAsync();
    }
}
