using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastDrive.Models
{
    public class Booking
    {
        [Key]
        public int IDBooking {  get; set; }
        public int IDUser { get; set; }

        public string CarPatent {  get; set; }

        [ForeignKey(nameof(IDUser))]
        public User User { get; set; }

        [ForeignKey("CarPatent")]
        public Car Car { get; set; }

        public DateTime DateStart {  get; set; }
        public DateTime DateEnd { get; set; }

    }
}
