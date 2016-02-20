using BusMapperApi.Dao;
using BusMapperApi.Models;
using System.Collections.Generic;

namespace BusMapperApi.Services
{
    public class BusService
    {
        BusDao BusDao = null;
        private static BusService instance;

        public static BusService INSTANCE
        {
            get
            {
                if (instance == null)
                {
                    instance = new BusService();
                }
                return instance;
            }
        }

        private BusService()
        {
            instance = this;
            BusDao = new BusDao();
        }

        public List<Trip> getTrips()
        {
            return BusDao.getTrips();
        }

        public List<Stop> getTripStops(int tripId)
        {
            return BusDao.getTripStops(tripId);
        }

        public List<Point> GetShape(int shapeId)
        {
            return BusDao.getPoints(shapeId);
        }

        public List<string> getStopTimes(int stopId, int tripId)
        {
            return BusDao.getStopTimes(stopId, tripId);
        }
    }
}