namespace WebApp.Extensibility.Initialization
{
    public interface IInitializationManager
    {
        public void RegisterInitializer<T>()
            where T : IAsyncInitializable;
    }
}
