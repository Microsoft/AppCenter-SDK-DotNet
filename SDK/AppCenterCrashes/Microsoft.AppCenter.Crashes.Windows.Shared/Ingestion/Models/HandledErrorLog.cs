// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.AppCenter.Crashes.Ingestion.Models
{
    using Microsoft.AppCenter.Ingestion.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Handled Error log for managed platforms (such as Xamarin, Unity,
    /// Android Dalvik/ART)
    /// </summary>
    [Newtonsoft.Json.JsonObject(JsonIdentifier)]
    public partial class HandledErrorLog : LogWithProperties
    {
        internal const string JsonIdentifier = "handledError";

        /// <summary>
        /// Initializes a new instance of the HandledErrorLog class.
        /// </summary>
        /// <param name="timestamp">Log timestamp, example:
        /// '2017-03-13T18:05:42Z'.
        /// </param>
        /// <param name="sid">When tracking an analytics session, logs can be
        /// part of the session by specifying this identifier.
        /// This attribute is optional, a missing value means the session
        /// tracking is disabled (like when using only error reporting
        /// feature).
        /// Concrete types like StartSessionLog or PageLog are always part of a
        /// session and always include this identifier.
        /// </param>
        /// <param name="userId">optional string used for associating logs with
        /// users.
        /// </param>
        /// <param name="properties">Additional key/value pair parameters.
        /// </param>
        /// <param name="id">Unique identifier for this Error.
        /// </param>
        public HandledErrorLog(Device device, Exception exception, System.DateTime? timestamp = default(System.DateTime?), System.Guid? sid = default(System.Guid?), string userId = default(string), IDictionary<string, string> properties = default(IDictionary<string, string>), System.Guid? id = default(System.Guid?))
            : base(device, timestamp, sid, userId, properties)
        {
            Id = id;
            Exception = exception;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        partial void CustomInit();

        /// <summary>
        /// Gets or sets unique identifier for this Error.
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "exception")]
        public Exception Exception { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public override void Validate()
        {
            base.Validate();
            if (Exception == null)
            {
                throw new ValidationException(ValidationException.Rule.CannotBeNull, "Exception");
            }
            if (Exception != null)
            {
                Exception.Validate();
            }
        }
    }
}
