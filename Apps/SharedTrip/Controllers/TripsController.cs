﻿using SharedTrip.Services;
using SharedTrip.ViewModels.Trips;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SharedTrip.Controllers
{
    public class TripsController: Controller
    {
        private readonly ITripsService tripsService;

        public TripsController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }
        public HttpResponse Add()
        {
            return this.View();
        }

        public HttpResponse All()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddTripInputModel input)
        {
            if (string.IsNullOrEmpty(input.StartPoint))
            {
                return this.Error("Start point is required.");
            }

            if (string.IsNullOrEmpty(input.EndPoint))
            {
                return this.Error("End point is required.");
            }

            if (input.Seats < 2 || input.Seats > 6)
            {
                return this.Error("Seats should be between 2 and 6.");
            }

            if (string.IsNullOrEmpty(input.Description) || input.Description.Length > 80)
            {
                return this.Error("Description is required and should be less than 80 characters long.");
            }

            if (!DateTime.TryParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return this.Error("Invalid data. Please use dd.MM.yyyy HH:mm format.");
            }

            this.tripsService.CreateTrip(input);

            return this.Redirect("/Trips/All");
        }
    }
}