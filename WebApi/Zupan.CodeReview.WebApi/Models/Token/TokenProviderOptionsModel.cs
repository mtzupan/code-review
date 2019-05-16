using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zupan.CodeReview.WebApi.Models.Token
{
    using System;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///
    /// </summary>
    public class TokenProviderOptionsModel
    {
        /// <summary>
        /// The scheme
        /// </summary>
        public const string Scheme = "Token";

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TokenType Type { get; set; }

        /// <summary>
        /// Gets or sets the accss token end point.
        /// </summary>
        /// <value>
        /// The access token end point.
        /// </value>
        public string AccessTokenEndPoint { get; set; }

        /// <summary>
        /// Gets or sets the refresh token end point.
        /// </summary>
        /// <value>
        /// The refresh token end point.
        /// </value>
        public string RefreshTokenEndPoint { get; set; }

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>
        /// The issuer.
        /// </value>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        /// <value>
        /// The audience.
        /// </value>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the expiration for refresh token.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        public TimeSpan RefreshTokenExpiration { get; set; }

        /// <summary>
        /// Gets or sets the access token expiration.
        /// </summary>
        /// <value>
        /// The access token expiration.
        /// </value>
        public TimeSpan AccessTokenExpiration { get; set; }

        /// <summary>
        /// Gets or sets the refresh token signing credentials.
        /// </summary>
        /// <value>
        /// The signing credentials.
        /// </value>
        public SigningCredentials RefreshTokenSigningCredentials { get; set; }

        /// <summary>
        /// Gets or sets the refresh token signing credentials.
        /// </summary>
        /// <value>
        /// The signing credentials.
        /// </value>
        public SigningCredentials AccessTokenSigningCredentials { get; set; }
    }

    /// <summary>
    /// The authentication token Type
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// The refresh type
        /// </summary>
        Refresh,

        /// <summary>
        /// The access type
        /// </summary>
        Access
    }
}
