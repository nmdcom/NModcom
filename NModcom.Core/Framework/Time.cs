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
    /// Interface for classes that provide a custom-interpretation
    /// for simulation time.
    /// Simulation time (and related values, such as StartTime and StopTime) are 
    /// stored as double precision numbers. The framework does not depend on a
    /// specific interpretation of these numbers. Time classes provide a mapping
    /// from these double's to some real-world time-scale such as year-month-day.
    /// </summary>
    public interface ITime
    {
        /// <summary>
        /// Year of the current time, e.g. 2006.
        /// </summary>
        int CurrentYear { get; }

        /// <summary>
        /// Month of the current time, 1..12
        /// </summary>
        int CurrentMonth { get; }

        /// <summary>
        /// Day-of-month of the current time, 1..31
        /// </summary>
        int CurrentDay { get; }

        /// <summary>
        /// Get year (e.g. 2006), month (1..12), and day (1..31) of the current time.
        /// </summary>
        void GetCurrentYMD(out int year, out int month, out int day);

        /// <summary>
        /// Get year (e.g. 2006), month (1..12), and day (1..31) of the start time
        /// of the simulation.
        /// </summary>
        void GetStartYMD(out int year, out int month, out int day);

        /// <summary>
        /// Set year (e.g. 2006), month (1..12), and day (1..31) of the start time 
        /// of the simulation.
        /// </summary>
        void SetStartYMD(int year, int month, int day);

        /// <summary>
        /// Get year (e.g. 2006), month (1..12), and day (1..31) of the stop time 
        /// of the simulation.
        /// </summary>
        void GetStopYMD(out int year, out int month, out int day);

        /// <summary>
        /// Set year (e.g. 2006), month (1..12), and day (1..31) of the stop time 
        /// of the simulation.
        /// </summary>
        void SetStopYMD(int year, int month, int day);

        /// <summary>
        /// Get the current time as a <see cref="DateTime"/>.
        /// </summary>
        DateTime CurrentDate { get; }

        /// <summary>
        /// Get and set the start time as a <see cref="DateTime"/>.
        /// </summary>
        DateTime StartDate { get; set; }

        /// <summary>
        /// Get and set the stop time as a <see cref="DateTime"/>.
        /// </summary>
        DateTime StopDate { get; set; }

        /// <summary>
        /// Length of the rotation in years. A rotation is a sequence of crops that 
        /// are grown at a particular location. 
        /// </summary>
        /// <remarks>In the current implementation, we assume 
        /// that a rotation has a fixed length, e.g. two or three years. The first year 
        /// of the rotation is the calendar year in which the simulation starts.
        /// </remarks>
        short RotationLength { get; set; }

        /// <summary>
        /// Current cycle of the rotation (1..n).
        /// </summary>
        short CurrentCycle { get; }

        /// <summary>
        /// Year in the current cycle (1..n).
        /// </summary>
        short YearOfRotation { get; }

        //UITLEG is beschrijving bij param en returns OK??
        /// <summary>
        /// Utility method used by several other methods, but can also be used by
        /// other classes. This method attempts to interpret the argument t as 
        /// a moment in time and returns the double value that represents that moment. 
        /// Method <see cref="FromDouble"/> acts in the opposite direction.
        /// </summary>
        /// <param name="t">An object that can be represented as a moment in time.</param>
        /// <returns>Time represented as a double.</returns>
        double ToDouble(object t);

        //UITLEG is beschrijving bij param en returns OK??
        /// <summary>
        /// See <see cref="ToDouble"/>.
        /// </summary>
        /// <param name="t">Time represented as a double.</param>
        /// <returns>An object that can be represented as a moment in time.</returns>
        object FromDouble(double t);
    }

}
