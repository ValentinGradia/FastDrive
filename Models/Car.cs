using System.ComponentModel.DataAnnotations;

namespace FastDrive.Models
{
    public class Car
    {
        [Key]
        public string Patent { get; set; }
        public string Model { get; set; }
        public int Km { get; set; }
        public ECarStatus CarStatus { get; set; }

    }

    public enum ECarStatus
    {
        Available,
        InUse,
        UnderRepair,
        Booked
    }
}
