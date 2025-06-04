using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace FastDrive.Models
{
    public class User
    {

        [Key]
        [JsonIgnore]
        public int IDUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string Surname {  get; set; }
        public int DNI { get; set; }
        public string UserType { get; set; }

        [AllowNull] //Navegation properties do not generate their own columns
        public ICollection<Booking> Bookings { get; set; }


    }

}
