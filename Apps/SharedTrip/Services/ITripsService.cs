using SharedTrip.ViewModels.Trips;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        public void CreateTrip(AddTripInputModel trip);
    }
}
