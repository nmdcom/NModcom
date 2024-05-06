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

namespace NModcom
{
    // Why call an Interface an abstract base class??
    /// <summary>
    /// Abstract base class for Simulation Events.
    /// </summary>
    public interface ISimEvent
    {
        //UITLEG
        /// <summary>
        /// The simulation environment in which the event takes place.
        /// </summary>
        ISimEnv SimEnv
        {
            get;
            set;
        }

        /// <summary>
        /// The source simobj.
        /// </summary>
        ISimObj Source
        {
            get;
        }

        /// <summary>
        /// The target simobj.
        /// </summary>
        ISimObj Target
        {
            get;
        }

        /// <summary>
        /// Events are handled by priority with higher priorities first.
        /// When two time events are schedued at the same time, the one with the highest
        /// priority is handled first.
        /// When they have the same time and the same priority, the first that was
        /// scheduled will be handled first.
        /// </summary>
        int Priority
        {
            get;
        }

        /// <summary>
        /// A possible message for the target object. 
        /// NOTE: THIS PROPERTY WILL VERY LIKELY BE REMOVED AFTER JUNE 2006.
        /// </summary>
        int Message
        {
            get;
        }

        /// <summary>
        /// When this flag is True, the event has been canceled and should not be rescheduled.
        /// </summary>
        bool Canceled
        {
            get;
        }

        //UITLEG
        /// <summary>
        /// The event is being handled.
        /// </summary>
        void HandleEvent();

        /// <summary>
        /// Calling Cancel() when the event is handled will stop it from rescheduling.
        /// </summary>
        void Cancel();
    }


}
