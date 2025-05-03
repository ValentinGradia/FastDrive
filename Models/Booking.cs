namespace FastDrive.Models
{
    public class Booking
    {

        public int IDBooking {  get; set; }
        public int IDUsuario { get; set; }
        public EBookingStatus Status { get; set; }
        public User User { get; set; }
        public Car Car { get; set; }
        public DateTime DateStart {  get; set; }
        public DateTime DateEnd { get; set; }

    }

    public enum EBookingStatus
    {
        Done,
        InProgress
    }
}
