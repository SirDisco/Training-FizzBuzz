using System;
using System.Collections.Generic;

namespace BusBoard
{
    public class UserInterface
    {
        // Current Enquiry Values.
        private LatitudeLongitude longLat;
        private string postCode;
        private string stopID;
        private BusStop currentStop;
        private List<BusStop> _nearbyBusStops;
        
        // Current UI States.
        private States currentState;
        private int _nearestStopsToShow;
        private int arrivalsToShow;

        public UserInterface(int nearestStopsToShow, int arrivalsToShow)
        {
            currentState = States.Lost;
            this._nearestStopsToShow = nearestStopsToShow;
            this.arrivalsToShow = arrivalsToShow;
        }

        public void Run()
        {
            while (true)
            {
                switch (currentState)
                {
                    case States.Lost:
                        Lost();
                        break;
                    case States.ChooseYourStop:
                        ChooseYourStop(_nearbyBusStops);
                        break;
                    case States.AtStop:
                        AtStop();
                        break;
                    case States.AtArrivals:
                        // do you want directions to this stop?
                        // new enquiry
                        break;
                    case States.AtDirections:
                        // new enquiry
                        // back to arivals
                        break;
                }
            }

        }

        private void AtStop()
        {
            // Display Arrivals
            // Give option of directions to Stop (will require location prompt).
        }
        

        private void Lost()
        {
            _initEnquiry();
            LocationType locationType = LocationType.Invalid;
            Console.WriteLine($"\nYou are lost. \nEnter a: Postcode, StopID or Co-Ordinates(space between).");

            
            while (locationType == LocationType.Invalid)
            {
                var stringToValidate = Console.ReadLine();
                locationType = inputValidator(stringToValidate);
                
                switch (locationType)
                {
                    case LocationType.Postcode:
                        longLat = Postcode_API.GetLatLongFromPostcode(stringToValidate);
                        _nearbyBusStops = TFL_API.GetStopFromLongLat(longLat, _nearestStopsToShow);
                        currentState = States.ChooseYourStop;
                        break;
                    case LocationType.LonLat:
                        
                        break;
                    case LocationType.StopID:
                        currentState = States.AtStop;
                        break;
                    case LocationType.Invalid:
                        Console.WriteLine("This is not a valid postcode, stopID or Co-ordinate. Try again.");
                        break;
                }
                
            }
            


        }

        private LocationType inputValidator(string stringToValidate)
        {
            // is a postcode
            if (Postcode_API.IsValidPostcode((stringToValidate)))
                return LocationType.Postcode;
            
            // is longLat
            // is a stopId;

            return LocationType.Invalid;
        }

        private void ChooseYourStop(List<BusStop> busStops)
        {
            // Display Messages
            if (busStops == null || busStops.Count == 0)
            {
                Console.WriteLine("There are no nearby bus stops, try again.");
                currentState = States.Lost;
                return;
            }
            Console.WriteLine($"\nThe following stops are nearby:");
            Console.WriteLine("Select a bus stop to see arrivals and route options.");
            
            Console.WriteLine("[0] ...see arrivals for all stops.");
            for (int i = 0; i < busStops.Count; i++)
            {
                Console.WriteLine($"[{i+1}] {busStops[i].CommonName}");
            }
            Console.WriteLine($"[{busStops.Count+1}] ...back");

            
            // READ USER INPUT
            int userSelection = -1;
            bool selectionValid = false;
            while (!selectionValid)
            {
                userSelection = int.Parse(Console.ReadLine() ?? string.Empty);
                if (userSelection <= busStops.Count + 1)
                {
                    selectionValid = true;
                }
                else
                {
                    Console.WriteLine("Incorrect Selection, try again.");
                }
            }
            
            // BEHAVIOUR FOR SELECTION
            if (userSelection == 0)
            {
                //Display All
                foreach (var stop in _nearbyBusStops)
                {
                    
                }
                ChooseYourStop(_nearbyBusStops);
            }
            if (userSelection > 0 && userSelection < busStops.Count + 1)
            {
                currentStop = busStops[userSelection-1];
                currentState = States.AtStop;
            }
            if (userSelection > busStops.Count)
            {
                currentState = States.Lost;
            }
            

        }

        private void _initEnquiry()
        {
            longLat = null;
            _nearbyBusStops = new List<BusStop>();
            currentStop = null;
        }
        
        
        
        
        private void newRequest()
        {
            // Clears the variables
            currentState = States.Lost;

        }
        
        
        
        // I give postcode, i get nearest stops
        // I give stopID, i get next few arrivals
        // I give co-ordinates, i get nearest stops.

        // I request route and give location, i get directions.









    }
}