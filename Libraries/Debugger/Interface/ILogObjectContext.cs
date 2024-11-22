namespace Debugger
{
    /// <summary>
    /// Defines an interface for providing context information related to logging.
    /// </summary>
    /// <typeparam name="T">The type of the context object. This could be any type that provides relevant information for logging.</typeparam>
    public interface ILogObjectContext<T>
    {
        /// <summary>
        /// Gets the name of the context. This can be used to identify or describe the context in log entries.
        /// </summary>
        /// <value>The name of the context</value>
        /// <example>The logging class name.</example>
        string ContextName { get; }

        /// <summary>
        /// Gets the context object associated with the log entry. This object provides additional information relevant to logging.
        /// </summary>
        /// <value>The context object.</value>
        /// <example>A unity game object.</example>
        T Context { get; }
    }
}
