using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zupan.CodeReview.WebApi.Middleware.Token
{
    using Microsoft.Extensions.Primitives;
    using Dtos.Common;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Models.Token;
    using Newtonsoft.Json;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Principal;
    using Zupan.CodeReview.Services.Interfaces.Core;
    using Zupan.CodeReview.Dtos.Core;

    /// <summary>
    /// Access Token Generation Middleware
    /// </summary>
    public class AccessTokenProviderMiddleware
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The serializer settings
        /// </summary>
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// The options
        /// </summary>
        private readonly TokenProviderOptionsModel _options;

        /// <summary>
        /// The authorization service
        /// </summary>
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// The validation parameters for refresh token
        /// </summary>
        private readonly TokenValidationParameters _refreshTokenvalidationParameters;

        /// <summary>
        /// The refresh token
        /// </summary>
        private string _refreshToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenProviderMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="tokenOptions">The token options.</param>
        /// <param name="refreshTokenvalidationParameters">The validation parameters for refreshToken.</param>
        /// <param name="authService">The auth service.</param>
        public AccessTokenProviderMiddleware(
            RequestDelegate next, IOptions<TokenProviderOptionsModel> tokenOptions, IOptions<TokenValidationParameters> refreshTokenvalidationParameters, IAuthorizationService authService)
        {
            _next = next;

            _options = tokenOptions.Value;

            _authService = authService;

            _refreshTokenvalidationParameters = refreshTokenvalidationParameters.Value;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue("grant_type", out var grantType);

            if (grantType == "refresh_token")
            {
                context.Request.Headers.TryGetValue("refresh_token", out var refreshToken);
                _refreshToken = (refreshToken != StringValues.Empty) ? refreshToken.ToString() : string.Empty;

                var refreshTokenClaims = await ValidateRefreshToken(context, _refreshToken);

                if (refreshTokenClaims == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Could not generate access token for the user");
                    return;
                }

                var result = GenerateAccessToken(refreshTokenClaims);

                if (result != null)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result, _serializerSettings));
                    return;
                }

                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Could not generate access token for the user");
                return;
            }
            await _next(context);
        }

        /// <summary>
        /// Validates the refresh token.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="refreshToken">The refresh token.</param>
        private async Task<ClaimsPrincipal> ValidateRefreshToken(HttpContext context, string refreshToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validRefreshTOken =
                    handler.ValidateToken(refreshToken, _refreshTokenvalidationParameters, out var _);
                return validRefreshTOken;
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Refresh Token.");
                return null;
            }
        }

        /// <summary>
        /// Generates the access token asynchronous.
        /// </summary>
        /// <param name="refreshTokenClaims">The refresh token claims.</param>
        private TalosResult<UserSessionsDto> GenerateAccessToken(ClaimsPrincipal refreshTokenClaims)
        {
            int.TryParse(refreshTokenClaims.FindFirst(TokenClaimsModel.UserId).Value, out var userId);

            if (userId <= 0)
            {
                return null;
            }

            var userInDatabase = _authService.GetUserById(userId);

            if (userInDatabase == null)
            {
                return null;
            }

            var genericIdentity = new GenericIdentity(userInDatabase.Name, "Token");
            var identity = new ClaimsIdentity(genericIdentity);
            identity.AddClaim(new Claim(TokenClaimsModel.TokenType, "access"));
            identity.AddClaim(new Claim(TokenClaimsModel.UserId, userInDatabase.Id.ToString()));
            identity.AddClaim(new Claim(TokenClaimsModel.Login, userInDatabase.Login));
            identity.AddClaim(new Claim(TokenClaimsModel.Role, userInDatabase.Name));

            var now = DateTime.UtcNow.ToLocalTime();
            var accessTokenClaims = identity.Claims;

            var jwt = new JwtSecurityToken(
                        _options.Issuer,
                        _options.Audience,
                        accessTokenClaims,
                        now,
                        now.Add(_options.AccessTokenExpiration),
                        _options.AccessTokenSigningCredentials
                        );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            var result = new UserSessionsDto()
            {
                User = userInDatabase,
                RefreshToken = _refreshToken,
                AccessToken = accessToken
            };

            var response = new TalosResult<UserSessionsDto>
            {
                Result = result,
                Success = true
            };

            return response;
        }
    }
}