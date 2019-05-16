namespace Zupan.CodeReview.WebApi.Models.Token
{
    /// <summary>
    /// This class represents what is inside the token
    /// </summary>
    public class TokenClaimsModel
    {
        /// <summary>
        /// The token type
        /// </summary>
        public const string TokenType = "token";

        /// <summary>
        /// The login
        /// </summary>
        public const string Login = "login";

        /// <summary>
        /// The user identifier
        /// </summary>
        public const string UserId = "userid";

        /// <summary>
        /// Is revoked
        /// </summary>
        public const string Revoked = "revoked";

        /// <summary>
        /// The role of the user
        /// </summary>
        public const string Role = "role";
    }
}
