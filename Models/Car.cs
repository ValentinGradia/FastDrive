using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FastDrive.Models
{
    public class Car
    {
        [Key]
        public string Patent { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public int Km { get; set; }
        public ECarStatus CarStatus { get; set; }

        [AllowNull]//Navegation properties do not generate their own columns
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] //to avoid the loop
        public ICollection<Booking> Bookings { get; set; }

    }

    public enum ECarStatus
    {
        Available = 0,
        Booked = 1,
        UnderRepair = 2,
    }
}
