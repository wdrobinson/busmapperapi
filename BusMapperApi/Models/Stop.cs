
namespace BusMapperApi.Models
{
    public class Stop
    {
        public int StopId { get; set; }
        public string StopName { get; set; }
        public string StopTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}