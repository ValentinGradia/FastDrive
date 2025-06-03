using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FastDrive.Models
{
    public class JWT
    {
        private static string _jwtToken = "KHPK6Ucf/zjvU4qW8/vkuuGLHeIo0l9ACJiTaAPLKbk=";

        public static string GenerateJwtToken(User user)
        {
            var key = JWT._jwtToken;
            // Create a symmetric security key using a secret key from the configuration.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // Specify the signing credentials using the security key and HMAC-SHA256 algorithm.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define the claims for the JWT token, including username and role.
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier , user.IDUser.ToString()), // Claim representing the user's ID.
                new Claim(ClaimTypes.Role, user.UserType.ToString())   // Claim representing the user's role.
            };

            // Create a JWT token with specified claims, expiration, and signing credentials.
            var token = new JwtSecurityToken(
                issuer: "MyApiServer",
                audience: "PostmanClient",
                claims: claims, // Pass the claims defined above.
                expires: DateTime.Now.AddHours(1), // Set token expiration to 1 hour from now.
                signingCredentials: credentials); // Include signing credentials for the token.
            // Serialize the JWT token into a string and return it.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
