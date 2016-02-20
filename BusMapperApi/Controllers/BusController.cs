using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusMapperApi.Models;
using transit_realtime;
using BusMapperApi.Services;
using ProtoBuf;
using System.Web.Http.Cors;

namespace BusMapperApi.Controllers
{
    [CustomExceptionFilter]
    [EnableCors(origins: "http://willdrob.github.io, http://localhost:8000", headers: "*", methods: "*")]
    public class BusController : ApiController
    {
        public HttpResponseMessage GetTrips()
        {
            var memCacher = new MemoryCacher();
            var cacheTrips = memCacher.GetValue("trips");
            if (cacheTrips != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, cacheTrips);
            }
            var trips = BusService.INSTANCE.getTrips();
            memCacher.Add("trips", trips, DateTimeOffset.UtcNow.AddDays(1));
            return Request.CreateResponse(HttpStatusCode.OK, trips);
        }

        public HttpResponseMessage GetTripStops(int tripId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, BusService.INSTANCE.getTripStops(tripId));
        }

        public HttpResponseMessage GetShape(int shapeId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, BusService.INSTANCE.GetShape(shapeId));
        }

        public HttpResponseMessage GetStopTimes(int stopId, int tripId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, BusService.INSTANCE.getStopTimes(stopId, tripId));
        }

        public HttpResponseMessage GetBusPositions()
        {
            var memCacher = new MemoryCacher();
            var cacheBuses = memCacher.GetValue("busPostions");
            if (cacheBuses != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, cacheBuses);
            }
            bool updatingCacheBuses = false;
            var cacheObject = memCacher.GetValue("updatingBusCache");
            if (cacheObject != null)
            {
                updatingCacheBuses = (bool)cacheObject;
            }
            if (updatingCacheBuses)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Currently updating positions");
            }
            return Request.CreateResponse(HttpStatusCode.OK, UpdateBusCache());
        }

        private List<Bus> UpdateBusCache()
        {
            var memCacher = new MemoryCacher();
            memCacher.Add("updatingBusCache", true, DateTimeOffset.UtcNow.AddSeconds(10));

            WebRequest vehiclePositionsReq = HttpWebRequest.Create("http://googletransit.ridetarc.org/realtime/vehicle/VehiclePositions.pb");
            FeedMessage vehiclePositionsFeed = Serializer.Deserialize<FeedMessage>(vehiclePositionsReq.GetResponse().GetResponseStream());
            var buses = new List<Bus>();

            foreach (var entity in vehiclePositionsFeed.entity)
            {
                if (vehiclePositionsFeed.header.timestamp - entity.vehicle.timestamp < 600000)
                {
                    buses.Add(new Bus
                    {
                        Label = entity.vehicle.vehicle.label,
                        TripId = Convert.ToInt32(entity.vehicle.trip.trip_id),
                        Latitude = Convert.ToDouble(entity.vehicle.position.latitude),
                        Longitude = Convert.ToDouble(entity.vehicle.position.longitude)
                    });
                }
            }
            memCacher.Add("busPostions", buses, DateTimeOffset.UtcNow.AddSeconds(30));
            memCacher.Add("updatingBusCache", false, DateTimeOffset.UtcNow.AddSeconds(30));
            return buses;
        }
    }
}