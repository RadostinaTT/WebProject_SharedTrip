using SharedTrip.ViewModels.Trips;
using System.Collections.Generic;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        public void CreateTrip(AddTripInputModel trip);

        IEnumerable<TripViewModel> GetAll();
    }
}
