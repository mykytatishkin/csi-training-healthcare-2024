namespace CSI.IBTA.ProcessingService.Interfaces
{
    public interface IScopedProcessingService
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}
