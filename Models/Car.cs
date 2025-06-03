using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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

        [AllowNull]
        public ICollection<Booking> Bookings { get; set; }

    }

    public enum ECarStatus
    {
        Available = 0,
        InUse = 1,
        UnderRepair = 2,
        Booked = 3
    }
}
