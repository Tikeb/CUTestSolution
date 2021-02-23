using System.Collections.Generic;
using System.Net;

namespace UI.Interfaces.Models
{
    /// <summary>
    /// IAPICallResult interface.
    /// </summary>
    /// <typeparam name="T">Represents the type of object that will result from de-serializing the HttpResponse.</typeparam>
    public interface IAPICallResult<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the API call was successful or not.
        /// </summary>
        bool IsSuccessStatusCode { get; set; }

        /// <summary>
        /// Gets or sets this field.
        /// </summary>
        HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets this field.
        /// </summary>
        List<string> LocationSegments { get; set; }

        /// <summary>
        /// Gets or sets this field.
        /// </summary>
        T ResultObject { get; set; }

        /// <summary>
        /// Gets or sets this field.
        /// </summary>
        string Message { get; set; }
    }
}