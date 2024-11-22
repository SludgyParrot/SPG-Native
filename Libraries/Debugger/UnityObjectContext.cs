using UnityEngine;

namespace Debugger
{
    /// <summary>
    /// Represents a context object in the Unity environment, providing
    /// information about the context and its associated name.
    /// </summary>
    public class UnityObjectContext : ILogObjectContext<Object>
    {
        /// <summary>
        /// Gets the name of the context, which is the name of the reference class.
        /// </summary>
        public string ContextName { get; }

        /// <summary>
        /// Gets the context object associated with this context.
        /// </summary>
        public Object Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityObjectContext"/> class.
        /// </summary>
        /// <param name="classReference">
        /// The reference class used to determine the context name. Cannot be <c>null</c>.
        /// </param>
        /// <param name="context">
        /// The context object to be associated with this instance. Cannot be <c>null</c>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="classReference"/> or <paramref name="context"/> is <c>null</c>.
        /// </exception>
        public UnityObjectContext(object classReference, Object context)
        {
            if(classReference == null)
            {
                throw new System.ArgumentNullException($"{nameof(classReference)} cannot be null.");
            }

            if (context == null)
            {
                throw new System.ArgumentNullException($"{nameof(context)} cannot be null.");
            }

            ContextName = classReference.ToString();
            Context = context;
        }
    }
}
