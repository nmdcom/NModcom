/*
 * ==============================================================================
 * NMODCOM: software for component-based simulation
 * 
 * MIT License
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * ================================================================================
*/

using System;

namespace NModcom
{

    /// <summary>
    /// CalendarTime interprets the CurrentTime field of its SimEnv as follows:
    /// 1 unit is one day; multiply CurrentTime by TICKSPERDAY to get the value
    /// of the Ticks field of the corresponding .NET DateTime value. 
    /// </summary>
    public class CalendarTime : ITime
    {
        private ISimEnv simenv;
        private short rotationLength;

        /// <summary>
        /// Constructs a new CalendarTime and associates it with a <see cref="ISimEnv"/>.
        /// </summary>
        /// <param name="simenv">The ISimEnv with which this instance will be associated.</param>
        public CalendarTime(ISimEnv simenv)
        {
            this.simenv = simenv;
        }

        public override string ToString()
        {
            return CurrentDate.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// See <see cref="ITime.CurrentYear"/>.
        /// </summary>
        public virtual int CurrentYear
        {
            get { return CurrentDate.Year; }
        }

        /// <summary>
        /// See <see cref="ITime.CurrentMonth"/>.
        /// </summary>
        public virtual int CurrentMonth
        {
            get { return CurrentDate.Month; }
        }

        /// <summary>
        /// See <see cref="ITime.CurrentDay"/>.
        /// </summary>
        public virtual int CurrentDay
        {
            get { return CurrentDate.Day; }
        }

        private void GetYMD(double dt, out int year, out int month, out int day)
        {
            DateTime datetime = ToDateTime(dt);
            year = datetime.Year;
            month = datetime.Month;
            day = datetime.Day;
        }

        /// <summary>
        /// See <see cref="ITime.GetCurrentYMD"/>.
        /// </summary>
        /// <param name="year">The year (e.g. 2005) of the current time.</param>
        /// <param name="month">The month (1..12) of the current time.</param>
        /// <param name="day">The day (1..31) of the current time.</param>
        public void GetCurrentYMD(out int year, out int month, out int day)
        {
            GetYMD(simenv.CurrentTime, out year, out month, out day);
        }

        /// <summary>
        /// See <see cref="ITime.GetStartYMD"/>.
        /// </summary>
        /// <param name="year">The year (e.g. 2005) of the start time.</param>
        /// <param name="month">The month (1..12) of the start time.</param>
        /// <param name="day">The day (1..31) of the start time.</param>
        public void GetStartYMD(out int year, out int month, out int day)
        {
            GetYMD(simenv.StartTime, out year, out month, out day);
        }

        /// <summary>
        /// See <see cref="ITime.SetStartYMD"/>.
        /// </summary>
        /// <param name="year">The year (e.g. 2005) of the start time.</param>
        /// <param name="month">The month (1..12) of the start time.</param>
        /// <param name="day">The day (1..31) of the start time.</param>
        public void SetStartYMD(int year, int month, int day)
        {
            simenv.StartTime = ToDouble(year, month, day);
        }

        /// <summary>
        /// Implements the member of <see cref="ITime"/> with the same name.
        /// </summary>
        /// <param name="year">The year (e.g. 2005) of the stop time.</param>
        /// <param name="month">The month (1..12) of the stop time.</param>
        /// <param name="day">The day (1..31) of the stop time.</param>
        public void GetStopYMD(out int year, out int month, out int day)
        {
            GetYMD(simenv.StopTime, out year, out month, out day);
        }

        /// <summary>
        /// Implements the member of <see cref="ITime"/> with the same name.
        /// </summary>
        /// <param name="year">The year (e.g. 2005) of the stop time.</param>
        /// <param name="month">The month (1..12) of the stop time.</param>
        /// <param name="day">The day (1..31) of the stop time.</param>
        public void SetStopYMD(int year, int month, int day)
        {
            simenv.StopTime = ToDouble(year, month, day);
        }

        /// <summary>
        /// Implements the member of <see cref="ITime"/> with the same name.
        /// </summary>
        public DateTime CurrentDate
        {
            get { return ToDateTime(simenv.CurrentTime); }
        }

        /// <summary>
        /// Implements the member of <see cref="ITime"/> with the same name.
        /// </summary>
        public DateTime StartDate
        {
            get { return ToDateTime(simenv.StartTime); }
            set { simenv.StartTime = ToDouble(value); }
        }

        /// <summary>
        /// Implements the member of <see cref="ITime"/> with the same name.
        /// </summary>
        public DateTime StopDate
        {
            get { return ToDateTime(simenv.StopTime); }
            set { simenv.StopTime = ToDouble(value); }
        }

        /// <summary>
        /// Implements the member of <see cref="ITime"/> with the same name.
        /// </summary>
        public short RotationLength
        {
            get { return rotationLength; }
            set { rotationLength = value; }
        }

        /// <summary>
        /// Implements the member of <see cref="ITime"/> with the same name.
        /// </summary>

        // The table below explains the logic behind the implementation of CurrentCycle and
        // YearOfRotation.
        //
        // year  cycle  year-of-rotation  (year-startyear)  (year-startyear) div length   remainder
        // ====  =====  ================   ==============    ==========================   =========
        // 1962    1         1                   0                      0                     0
        // 1962    1         1                   1                      0                     1
        // 1963    1         1                   2                      0                     2
        // 1964    1         2                   0                      1                     0
        // 1965    1         2                   1                      1                     1
        // 1966    1         2                   2                      1                     2
        // 1967    1         3                   0                      2                     0
        // 1968    1         3                   1                      2                     1
        // 
        //
        public short CurrentCycle
        {
            get
            {
                int startyear, startmonth, startday;
                int currentyear, currentmonth, currentday;
                GetStartYMD(out startyear, out startmonth, out startday);
                GetCurrentYMD(out currentyear, out currentmonth, out currentday);
                int r;
                return Convert.ToInt16(1 + Math.DivRem(currentyear - startyear, rotationLength, out r));
            }
        }

        /// <summary>
        /// Implements the member of <see cref="ITime"/> with the same name.
        /// </summary>
        public short YearOfRotation
        {
            get
            {
                int startyear, startmonth, startday;
                int currentyear, currentmonth, currentday;
                GetStartYMD(out startyear, out startmonth, out startday);
                GetCurrentYMD(out currentyear, out currentmonth, out currentday);
                int r;
                Math.DivRem(currentyear - startyear, rotationLength, out r);
                return Convert.ToInt16(1 + r);
            }
        }

        // The meat of this class follows below.
        /// <summary>
        /// Returns datetime as the Ticks field of the corresponding .NET DateTime value.   .
        /// </summary>
        /// <param name="d">"Double representation of datetime."</param>
        /// <returns>"Datetime as value of Ticks field."</returns>
        public static DateTime ToDateTime(double d)
        {
            return new DateTime((long)(d * TICKSPERDAY));
        }

        /// <summary>
        /// Returns datetime as a DateTime.
        /// </summary>
        /// <param name="t">Datetime represented as a double.</param>
        /// <returns>Datetime as a DateTime.</returns>
        public object FromDouble(double t)
        {
            return ToDateTime(t);
        }

        /// <summary>
        /// Returns time as a double.
        /// </summary>
        /// <param name="d">Time as a DateTime.</param>
        /// <returns>Time as a double.</returns>
        public static double ToDouble(DateTime d)
        {
            return (double) d.Ticks / TICKSPERDAY;
        }

        /// <summary>
        /// Returns time as a double.
        /// </summary>
        /// <param name="t">Time as an object.</param>
        /// <returns>Time as a double.</returns>
        public double ToDouble(object t)
        {
            DateTime dateTime = DateTime.Parse(t.ToString());
            return ToDouble(dateTime);
        }

        /// <summary>
        /// Returns time as a double.
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <param name="day">Day</param>
        /// <returns>Datetime as a double.</returns>
        public static double ToDouble(int year, int month, int day)
        {
            DateTime dt = new DateTime(year, month, day);
            return ToDouble(dt);
        }

        private const long TICKSPERDAY = 864000000000;

    }

}
