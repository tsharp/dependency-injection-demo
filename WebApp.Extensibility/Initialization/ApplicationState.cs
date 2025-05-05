namespace WebApp.Extensibility.Initialization
{
    public enum ApplicationState
    {
        /// <summary>
        /// The application has not been initialized.
        /// </summary>
        Uninitialized,

        /// <summary>
        /// The application is performing setup operations.
        /// </summary>
        Initializing,

        /// <summary>
        /// The application is fully running and operational.
        /// </summary>
        Running,

        /// <summary>
        /// The application is in a paused or standby state.
        /// </summary>
        Paused,

        /// <summary>
        /// The application is reloading configuration or resources.
        /// </summary>
        Reloading,

        /// <summary>
        /// The application is restarting due to a configuration change or manual trigger.
        /// </summary>
        Restarting,

        /// <summary>
        /// The application is gracefully shutting down.
        /// </summary>
        ShuttingDown,

        /// <summary>
        /// The application has been terminated.
        /// </summary>
        Terminated,

        /// <summary>
        /// The application has entered a faulted state due to a critical error.
        /// </summary>
        Crashed,
    }
}
