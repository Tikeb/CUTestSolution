using System.Collections.Generic;
using System.Net;
using UI.Interfaces.Models;

namespace UI.Models
{
    /// <summary>
    /// APICallResult represents the result of a call to the API.
    /// </summary>
    /// <typeparam name="T">Represents the type of object that will result from de-serializing the HttpResponse.</typeparam>
    public class APICallResult<T> : IAPICallResult<T>
    {
        /// <inheritdoc/>
        public bool IsSuccessStatusCode { get; set; }

        /// <inheritdoc/>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <inheritdoc/>
        public List<string> LocationSegments { get; set; }

        /// <inheritdoc/>
        public T ResultObject { get; set; }

        /// <inheritdoc/>
        public string Message { get; set; }
    }
}