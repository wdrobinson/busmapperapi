
namespace BusMapperApi.Models
{
    public class Trip
    {
        public string RouteId { get; set; }
        public int DirectionId { get; set; }
        public int TripId { get; set; }
        public string TripName { get; set; }
        public int ShapeId { get; set; }
    }
}