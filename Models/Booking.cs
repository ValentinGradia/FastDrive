using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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

        public ECarStatus BookingStatus { get; set; }

        public int? Km {  get; set; } // total Km that the car travel

        public bool? DamageReport { get; set; } //If the car has it, has to be under repair

        public int? Cost {  get; set; } //Cost of the booking depending on the total KM

    }


    public enum  EBookingStatus
    {
        Reserved = 0, //when user make the booking
        InUse = 1, //when the worker deliver the car
        Completed = 2, //when the car is returned
        Cancelled = 3
    }
}
