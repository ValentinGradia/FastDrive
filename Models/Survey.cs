using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FastDrive.Models
{
    public class Survey
    {
        [Key]
        [JsonIgnore]
        public int IDSurvey {  get; set; }
        public int IDUser { get; set; }
        public int IDBooking { get; set; }
        public string Description { get; set; }
        public int ServiceCalification {  get; set; }
    }
}
