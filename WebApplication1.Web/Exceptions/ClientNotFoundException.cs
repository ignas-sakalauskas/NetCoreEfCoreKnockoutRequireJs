using System;

namespace WebApplication1.Web.Exceptions
{
    /// <summary>
    /// ClientNotFoundException exception
    /// </summary>
    public class ClientNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Error message</param>
        public ClientNotFoundException(string message)
            : base(message)
        {
        }
    }
}