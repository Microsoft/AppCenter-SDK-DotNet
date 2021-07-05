// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net.Http;
using System.Security.Authentication;

namespace Microsoft.AppCenter.Ingestion.Http
{
    /// <inheritdoc />
    public sealed partial class HttpNetworkAdapter
    {
        // Static initializer specific to windows desktop platforms.
        static HttpNetworkAdapter()
        {
            HttpMessageHandlerOverride = () => new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12
            };
        }
    }
}
