using System.Text.Json.Serialization;
using System.Text.Json;

namespace FastDrive.Models
{
    public class JSON
    {

        //Solving the error of " A possible object cycle was detected which is not supported." when i give this into a json
        public static string JsonOptions<T>(T obj)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                MaxDepth = 0,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(obj, options);

            return json;
        }
    }
}
