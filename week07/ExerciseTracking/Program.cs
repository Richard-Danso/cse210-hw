using System;
using System.Collections.Generic;

namespace ExerciseTracking
{
    public abstract class Activity
    {
        // Encapsulation: private member variables
        private string _date;
        private int _minutes;

        // Constructor
        public Activity(string date, int minutes)
        {
            _date = date;
            _minutes = minutes;
        }

        // Getters for the private fields (needed by derived classes or calculations)
        public int GetMinutes()
        {
            return _minutes;
        }

        public string GetDate()
        {
            return _date;
        }

        // Abstract methods to be overridden in derived classes
        public abstract double GetDistance();
        public abstract double GetSpeed();
        public abstract double GetPace();

        // Virtual summary method available to all classes
        public virtual string GetSummary()
        {
            // Dynamically calls the overridden methods depending on the object type
            return $"{_date} {this.GetType().Name} ({_minutes} min) - " +
                   $"Distance {GetDistance():F1} miles, " +
                   $"Speed {GetSpeed():F1} mph, " +
                   $"Pace: {GetPace():F1} min per mile";
        }
    }
}

namespace ExerciseTracking
{
    public class Running : Activity
    {
        private double _distance; // In miles

        public Running(string date, int minutes, double distance) : base(date, minutes)
        {
            _distance = distance;
        }

        public override double GetDistance()
        {
            return _distance;
        }

        public override double GetSpeed()
        {
            // Speed (mph) = (distance / minutes) * 60
            return (_distance / GetMinutes()) * 60;
        }

        public override double GetPace()
        {
            // Pace (min per mile) = minutes / distance
            return GetMinutes() / _distance;
        }
    }
}

namespace ExerciseTracking
{
    public class Cycling : Activity
    {
        private double _speed; // In mph

        public Cycling(string date, int minutes, double speed) : base(date, minutes)
        {
            _speed = speed;
        }

        public override double GetDistance()
        {
            // Distance = (speed * minutes) / 60
            return (_speed * GetMinutes()) / 60;
        }

        public override double GetSpeed()
        {
            return _speed;
        }

        public override double GetPace()
        {
            // Pace = 60 / speed
            return 60 / _speed;
        }
    }
}

namespace ExerciseTracking
{
    public class Swimming : Activity
    {
        private int _laps;

        public Swimming(string date, int minutes, int laps) : base(date, minutes)
        {
            _laps = laps;
        }

        public override double GetDistance()
        {
            // Distance (miles) = swimming laps * 50 / 1000 * 0.62
            return _laps * 50.0 / 1000.0 * 0.62;
        }

        public override double GetSpeed()
        {
            return (GetDistance() / GetMinutes()) * 60;
        }

        public override double GetPace()
        {
            return GetMinutes() / GetDistance();
        }
    }
}


namespace ExerciseTracking
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a list to hold all types of activities
            List<Activity> activities = new List<Activity>();

            // Create at least one activity of each type
            Running run = new Running("03 Nov 2022", 30, 3.0);
            Cycling cycle = new Cycling("03 Nov 2022", 45, 15.0);
            Swimming swim = new Swimming("04 Nov 2022", 20, 20);

            // Put each activity in the same list
            activities.Add(run);
            activities.Add(cycle);
            activities.Add(swim);

            Console.WriteLine("Exercise Tracking Summary:\n");

            // Iterate through the list and polymorphically call GetSummary
            foreach (Activity activity in activities)
            {
                Console.WriteLine(activity.GetSummary());
            }
        }
    }
}