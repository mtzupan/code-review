using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zupan.CodeReview.WebApi
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;
    using Middleware;
    using Models.Token;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Builder;
    using System;

    /// <summary>
    /// Auth
    /// </summary>
    public static class Auth
    {
        /// <summary>
        /// The refresh token key
        /// </summary>
        private static SymmetricSecurityKey _refreshTokenKey;

        /// <summary>
        /// The access token key
        /// </summary>
        private static SymmetricSecurityKey _accessTokenKey;

        /// <summary>
        /// The token validation parameters
        /// </summary>
        private static TokenValidationParameters _tokenValidationParameters;

        /// <summary>
        /// Initializes the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void SetAppParametersForAuth(IConfigurationRoot configuration)
        {
            _refreshTokenKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("Token:RefreshTokenSecretKey").Value));
            _accessTokenKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration.GetSection("Token:AccessTokenSecretKey").Value));
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _accessTokenKey,
                ValidateIssuer = true,
                ValidIssuer = configuration.GetSection("Token:Issuer").Value,
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("Token:Audience").Value,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        /// <summary>
        /// Configures the API authorization for the services.
        /// </summary>
        public static void ConfigureApiAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = _tokenValidationParameters;
                });
        }

        /// <summary>
        /// Configures the authentication parameters for the application
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="authorizationService">The authorization service.</param>
        public static void ConfigureAuthentication(this IApplicationBuilder app, IConfiguration configuration)
        {
            var refreshTokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _refreshTokenKey,
                ValidateIssuer = true,
                ValidIssuer = configuration.GetSection("Token:Issuer").Value,
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("Token:Audience").Value,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var providerModels = GenerateProviderTokenOptions(configuration, _refreshTokenKey, _accessTokenKey);

            app.UseRefreshTokenProvider(Options.Create(providerModels.FirstOrDefault(p => p.Type == TokenType.Refresh)));

            app.UseAccessTokenProvider(Options.Create(providerModels.FirstOrDefault(p => p.Type == TokenType.Access)), Options.Create(refreshTokenValidationParameters));

            app.UseAuthentication();
        }

        /// <summary>
        /// Generates provider token options for access and refresh token.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        /// <param name="refreshTokenKey">The refresh token key.</param>
        /// <param name="accessTokenKey">The access token key.</param>
        /// <returns>A list with the providers</returns>
        private static List<TokenProviderOptionsModel> GenerateProviderTokenOptions(IConfiguration configuration, SecurityKey refreshTokenKey, SecurityKey accessTokenKey)
        {
            var refreshTokenProviderOptions = new TokenProviderOptionsModel
            {
                Type = TokenType.Refresh,
                RefreshTokenSigningCredentials = new SigningCredentials(refreshTokenKey, SecurityAlgorithms.HmacSha256),
                RefreshTokenEndPoint = configuration.GetSection("Token:LoginEndPoint").Value,
                RefreshTokenExpiration =
                TimeSpan.Parse(configuration.GetSection("Token:RefreshTokenExpireMinutes").Value),
                Audience = configuration.GetSection("Token:Audience").Value,
                Issuer = configuration.GetSection("Token:Issuer").Value
            };

            var accessTokenProviderOptions = new TokenProviderOptionsModel
            {
                Type = TokenType.Access,
                AccessTokenSigningCredentials = new SigningCredentials(accessTokenKey, SecurityAlgorithms.HmacSha256),
                AccessTokenEndPoint = configuration.GetSection("Token:RefreshTokenEndPoint").Value,
                AccessTokenExpiration =
                TimeSpan.Parse(configuration.GetSection("Token:AccessTokenExpireMinutes").Value),
                Audience = configuration.GetSection("Token:Audience").Value,
                Issuer = configuration.GetSection("Token:Issuer").Value
            };

            var providerOptionsList =
                new List<TokenProviderOptionsModel> { refreshTokenProviderOptions, accessTokenProviderOptions };

            return providerOptionsList;
        }
    }
}
