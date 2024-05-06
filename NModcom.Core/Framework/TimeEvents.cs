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

namespace NModcom.Framework
{
    /// <summary>
    /// Implements a sorted list of TimeEvents.
    /// TimeEvents are sorted by EventTime and Priority.
    /// </summary>
    public interface ITimeEvents
    {
        /// <summary>
        /// Adds an event to the list.
        /// </summary>
        /// <param name="timeEvent">Time event to add.</param>
        void AddEvent(ITimeEvent timeEvent);

        /// <summary>
        /// Returns the first event in the list. The event is removed.
        /// </summary>
        /// <returns>An event. Null if the list is empty.</returns>
        ITimeEvent RemoveFirst();

        /// <summary>
        /// Returns the first event in the list, but leaves it in the list.
        /// </summary>
        /// <returns>First event, null if the list is empty.</returns>
        ITimeEvent GetFirst();

        /// <summary>
        /// Clears the list, removes all events.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes a specific event. This method is called by the simulation environment
        /// when a Simulation object is removed.
        /// </summary>
        /// <param name="timeEvent">The time event.</param>
        void RemoveEvent(ITimeEvent timeEvent);

        /// <summary>
        /// Removes all events that reference this object.
        /// </summary>
        /// <param name="simObj">Simulation object</param>
        void RemoveObject(ISimObj simObj);
    }


}
