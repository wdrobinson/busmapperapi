using System;
using System.Collections.Generic;
using System.Linq;
using BusMapperApi.Models;
using Dapper;
using System.Data.SqlClient;

namespace BusMapperApi.Dao
{
    public class BusDao
    {

        public List<Trip> getTrips()
        {
            using (SqlConnection conn = DaoConnections.GetDatabaseConnection())
            {
                string cmdText = String.Format("SELECT route_id AS RouteId, direction_id AS DirectionId, trip_id AS TripId, trip_headsign AS TripName, shape_id AS ShapeId FROM trips");
                return conn.Query<Trip>(cmdText).ToList<Trip>();
            }
        }

        public List<Stop> getTripStops(int tripId)
        {
            using (SqlConnection conn = DaoConnections.GetDatabaseConnection())
            {
                string cmdText = String.Format("SELECT stops.stop_id AS StopId, stop_name AS StopName, arrival_time AS StopTime, stop_lat AS Latitude, stop_lon AS Longitude FROM stop_times JOIN stops ON (stop_times.stop_id = stops.stop_id) WHERE trip_id = @tripId");
                return conn.Query<Stop>(cmdText, new { tripId = tripId }).ToList<Stop>();
            }
        }

        public List<Point> getPoints(int shapeId)
        {
            using (SqlConnection conn = DaoConnections.GetDatabaseConnection())
            {
                string cmdText = String.Format("SELECT shape_pt_lat AS Latitude, shape_pt_lon AS Longitude FROM shapes WHERE shape_id = @shapeId ORDER BY shape_pt_sequence");
                return conn.Query<Point>(cmdText, new { shapeId = shapeId }).ToList<Point>();
            }
        }

        public List<string> getStopTimes(int stopId, int tripId)
        {
            using (SqlConnection conn = DaoConnections.GetDatabaseConnection())
            {
                string cmdText = String.Format("SELECT arrival_time FROM stop_times WHERE stop_id = @stopId AND trip_id = @tripId");
                return conn.Query<string>(cmdText, new { stopId, tripId }).ToList<string>();
            }
        }
    }
}