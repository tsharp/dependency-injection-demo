namespace WebApp.Extensibility.Initialization
{
    public interface IAsyncInitializable
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}
