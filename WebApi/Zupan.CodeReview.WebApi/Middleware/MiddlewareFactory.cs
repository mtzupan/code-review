namespace Zupan.CodeReview.WebApi.Middleware
{
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.IdentityModel.Tokens;
    using Token;
    using Models.Token;

    /// <summary>
    /// Middleware factory that provides a extension method to the app builder
    /// </summary>
    public static class MiddlewareFactory
    {
        /// <summary>
        /// Uses the access token provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="options">The options.</param>
        /// <param name="validationParameters">The validation parameters.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAccessTokenProvider(this IApplicationBuilder builder, IOptions<TokenProviderOptionsModel> options, IOptions<TokenValidationParameters> validationParameters)
        {
            return builder.UseMiddleware<AccessTokenProviderMiddleware>(options, validationParameters);
        }

        /// <summary>
        /// Uses the refresh token provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="tokenOptions">The token options.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseRefreshTokenProvider(this IApplicationBuilder builder, IOptions<TokenProviderOptionsModel> tokenOptions)
        {
            return builder.UseMiddleware<RefreshTokenProviderMiddleware>(tokenOptions);
        }
    }
}
