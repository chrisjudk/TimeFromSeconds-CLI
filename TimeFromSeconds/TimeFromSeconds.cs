using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace TFS
{
    class TimeFromSeconds
    {
        public enum ErrorCode
        {
            Success = 0,
            FileNotFound = 2,
            InvalidData = 13,
            BadArguments = 160,
            InvalidIndex = 1413
        }//enum ErrorCode

        public enum Unit
        {
            Seconds,
            Minutes,
            Hours,
            Days,
            Weeks,
        }//enum Unit

        private const string DEV = "Chris Judkins";
        private static readonly int WEEK = 604800;
        private static readonly int DAY = 86400;
        private static readonly int HOUR = 3600;
        private static readonly int MINUTE = 60;
        private static readonly double MILLISECOND = Math.Pow(10, -3);
        private static readonly double MICROSECOND = Math.Pow(10, -6);
        private static readonly double NANOSECOND = Math.Pow(10, -9);
        private static bool debug;
        private double input;
        private static List<ErrorCode> eList = new List<ErrorCode>();

        public double Input
        {
            get { return this.input; }
            set
            {
                if (value >= 0)
                    this.input = value;
            }//set
        }//Input

        public Unit TFSUnit { get; set; }

        public static bool Debug
        {
            get
            {
                return debug;
            }//get
            set
            {
                debug = value;
            }//set
        }//Debug

        public override string ToString()
        {
            try
            {
                return FromSeconds();
            }//try
            catch (Exception e)
            {
                Error(e);
                return e.Message;
            }//catch
        }//ToString()

        public TimeFromSeconds(Unit aUnit, double aInput)
        {
            TFSUnit = aUnit;
            Input = aInput;
        }//TimeFromSeconds()

        public string FromSeconds()
        {
            int weeks = 0;
            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            int milliseconds = 0;
            int microseconds = 0;
            int nanoseconds = 0;
            double inVar = this.Input;
            double original = inVar;
            double originalSeconds = 0;
            StringBuilder sb = new StringBuilder();
            if (TFSUnit == Unit.Minutes)
            {
                inVar *= MINUTE; //Convert to seconds
            }//if minutes

            else if (TFSUnit == Unit.Hours)
            {
                inVar *= HOUR;
            }//if hours

            else if (TFSUnit == Unit.Days)
            {
                inVar *= DAY;
            }//if days

            else if (TFSUnit == Unit.Weeks)
            {
                inVar *= WEEK;
            }//if hours

            if (this.Input < 0)
            {
                eList.Add(ErrorCode.BadArguments);
                throw new ArgumentOutOfRangeException("Time cannot be negative. Input was: " + Input);
            }
            else
            {
                originalSeconds = inVar;
                if (TFSUnit != Unit.Weeks || ((TFSUnit == Unit.Weeks) && ((originalSeconds % WEEK) != 0)))
                {
                    while (inVar >= WEEK)
                    {
                        weeks++;
                        inVar -= WEEK;
                    }//weeks
                }//unit not weeks

                if ((TFSUnit != Unit.Days) || ((TFSUnit == Unit.Days) && ((originalSeconds > WEEK) || (originalSeconds % DAY) != 0)))
                {
                    while (inVar >= DAY)
                    {
                        days++;
                        inVar -= DAY;
                    }//days
                }//unit not days

                if ((TFSUnit != Unit.Hours) || ((TFSUnit == Unit.Hours) && ((originalSeconds > DAY) || (originalSeconds % HOUR) != 0)))
                {
                    while (inVar >= HOUR)
                    {
                        hours++;
                        inVar -= HOUR;
                    }//hours
                }//if unit not hours

                Debugger($"Debug: inVar = {inVar}, hours = {hours}, minutes = {minutes}, seconds = {seconds}");

                if ((TFSUnit != Unit.Minutes) || ((TFSUnit == Unit.Minutes) && ((originalSeconds > HOUR) || (originalSeconds % MINUTE) != 0)))
                {
                    while (inVar >= MINUTE)
                    {
                        minutes++;
                        inVar -= MINUTE;
                    }//minutes
                }//if unit not minutes
                Debugger($"Debug: inVar = {inVar}, hours = {hours}, minutes = {minutes}, seconds = {seconds}");

                while (inVar >= 1)
                {
                    seconds++;
                    inVar--;
                }//seconds

                while (inVar >= MILLISECOND)
                {
                    milliseconds++;
                    inVar -= MILLISECOND;
                }//milliseconds

                while (inVar >= MICROSECOND)
                {
                    microseconds++;
                    inVar -= MICROSECOND;
                }//microseconds

                while (inVar >= NANOSECOND)
                {
                    nanoseconds++;
                    inVar -= NANOSECOND;
                }//nanoseconds
                Debugger($"Debug: original = {original}\nTFSUnit = {TFSUnit}\ninVar = {inVar}\nweeks = {weeks}\ndays = {days}\nhours = {hours}\nminutes = {minutes}\nseconds = {seconds}\nmilliseconds = {milliseconds}\nnanoseconds = {nanoseconds}");
                sb.Append($"{original} {TFSUnit} is");
                if (weeks != 0)
                    sb.Append($" {weeks} weeks,");
                if (days != 0)
                    sb.Append($" {days} days,");
                if (hours != 0)
                    sb.Append($" {hours} hours,");
                if (minutes != 0)
                    sb.Append($" {minutes} minutes,");
                if (seconds != 0)
                    sb.Append($" {seconds} seconds,");
                if (milliseconds != 0)
                    sb.Append($" {milliseconds} milliseconds,");
                if (microseconds != 0)
                    sb.Append($" {microseconds} microseconds,");
                if (nanoseconds != 0)
                    sb.Append($" {nanoseconds} nanoseconds,");
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }//else  
        }//FromSeconds()



        public static void Main(string[] args)
        {
            int option = -1;
            double input = -1;
            if (args != null)
            {
                foreach (string arg in args)
                {
                    if(ArgsCheck(arg, "debug"))
                    {
                        Debug = true;
                    }//if
                }//foreach
            }//if
            else
            {
                Debug = false;
            }//default values
            Debugger("Debug: Debugging is on!");


            const int OPTIONCOUNT1 = 5;
            while(!(option < OPTIONCOUNT1 && (option >= 0)))
            {
                Console.WriteLine($"Welcome to the seconds to human readable time converter!\n\nThis program was produced by {DEV}.");
                Console.WriteLine("\n0) Seconds\n1) Minutes\n2) Hours\n3) Days\n4) Weeks\n");
                Console.Write("Select Input type ('q' to exit): ");
                string o = Convert.ToString(Console.ReadKey().KeyChar).ToLowerInvariant();
                Console.Clear();
                if (!o.Equals("q"))
                {
                    if (o.Equals("\r"))
                    {
                        option = -1;
                    }//if enter key pressed
                    else if (int.TryParse(o, out option))
                    {
                        if ((option < OPTIONCOUNT1) && (option >= 0))
                        {
                            while (!(input > 0))
                            {
                                Console.Clear();
                                Console.Write($"Please enter a time in {(Unit)option} (\"q\" to exit): ");
                                string i = Console.ReadLine().ToLowerInvariant();
                                if (!i.Equals("q"))
                                {
                                    if (double.TryParse(i, out input))
                                    {
                                        if (input >= 0)
                                        {
                                            TimeFromSeconds aTFS = new TimeFromSeconds((Unit)option, input);
                                            Console.WriteLine(aTFS);
                                            option = -1;
                                            input = -1;
                                            Console.WriteLine("Press any key to continue . . . (q to exit)");
                                            if (!(Convert.ToString(Console.ReadKey().KeyChar).ToLowerInvariant().Equals("q")))
                                            {
                                                option = -1;
                                                input = -1;
                                                Console.Clear();
                                                break;
                                            }//if key pressed not q
                                            else
                                                Exit();
                                        }//if input is positive
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Input must be a positive number. Please enter a valid input!");
                                        }//else input is not positive
                                    }//if input is parsed
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Input must be a positive number. Please enter a valid input!");
                                    }//else i failed to parse 
                                }//if i not q
                                else if (i.ToLower().Equals("q"))
                                {
                                    Exit();
                                }//else i is q
                            }// while input not positive
                        }//if option is valid
                    }//if option is parsed
                    else
                    {
                        Console.Clear();
                        option = -1;
                        Console.WriteLine("Please enter a valid option!\n");
                    }//else option is not parsed
                }//if o not q
                else if (o.ToLower().Equals("q"))
                {
                    Exit();
                }//else o is q
            }//while option not valid
        }//Main()




        private static void Debugger(string m)
        {
            if (Debug)
            {
                Console.WriteLine("\n\n---------------debug---------------\n");
                Console.WriteLine(m);
                Console.WriteLine("\n-------------end debug-------------\n\n");
            }//if
        }//Error(String)

        private static void Error(string m, Exception e)
        {
            Debugger($"{m}\nError:\n\nData: {e.Data}\nMessage: {e.Message}\nSource: {e.Source}");
            Console.WriteLine("\nSomething went wrong!\n\nError: " + e.Message);
        }//Error(String, Exception)

        private static void Error(Exception e)
        {
            Debugger($"Error:\n\nData: {e.Data}\nMessage: {e.Message}\nSource: {e.Source}");
            Console.WriteLine("\nSomething went wrong!\n\nError: " + e.Message);
        }//Error(Exception)

        private static void Exit()
        {
            if (eList.Count == 0)
            {
                Console.WriteLine("\nPress any key to exit . . .");
                Console.ReadKey();
                Environment.Exit((int)ErrorCode.Success);
            }
            else
            {
                ErrorCode[] eArray = eList.ToArray();
                int index = eArray.Length;
                foreach (ErrorCode e in eArray)
                    Debugger($"Error Code: {e}");
                Console.WriteLine("\nPress any key to exit . . .");
                Console.ReadKey();
                Environment.Exit((int)eArray[index]);
            }
        }//ExitCode

        private static bool ArgsCheck(string testString, string argumentString)
        {
            if (String.IsNullOrWhiteSpace(testString))
            {
                eList.Add(ErrorCode.BadArguments);
                throw new ArgumentNullException("testString: {" + testString + "}");
            }
            else if (String.IsNullOrWhiteSpace(argumentString))
            {
                eList.Add(ErrorCode.BadArguments);
                throw new ArgumentNullException("argumentString: {" + argumentString + "}");
            }
            else
            {
                testString = testString.ToLowerInvariant();
                argumentString = argumentString.ToLowerInvariant();

                if (testString == $"--{argumentString}" || testString == $"-{argumentString[0]}")
                {
                    return true;
                }//if
                else
                    return false;
            }//else
        }//ArgsCheck()
    }//TimeFromSeconds
}//namespace
