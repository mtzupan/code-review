using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Zupan.CodeReview.WebApi.Models.Token;
using Zupan.CodeReview.Services.Interfaces.Core;

namespace Zupan.CodeReview.WebApi.Middleware.Token
{

    /// <summary>
    /// Refresh token provider middleware class
    /// </summary>
    public class RefreshTokenProviderMiddleware
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The options
        /// </summary>
        private readonly TokenProviderOptionsModel _options;

        /// <summary>
        /// The authorization service
        /// </summary>
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenProviderMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="tokenOptions">The token options.</param>
        /// <param name="authorizationService">The user service.</param>
        public RefreshTokenProviderMiddleware(
            RequestDelegate next, IOptions<TokenProviderOptionsModel> tokenOptions, IAuthorizationService authorizationService)
        {
            _next = next;

            _options = tokenOptions.Value;

            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.ToString().Equals(_options.RefreshTokenEndPoint, StringComparison.Ordinal))
            {
                // This always executes coming out of the pipeline.
                // So this will always return something bad
                // Request must be POST with Content-Type: application/x-www-form-urlencoded
                if (!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)
                {
                    //await GenerateUnauthorizedResponse(context);
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("This isn't a POST request, or it doesn't have FormContentType");

                    return;
                }

                try
                {
                    var username = context.Request.Form["UserName"].ToString();
                    var password = context.Request.Form["Password"].ToString();

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        //await GenerateUnauthorizedResponse(context);
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Username or password is being received as 'null' or 'empty'");
                        return;
                    }

                    await GenerateRefreshTokenAsync(context, username, password);
                    await _next(context);
                    return;
                }
                catch (Exception e)
                {
                    //await GenerateUnauthorizedResponse(context);
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(e.ToString());
                    return;
                }
            }
            await _next(context);
            // .NET Core Middleware Question - Does that go on?
        }

        /// <summary>
        /// Generates the unathorized request response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private async Task GenerateUnauthorizedResponse(HttpContext context)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }

        /// <summary>
        /// Generates the refresh token.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private async Task GenerateRefreshTokenAsync(HttpContext context, string username, string password)
        {
            var userInDatabase = _authorizationService.GetUserByLoginAndPassword(username, password);

            if (userInDatabase != null)
            {
                var genericIdentity = new GenericIdentity(userInDatabase.Name, "Token");
                var identity = new ClaimsIdentity(genericIdentity);
                identity.AddClaim(new Claim(TokenClaimsModel.TokenType, "refresh"));
                identity.AddClaim(new Claim(TokenClaimsModel.Revoked, "false"));
                identity.AddClaim(new Claim(TokenClaimsModel.UserId, userInDatabase.Id.ToString()));

                var now = DateTime.UtcNow.ToLocalTime();
                var refreshTokenClaims = identity.Claims;

                var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: refreshTokenClaims,
                    notBefore: now,
                    expires: now.Add(_options.RefreshTokenExpiration),
                    signingCredentials: _options.RefreshTokenSigningCredentials
                );

                var refreshToken = new JwtSecurityTokenHandler().WriteToken(jwt);

                context.Request.Headers.Add("grant_type", "refresh_token");
                context.Request.Headers.Add("refresh_token", refreshToken);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
            }
        }
    }
}
