﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    /// Attribute used to bind necessary information for a SignalR client to connect to SignalR Service.
    /// <para> 
    /// The connection info object will have the following properties:
    /// <code>
    /// public class MyConnectionInfo 
    /// {
    /// public string Url { get; set; }
    /// public string AccessToken { get; set; }
    /// } 
    /// </code>
    /// </para>
    /// </summary>
    public sealed class SignalRConnectionInfoInputAttribute : InputBindingAttribute
    {
        /// <summary>
        /// Gets or sets the app setting name that contains the Azure SignalR connection string.
        /// </summary>
        public string? ConnectionStringSetting { get; set; }

        /// <summary>
        /// Gets or sets the name of the hub to which the SignalR client is going to connect.
        /// </summary>
        public string? HubName { get; set; }

        /// <summary>
        /// Gets or sets the user id assigned to the SignalR client.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the JWT token whose claims will be added to the user claims.
        /// </summary>
        public string? IdToken { get; set; }

        /// <summary>
        /// Gets or sets the claim type list used to filter the claims in the <see cref="IdToken"/>.
        /// </summary>
        public string[]? ClaimTypeList { get; set; }
    }
}
