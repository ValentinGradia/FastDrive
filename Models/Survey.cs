using System.ComponentModel.DataAnnotations;

namespace FastDrive.Models
{
    public class Survey
    {
        [Key]
        public int IDSurvey {  get; set; }
        public int IDUser { get; set; }
        public int IDBooking { get; set; }
        public string Description { get; set; }
        public int ServiceCalification {  get; set; }
    }
}
