using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace TFS
{
    class TimeFromSeconds
    {
        //Enumerations

        //Defines Error Codes
        public enum ErrorCode
        {
            Success = 0,
            FileNotFound = 2,
            InvalidData = 13,
            BadArguments = 160,
            ArithmeticOverflow = 534,
            InvalidIndex = 1413
        }//enum ErrorCode

        //Defines Units used
        public enum Unit
        {
            Seconds,
            Minutes,
            Hours,
            Days,
            Weeks,
            Years,
            Decades,
            Centuries,
            Millennia
        }//enum Unit



        //Assignment of CONSTANTs and readonly variables
        private const string DEV = "Chris Judkins";
        private static readonly decimal MILLENNIUM = 31557600000;
        private static readonly Decimal CENTURY = 3155760000;
        private static readonly Decimal DECADE = 315576000;
        private static readonly Decimal YEAR = 31557600;
        private static readonly Decimal WEEK = 604800;
        private static readonly Decimal DAY = 86400;
        private static readonly Decimal HOUR = 3600;
        private static readonly Decimal MINUTE = 60;
        private static readonly decimal DECISECOND = .1M;
        private static readonly decimal CENTISECOND = .01M;
        private static readonly decimal MILLISECOND = .001M;
        private static readonly decimal MYRIOSECOND = 10e-4M;
        private static readonly decimal MICROSECOND = 10e-6M;
        private static readonly decimal NANOSECOND = 10e-9M;
        private static readonly decimal PICOSECOND = 10e-12M;
        private static readonly decimal FEMTOSECOND = 10e-15M;



        //Asignment of variables
        private static bool debugMode;
        private decimal input;
        private static List<ErrorCode> eList = new List<ErrorCode>();



        //Properties to access variables
        public decimal Input
        {
            get { return this.input; }
            set
            {
                if (value >= 0)
                    this.input = value;
            }//set
        }//Input

        public static bool DebugMode
        {
            get
            {
                return debugMode;
            }//get
            set
            {
                debugMode = value;
            }//set
        }//DebugMode



        //Auto-implemented Properties
        public Unit TFSUnit { get; set; }



        //Constructors
        public TimeFromSeconds(Unit aUnit, decimal aInput)
        {
            TFSUnit = aUnit;
            Input = aInput;
        }//TimeFromSeconds()



        //Methods

        //ToString() allows an object of the class to be called where a string input is expected (i.e. Console.WriteLine(aTFS);)
        public override string ToString()
        {
            try
            {
                return FromSeconds();
            }//try
            catch (OverflowException e)
            {
                eList.Add(ErrorCode.ArithmeticOverflow);
                Error(e);
                return e.Message;
            }//catch overflow
            catch (Exception e)
            {
                Error(e);
                return e.Message;
            }//catch
        }//ToString()

        //Primary method for performing conversion
        public string FromSeconds()
        {
            //Dec and Init local vars
            uint millennia = 0;
            uint centuries = 0;
            uint decades = 0;
            uint years = 0;
            uint weeks = 0;
            uint days = 0;
            uint hours = 0;
            uint minutes = 0;
            uint seconds = 0;
            uint deciseconds = 0;
            uint centiseconds = 0;
            uint milliseconds = 0;
            uint myrioseconds = 0;
            uint microseconds = 0;
            uint nanoseconds = 0;
            uint picoseconds = 0;
            uint femtoseconds = 0;
            uint temp = 0;
            decimal inVar = this.Input;
            decimal original = inVar;
            decimal originalSeconds = 0;
            StringBuilder sb = new StringBuilder();
            //Covert from input unit type to seconds
            if (TFSUnit == Unit.Minutes)
            {
                inVar *= MINUTE;
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

            else if (TFSUnit == Unit.Years)
            {
                inVar *= YEAR;
            }//if years

            else if (TFSUnit == Unit.Decades)
            {
                inVar *= DECADE;
            }//if decades

            else if (TFSUnit == Unit.Centuries)
            {
                inVar *= CENTURY;
            }//if centuries

            else if (TFSUnit == Unit.Millennia)
            {
                inVar *= MILLENNIUM;
            }//if Millennia

            //Check if input is negative
            if (this.Input < 0)
            {
                eList.Add(ErrorCode.BadArguments);
                throw new ArgumentOutOfRangeException("Time cannot be negative. Input was: " + Input);
            }
            else
            {
                originalSeconds = inVar;
                sb.Append($"{original} {TFSUnit} is:");
                while (inVar >= FEMTOSECOND)
                {
                    MyDebugger($"Debug: original = {original}\nTFSUnit = {TFSUnit}\noriginalSeconds = {originalSeconds}\ninVar = {inVar}\n" +
                    $"millennia = {millennia}\ncenturies = {centuries}\ndecades = {decades}\nyears = {years}\nweeks = {weeks}\ndays = {days}\n" +
                    $"hours = {hours}\nminutes = {minutes}\nseconds = {seconds}\n" +
                    $"deciseconds = {deciseconds}\ncentiseconds = {centiseconds}\nmilliseconds = {milliseconds}\n" +
                    $"myrioseconds = {myrioseconds}\nmicroseconds = {microseconds}\nnanoseconds = {nanoseconds}\npicoseconds = {picoseconds}\nfemtoseconds = {femtoseconds}");

                    // Break down seconds starting with largest unit and moving down to smallest unit until inVar < FEMTOSECOND
                    if (inVar >= MILLENNIUM && ((TFSUnit != Unit.Millennia) || (originalSeconds % MILLENNIUM != 0)))
                    {
                        temp = (uint) Math.Truncate(inVar / MILLENNIUM);
                        millennia += temp;
                        inVar -= (MILLENNIUM * temp);
                        sb.Append($" {millennia} millennia,");
                    }//Millennia

                    else if (inVar >= CENTURY && ((TFSUnit != Unit.Centuries) || (originalSeconds % CENTURY != 0)))
                    {
                        temp = (uint)Math.Truncate(inVar / CENTURY);
                        centuries += temp;
                        inVar -= (CENTURY * temp);
                        sb.Append($" {centuries} centuries,");
                    }//centuries

                    else if (inVar >= DECADE && ((TFSUnit != Unit.Decades) || (originalSeconds % DECADE != 0)))
                    {
                        temp = (uint)Math.Truncate(inVar / DECADE);
                        decades += temp;
                        inVar -= (DECADE * temp);
                        sb.Append($" {decades} decades,");
                    }//decades

                    else if (inVar >= YEAR && ((TFSUnit != Unit.Years) || (originalSeconds % YEAR != 0)))
                    {
                        temp = (uint)Math.Truncate(inVar / YEAR);
                        years += temp;
                        inVar -= (YEAR * temp);
                        sb.Append($" {years} years,");
                    }//years

                    else if (inVar >= WEEK && ((TFSUnit != Unit.Weeks) || (originalSeconds % WEEK != 0)))
                    {
                        temp = (uint)Math.Truncate(inVar / WEEK);
                        weeks += temp;
                        inVar -= (WEEK * temp);
                        sb.Append($" {weeks} weeks,");
                    }//weeks

                    else if (inVar >= DAY && ((TFSUnit != Unit.Days) || (originalSeconds % DAY != 0)))
                    {
                        temp = (uint)Math.Truncate(inVar / DAY);
                        days += temp;
                        inVar -= (DAY * temp);
                        sb.Append($" {days} days,");
                    }//days

                    else if (inVar >= HOUR && ((TFSUnit != Unit.Hours) || (originalSeconds % HOUR != 0)))
                    {
                        temp = (uint)Math.Truncate(inVar / HOUR);
                        hours += temp;
                        inVar -= (HOUR * temp);
                        sb.Append($" {hours} hours,");
                    }//hours

                    else if (inVar >= MINUTE && ((TFSUnit != Unit.Minutes) || (originalSeconds % MINUTE != 0)))
                    {
                        temp = (uint)Math.Truncate(inVar / MINUTE);
                        minutes += temp;
                        inVar -= (MINUTE * temp);
                        sb.Append($" {minutes} minutes,");
                    }

                    else if (inVar >= 1)
                    {
                        seconds = (uint)Math.Truncate(inVar);
                        inVar -= seconds;
                        sb.Append($" {seconds} seconds,");
                    }//seconds

                    else if (inVar >= DECISECOND)
                    {
                        temp = (uint)Math.Truncate(inVar / DECISECOND);
                        deciseconds += temp;
                        inVar -= (DECISECOND * temp);
                        sb.Append($" {deciseconds} deciseconds,");
                    }//deciseconds

                    else if (inVar >= CENTISECOND)
                    {
                        temp = (uint)Math.Truncate(inVar / CENTISECOND);
                        centiseconds += temp;
                        inVar -= (CENTISECOND * temp);
                        sb.Append($" {centiseconds} centiseconds,");
                    }//centiseconds

                    else if (inVar >= MILLISECOND)
                    {
                        temp = (uint)Math.Truncate(inVar / MILLISECOND);
                        milliseconds += temp;
                        inVar -= (MILLISECOND * temp);
                        sb.Append($" {milliseconds} milliseconds,");
                    }//milliseconds

                    else if (inVar >= MYRIOSECOND)
                    {
                        temp = (uint)Math.Truncate(inVar / MYRIOSECOND);
                        myrioseconds += temp;
                        inVar -= (MYRIOSECOND * temp);
                        sb.Append($" {myrioseconds} myrioseconds,");
                    }//myrioseconds

                    else if (inVar >= MICROSECOND)
                    {
                        temp = (uint)Math.Truncate(inVar / MICROSECOND);
                        microseconds += temp;
                        inVar -= (MICROSECOND * temp);
                        sb.Append($" {microseconds} microseconds,");
                    }//microseconds

                    else if (inVar >= NANOSECOND)
                    {
                        temp = (uint)Math.Truncate(inVar / NANOSECOND);
                        nanoseconds += temp;
                        inVar -= (NANOSECOND * temp);
                        sb.Append($" {nanoseconds} nanoseconds,");
                    }//nanoseconds

                    else if (inVar >= PICOSECOND)
                    {
                        temp = (uint)Math.Truncate(inVar / PICOSECOND);
                        picoseconds += temp;
                        inVar -= (PICOSECOND * temp);
                        sb.Append($" {picoseconds} picoseconds,");
                    }//picoseconds

                    else if (inVar >= FEMTOSECOND)
                    {
                        temp = (uint)Math.Truncate(inVar / FEMTOSECOND);
                        femtoseconds += temp;
                        inVar -= (FEMTOSECOND * temp);
                        sb.Append($" {femtoseconds} femtoseconds,");
                    }//femtoseconds
                }//while inVar larger than smallest unit
                MyDebugger($"Debug: original = {original}\nTFSUnit = {TFSUnit}\noriginalSeconds = {originalSeconds}\ninVar = {inVar}\n" +
                    $"millennia = {millennia}\ncenturies = {centuries}\ndecades = {decades}\nyears = {years}\nweeks = {weeks}\ndays = {days}\n" +
                    $"hours = {hours}\nminutes = {minutes}\nseconds = {seconds}\n" +
                    $"deciseconds = {deciseconds}\ncentiseconds = {centiseconds}\nmilliseconds = {milliseconds}\n" +
                    $"myrioseconds = {myrioseconds}\nmicroseconds = {microseconds}\nnanoseconds = {nanoseconds}\npicoseconds = {picoseconds}\nfemtoseconds = {femtoseconds}");

                //returns a string using string builder to only include units with non-zero values
                sb.Remove(sb.Length - 1, 1); //Remove the trailing ','
                return sb.ToString(); 
            }//else
        }//FromSeconds()


       
        public static void Main(string[] args)
        {
            int option = -1;
            decimal input = -1;
            if (args != null)
            {
                foreach (string arg in args)
                {
                    if(ArgsCheck(arg, "debug"))
                    {
                        DebugMode = true;
                    }//if
                }//foreach
            }//if
            else
            {
                DebugMode = false;
            }//default values
            MyDebugger("Debug: Debugging is on!");


            const int OPTIONCOUNT1 = 6;
            while(!(option < OPTIONCOUNT1 && (option >= 0)))
            {
                Console.WriteLine($"Welcome to the seconds to human readable time converter!\n\nThis program was produced by {DEV}.");
                Console.WriteLine("\n0) Seconds\n1) Minutes\n2) Hours\n3) Days\n4) Weeks\n5) Years\n");
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
                                    if (decimal.TryParse(i, out input))
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
                                }//else i is 'q', so exit
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
                }//else o is 'q', so exit
            }//while option not valid
        }//Main()




        private static void MyDebugger(string m)
        {
            if (DebugMode)
            {
                Console.WriteLine("\n\n---------------debug---------------\n");
                Console.WriteLine(m);
                Console.WriteLine("\n-------------end debug-------------\n\n");
            }//if
        }//Error(String)

        private static void Error(string m, Exception e)
        {
            MyDebugger($"{m}\nError:\n\nData: {e.Data}\nMessage: {e.Message}\nSource: {e.Source}");
            Console.WriteLine("\nSomething went wrong!\n\nError: " + e.Message);
        }//Error(String, Exception)

        private static void Error(Exception e)
        {
            MyDebugger($"Error:\n\nData: {e.Data}\nMessage: {e.Message}\nSource: {e.Source}");
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
                int index = eArray.Length - 1;
                StringBuilder sb = new StringBuilder();
                foreach (ErrorCode e in eArray)
                    sb.Append($"\nError Code: {e}");
                Trace.WriteLine(sb.ToString());
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
