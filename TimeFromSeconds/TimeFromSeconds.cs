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
        private static bool debug;
        private double input;
        private double output;
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

        public double Output
        {
            get { return this.output; }
            set
            {
                if (value >= 0)
                    this.output = value;
            }//set
        }//Input

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

        public TimeFromSeconds(Unit aUnit, double aInput)
        {
            if (aUnit == Unit.Seconds)
            {
                Input = aInput;
                try
                {
                    Console.WriteLine(FromSeconds());
                }//try
                catch (Exception e)
                {
                    Error(e);
                }//catch
            }//if seconds

            else if (aUnit == Unit.Minutes)
            {
                Input = aInput;
                Input *= 60; //Convert to seconds
                try
                {
                    Console.WriteLine(FromSeconds());
                }//try
                catch (Exception e)
                {
                    Error(e);
                }//catch
            }//if minutes

            else if (aUnit == Unit.Hours)
            {
                Input = aInput;
                Input *= 3600;
                try
                {
                    Console.WriteLine(FromSeconds());
                }//try
                catch (Exception e)
                {
                    Error(e);
                }//catch
            }//if hours

            else if (aUnit == Unit.Days)
            {
                Input = aInput;
                Input *= 86400;
                try
                {
                    Console.WriteLine(FromSeconds());
                }//try
                catch (Exception e)
                {
                    Error(e);
                }//catch
            }//if days

            else if (aUnit == Unit.Weeks)
            {
                Input = aInput;
                Input *= 604800;
                try
                {
                    Console.WriteLine(FromSeconds());
                }//try
                catch (Exception e)
                {
                    Error(e);
                }//catch
            }//if hours

        }//TimeFromSeconds()

        public string FromSeconds()
        {
            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            double inVar = this.Input;
            double original = inVar;

            if (this.Input < 0)
            {
                eList.Add(ErrorCode.BadArguments);
                throw new ArgumentOutOfRangeException("Time cannot be negative. Input was: " + Input);
            }
            else
            {
                while (inVar >= 86400)
                {
                    days++;
                    inVar -= 86400;
                }//days

                while (inVar >= 3600)
                {
                    hours++;
                    inVar -= 3600;
                }//hours
                Debugger($"Debug: inVar = {inVar}, hours = {hours}, minutes = {minutes}, seconds = {seconds}");

                while (inVar >= 60)
                {
                    minutes++;
                    inVar -= 60;
                }//minutes
                Debugger($"Debug: inVar = {inVar}, hours = {hours}, minutes = {minutes}, seconds = {seconds}");

                while (inVar > 0)
                {
                    seconds++;
                    inVar--;
                }//seconds
                Debugger($"Debug: inVar = {inVar}, hours = {hours}, minutes = {minutes}, seconds = {seconds}");

                return $"{original} seconds is {hours} hours, {minutes} minutes, and {seconds} seconds";
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

            Console.WriteLine($"Welcome to the seconds to human readable time converter!\n\nThis program was produced by {DEV}.\n");

            const int OPTIONCOUNT1 = 5;
            while(!(option < OPTIONCOUNT1 && (option >= 0)))
            {
                Console.WriteLine("0) Seconds\n1) Minutes\n2) Hours\n3) Days\n4) Weeks\n");
                Console.Write("Select Input type: ");
                if (int.TryParse(Console.ReadLine(), out option))
                {
                    if ((option < OPTIONCOUNT1) && (option >= 0))
                    {
                        while (!(input > 0))
                        {
                            Console.Clear();
                            Console.Write($"Please enter a time in {(Unit)option}: ");
                            if (double.TryParse(Console.ReadLine(), out input))
                            {
                                if (input > 0)
                                {
                                    TimeFromSeconds aTFS = new TimeFromSeconds((Unit)option, input);
                                    Exit();
                                }//if input is positive
                            }//if input is parsed
                        }// while input not positive
                    }//if option is valid
                }//if option is parsed
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
                Console.WriteLine("Press any key to exit . . .");
                Console.ReadKey();
                Environment.Exit((int)ErrorCode.Success);
            }
            else
            {
                ErrorCode[] eArray = eList.ToArray();
                int index = eArray.Length;
                foreach (ErrorCode e in eArray)
                    Debugger($"Error Code: {e}");
                Console.WriteLine("Press any key to exit . . .");
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
