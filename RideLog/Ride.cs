using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rides
{
    public class Ride
    {
        public enum WeatherCondition
        {
            none,
            sunny,
            cloudy,
            partlycloudy,
            rain,
            snow

        }

        #region FIELDS

        private string _trailSystem;
        private double _duration;
        private double _miles;
        private WeatherCondition _weather;
        private DateTime _date;

        #endregion

        #region PROPERTIES

        public string TrailSystem
        {
            get { return _trailSystem; }
            set { _trailSystem = value; }
        }
        public double Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public double Miles
        {
            get { return _miles; }
            set { _miles = value; }
        }

        public WeatherCondition Weather 
        {
            get { return _weather; }
            set { _weather = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }


        #endregion

        #region CONSTRUCTORS

        public Ride()
        {

        }

        public Ride(string trailsystem, double duration, double miles, WeatherCondition weather, DateTime date)
        {
            _trailSystem = trailsystem;
            _duration = duration;
            _miles = miles;
            _weather = weather;
            _date = date;
        }

        #endregion  

    }
}
