using SharedTrip.Data;
using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SharedTrip.Services
{
    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext db;

        public TripsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool AddUserToTrip(string userId, string tripId)
        {
            var userInTrip = this.db.UserTrips.Any(x => x.UserId == userId && x.TripId == tripId);
            if (userInTrip)
            {
                return false;
            }

            var userTrip = new UserTrip
            {
                TripId = tripId,
                UserId = userId,
            };

            this.db.UserTrips.Add(userTrip);
            this.db.SaveChanges();

            return true;
        }

        public bool HasTripAvailableSeats(string tripId)
        {
            var tripSeats = this.db.Trips
                .Where(x => x.Id == tripId)
                .Select(x => new { x.Seats, TakenSeats = x.UserTrips.Count() })
                .FirstOrDefault();
            var availableSeats = tripSeats.Seats - tripSeats.TakenSeats;

            return availableSeats > 0;
        }

        public void CreateTrip(AddTripInputModel trip)
        {
            var dbTrip = new Trip
            {
                StartPoint = trip.StartPoint,
                EndPoint = trip.EndPoint,
                DepartureTime = DateTime.ParseExact(trip.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
                Seats = (byte)trip.Seats,
                ImagePath = trip.ImagePath,
                Description = trip.Description,
            };

            this.db.Trips.Add(dbTrip);
            this.db.SaveChanges();
        }

        public IEnumerable<TripViewModel> GetAll()
        {
            var trips = this.db.Trips.Select(x => new TripViewModel
            {
                DepartureTime = x.DepartureTime,
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                Id = x.Id,
                Seats = x.Seats,
                UsedSeats = x.UserTrips.Count(),
            }).ToList();

            return trips;
        }

        public TripDetailsViewModel GetDetails(string tripId)
        {
            var trip = this.db.Trips.Where(x => x.Id == tripId)
                .Select(x => new TripDetailsViewModel 
                {
                    StartPoint = x.StartPoint,
                    Id = x.Id,
                    EndPoint = x.EndPoint,
                    DepartureTime = x.DepartureTime,
                    ImagePath = x.ImagePath,
                    Description = x.Description,
                    Seats = x.Seats,
                    UsedSeats = x.UserTrips.Count(),
                }).FirstOrDefault();

            return trip;
        }
    }
}
