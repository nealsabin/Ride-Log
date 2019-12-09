using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Rides
{
    // **************************************************
    //
    // Title: Ride Log
    // Description: Demonstration of classes and objects
    // Author: Sabin, Neal 
    // Dated Created: 11/25/2019
    // Last Modified: 12/8/2019
    //
    // **************************************************    
    class Program
    {
        static void Main(string[] args)
        {
            //
            // set theme
            //
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;

            //
            // read data from file into list
            //
            List<Ride> rides = ReadFromDataFile();

            //
            // call methods
            //
            DisplayWelcomeScreen();
            DisplayMenuScreen(rides);
            DisplayClosingScreen();

        }

        //static List<Ride> InitializeMonsterList()
        //{
        //    //
        //    // create a list of rides
        //    //
        //    List<Ride> rides = new List<Ride>()
        //    {

        //        new Ride()
        //        {
        //            TrailSystem = "Commons",
        //            Duration = 1.5,
        //            Weather = Ride.WeatherCondition.sunny
        //        },

        //        new Ride()
        //        {
        //            TrailSystem = "VASA",
        //            Duration = 3.5,
        //            Weather = Ride.WeatherCondition.cloudy
        //        }

        //    };

        //    Console.WriteLine(rides[0]);

        //    return rides;
        //}

        static void WriteToDataFile(List<Ride> rides)
        {
            string[] rideStrings = new string[rides.Count];

            //
            // create the array of ride string
            //
            for (int index = 0; index < rides.Count; index++)
            {
                //
                // create ride string
                //
                string rideString =
                    rides[index].TrailSystem + "," +
                    rides[index].Duration + "," +
                    rides[index].Miles + "," +
                    rides[index].Weather + "," +
                    rides[index].Date;

                //
                // add ride string to array
                //
                rideStrings[index] = rideString;
            }

            //
            // write array to data file
            //
            File.WriteAllLines("Data\\Data.txt", rideStrings);
        }

        static List<Ride> ReadFromDataFile()
        {
            List<Ride> rides = new List<Ride>();
            CultureInfo provider = CultureInfo.InvariantCulture;
            //
            // read from data file into an array
            //
            string[] rideStrings = File.ReadAllLines("Data\\Data.txt");

            foreach (string ride in rideStrings)
            {
                //
                // get ride property values
                //
                string[] rideProperties = ride.Split(',');

                //
                // create a new ride with the property values
                //
                Ride newRide = new Ride();

                newRide.TrailSystem = rideProperties[0];

                double.TryParse(rideProperties[1], out double duration);
                newRide.Duration = duration;

                double.TryParse(rideProperties[2], out double miles);
                newRide.Miles = miles;

                Enum.TryParse(rideProperties[3], out Ride.WeatherCondition weather);
                newRide.Weather = weather;

                newRide.Date = Convert.ToDateTime(rideProperties[4]);

                //
                // add new  ride to list
                //
                rides.Add(newRide);
            }

            return rides;
        }

        static void DisplayMenuScreen(List<Ride> rides)
        {
            bool quitApplication = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) List All Rides");
                Console.WriteLine("\t-----------------");
                Console.WriteLine("\tb) View Ride Detail");
                Console.WriteLine("\t-------------------");
                Console.WriteLine("\tc) Add Ride");
                Console.WriteLine("\t-----------");
                Console.WriteLine("\td) Delete Ride");
                Console.WriteLine("\t--------------");
                Console.WriteLine("\te) Update Ride");
                Console.WriteLine("\t--------------");
                Console.WriteLine("\tf) Write to Data File");
                Console.WriteLine("\t---------------------");
                Console.WriteLine("\tg) Filter by Sunny Weather");
                Console.WriteLine("\t--------------------");
                Console.WriteLine("\tq) Quit");
                Console.WriteLine("\t-------");
                Console.WriteLine();
                Console.Write("\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayAllRides(rides);
                        break;

                    case "b":
                        DisplayViewRideDetail(rides);
                        break;

                    case "c":
                        DisplayAddRide(rides);
                        break;

                    case "d":
                        DisplayDeleteRide(rides);
                        break;

                    case "e":
                        DisplayUpdateRide(rides);
                        break;

                    case "f":
                        DisplayWriteToFile(rides);
                        break;

                    case "g":
                        DisplayFilterByWeather(rides);
                        break;

                    case "q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        DisplayErrorMessageLetterShown();
                        break;
                }


            } while (!quitApplication);
        }

        static void DisplayFilterByWeather(List<Ride> rides)
        {
            List<Ride> filteredWeather = new List<Ride>();

            Ride.WeatherCondition selectedWeather = Ride.WeatherCondition.sunny;

            DisplayScreenHeader("Filter by Weather");

            //
            // add rides with the selected attitude to a new list
            //
            foreach (Ride ride in rides)
            {
                if (ride.Weather == selectedWeather)
                {
                    filteredWeather.Add(ride);
                }
            }

            //
            // LINQ examples
            //
            filteredWeather = rides.Where(m => m.Weather == selectedWeather).ToList();
            filteredWeather = filteredWeather.OrderBy(m => m.TrailSystem).ToList();

            //
            // display new list
            //
            Console.WriteLine($"\t{selectedWeather} Weather");
            Console.WriteLine();
            Console.WriteLine("\t***************************");
            foreach (Ride ride in filteredWeather)
            {
                RideInfo(ride);
                Console.WriteLine();
                Console.WriteLine("\t***************************");
            }

            DisplayContinuePrompt();
        }

        static void DisplayWriteToFile(List<Ride> rides)
        {
            DisplayScreenHeader("Write to Data File");

            // prompt the user to proceed or cancel
            DisplayContinuePrompt();

            WriteToDataFile(rides);

            // process the exceptions
            Console.WriteLine();
            Console.WriteLine("\tData written to file correctly.");

            DisplayContinuePrompt();

        }

        /// <summary>
        /// update rides
        /// </summary>
        static void DisplayUpdateRide(List<Ride> rides)
        {
            /// todo - numbered menu to select ride

            bool validResponse = false;
            Ride selectedRide = null;
            bool validInput;

            do
            {
                DisplayScreenHeader("Update Ride");

                //
                // display all ride names
                //
                Console.WriteLine("\tTrail System");
                Console.WriteLine("\t-------------");
                foreach (Ride ride in rides)
                {
                    Console.WriteLine("\t" + ride.TrailSystem);
                }

                //
                // get user ride choice
                //
                Console.WriteLine("\t-------------");
                Console.WriteLine();
                Console.Write("\tEnter name:");
                string rideName = Console.ReadLine();

                //
                // get ride object
                //

                foreach (Ride ride in rides)
                {
                    if (ride.TrailSystem == rideName)
                    {
                        selectedRide = ride;
                        validResponse = true;
                        break;
                    }
                }

                //
                // feedback for wrong name choice
                //
                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\tPlease select a ride shown above.");
                    Console.ForegroundColor = ConsoleColor.Black;
                    DisplayContinuePrompt();
                }
            } while (!validResponse);


            //
            // update ride
            //
            string userResponse;

            Console.WriteLine();
            Console.WriteLine("\tReady to update. Press enter to keep the current info.");
            Console.WriteLine();

            Console.WriteLine($"\tCurrent Trail System: {selectedRide.TrailSystem}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\tNew Trail System: ");
            Console.ForegroundColor = ConsoleColor.Black;
            userResponse = Console.ReadLine();
            Console.WriteLine();
            if (userResponse != "")
            {
                selectedRide.TrailSystem = userResponse;
            }

            Console.WriteLine($"\tCurrent Duration (hours): {selectedRide.Duration}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\tNew Duration (hours): ");
            Console.ForegroundColor = ConsoleColor.Black;
            userResponse = Console.ReadLine();
            Console.WriteLine();
            if (userResponse != "")
            {
                do
                {
                    if (double.TryParse(userResponse, out double duration) & duration > 0)
                    {
                        selectedRide.Duration = duration;
                        validInput = true;
                    }
                    else
                    {
                        DisplayErrorMessage();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("\tNew Duration (hours): ");
                        Console.ForegroundColor = ConsoleColor.Black;
                        validInput = false;
                    }
                } while (!validInput);
            }

            Console.WriteLine($"\tCurrent Distance (miles): {selectedRide.Miles}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\tNew Distance (miles: ");
            Console.ForegroundColor = ConsoleColor.Black;
            userResponse = Console.ReadLine();
            Console.WriteLine();
            if (userResponse != "")
            {
                do
                {
                    if (double.TryParse(userResponse, out double miles) & miles > 0)
                    {
                        selectedRide.Miles = miles;
                        validInput = true;
                    }
                    else
                    {
                        DisplayErrorMessage();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("\tNew Distance: ");
                        Console.ForegroundColor = ConsoleColor.Black;
                        validInput = false;
                    }
                } while (!validInput);
            }

            Console.WriteLine($"\tCurrent Weather: {selectedRide.Weather}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\tNew Weather: ");
            Console.ForegroundColor = ConsoleColor.Black;
            userResponse = Console.ReadLine();
            Console.WriteLine();
            if (userResponse != "")
            {
                do
                {
                    if (Enum.TryParse(userResponse, out Ride.WeatherCondition weather))
                    {
                        selectedRide.Weather = weather;
                        validInput = true;
                    }
                    else
                    {
                        DisplayWeatherErrorMessage();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("\tNew Weather: ");
                        Console.ForegroundColor = ConsoleColor.Black;
                        validInput = false;
                    }
                } while (!validInput);
            }




            Console.WriteLine($"\tCurrent Date: {selectedRide.Date}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\tNew Date: ");
            Console.ForegroundColor = ConsoleColor.Black;
            userResponse = Console.ReadLine();
            Console.WriteLine();
            if (userResponse != "")
            {
                selectedRide.Date = Convert.ToDateTime(userResponse);
            }

            DisplayContinuePrompt();

        }

        static void DisplayDeleteRide(List<Ride> rides)
        {
            DisplayScreenHeader("Delete Ride");

            //
            // display all ride names
            //
            Console.WriteLine("\tRide Names");
            Console.WriteLine("\t-------------");
            foreach (Ride ride in rides)
            {
                Console.WriteLine("\t" + ride.TrailSystem);
            }

            //
            // get user ride choice
            //
            Console.WriteLine();
            Console.Write("\tEnter name:");
            string rideName = Console.ReadLine();

            //
            // get ride object
            //
            Ride selectedRide = null;
            foreach (Ride ride in rides)
            {
                if (ride.TrailSystem == rideName)
                {
                    selectedRide = ride;
                    break;
                }
            }

            //
            // delete ride
            //
            if (selectedRide != null)
            {
                rides.Remove(selectedRide);
                Console.WriteLine();
                Console.WriteLine($"\t{selectedRide.TrailSystem} deleted");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"\t{rideName} not found");
            }

            DisplayContinuePrompt();
        }

        static void DisplayViewRideDetail(List<Ride> rides)
        {
            DisplayScreenHeader("Ride Detail");

            //
            // display all ride names
            //
            Console.WriteLine("\tRide Names");
            Console.WriteLine("\t-------------");
            foreach (Ride ride in rides)
            {
                Console.WriteLine("\t" + ride.TrailSystem);
            }

            //
            // get user ride choice
            //
            Console.WriteLine();
            Console.Write("\tEnter name:");
            string rideName = Console.ReadLine();

            //
            // get ride object
            //
            Ride selectedRide = null;
            foreach (Ride ride in rides)
            {
                if (ride.TrailSystem == rideName)
                {
                    selectedRide = ride;
                    break;
                }
            }

            //
            // display ride detail
            //
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.WriteLine("\t**************************");
            RideInfo(selectedRide);
            Console.WriteLine();
            Console.WriteLine("\t**************************");
            Console.ForegroundColor = ConsoleColor.Black;

            DisplayContinuePrompt();
        }

        static void DisplayAddRide(List<Ride> rides)
        {
            // todo - add option to confirm ride input
            Ride newRide = new Ride();
            bool validResponse;
            //string userResponse, format;
            //DateTime result;
            //CultureInfo provider = CultureInfo.InvariantCulture;

            //format = "d";

            DisplayScreenHeader("Add Ride");

            //
            // add ride object property values
            //

            //do
            //{
            //    Console.Write("\tDate of Ride: ");

            //    if (result = DateTime.ParseExact(ride.Date,format,provider))
            //    {
            //        newRide.Date = Convert.ToDateTime(Console.ReadLine());
            //        validResponse = true;
            //    }
            //    else
            //    {
            //        // FIX
            //        Console.WriteLine();
            //        Console.WriteLine("Please enter a number; 5, 25, 555");
            //        Console.WriteLine();
            //        validResponse = false;
            //    }

            //} while (!validResponse);

            Console.Write("\tDate of Ride: ");
            newRide.Date = Convert.ToDateTime(Console.ReadLine());

            Console.Write("\tTrail System: ");
            newRide.TrailSystem = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(newRide.TrailSystem))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tTrail System can not be empty.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\tTrail System: ");
                newRide.TrailSystem = Console.ReadLine();
            }

            do
            {
                Console.Write("\tDuration (hours): ");

                if (double.TryParse(Console.ReadLine(), out double duration) & duration > 0)
                {
                    newRide.Duration = duration;
                    validResponse = true;
                }
                else
                {
                    DisplayErrorMessage();
                    validResponse = false;
                }
            } while (!validResponse);

            do
            {
                Console.Write("\tDistance (miles): ");

                if (double.TryParse(Console.ReadLine(), out double miles) & miles > 0)
                {
                    newRide.Miles = miles;
                    validResponse = true;
                }
                else
                {
                    DisplayErrorMessage();
                    validResponse = false;
                }
            } while (!validResponse);

            do
            {
                Console.Write("\tWeather: ");

                if (Enum.TryParse(Console.ReadLine(), out Ride.WeatherCondition weather))
                {
                    newRide.Weather = weather;
                    validResponse = true;
                }
                else
                {
                    DisplayWeatherErrorMessage();
                    validResponse = false;
                }
            } while (!validResponse);

            //
            // echo new ride properties
            //
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\tNew Ride's Properties");
            Console.WriteLine("\t---------------------");
            Console.ForegroundColor = ConsoleColor.Black;
            RideInfo(newRide);
            DisplayContinuePrompt();

            rides.Add(newRide);
        }

        static void DisplayAllRides(List<Ride> rides)
        {
            DisplayScreenHeader("All Rides");

            Console.WriteLine("\t***************************");
            foreach (Ride ride in rides)
            {
                RideInfo(ride);
                Console.WriteLine();
                Console.WriteLine("\t***************************");
            }

            DisplayContinuePrompt();
        }

        static void RideInfo(Ride ride)
        {
            Console.WriteLine($"\tName: {ride.TrailSystem}");
            Console.WriteLine($"\tDuration (hours): {ride.Duration}");
            Console.WriteLine($"\tDistance (miles): {ride.Miles}");
            Console.WriteLine($"\tWeather: {ride.Weather}");
            Console.WriteLine($"\tDate: {ride.Date}");
        }

        #region HELPER METHODS

        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tRide Log");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Ride Log!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Error message shown when user inputs something other than one of the letters shown. 
        /// </summary>
        static void DisplayErrorMessageLetterShown()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t************************************************");
            Console.WriteLine("\tPlease indicate your choice with a letter shown.");
            Console.WriteLine("\t************************************************");
            Console.ForegroundColor = ConsoleColor.Black;
            DisplayContinuePrompt();
        }


        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Display Error Message for inputing something other than a number. 
        /// </summary>
        static void DisplayErrorMessage()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\tPlease enter a number; 1.5, 3, 5.5");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Display Error message for invalid input of weather
        /// </summary>
        static void DisplayWeatherErrorMessage()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\tPlease enter a valid weather condition; none, sunny, cloudy, partly cloudy, rain, or snow");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t\t" + headerText);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
        }

        #endregion
    }
}
