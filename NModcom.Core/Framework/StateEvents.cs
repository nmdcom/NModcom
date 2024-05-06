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
    /// A sorted list of StateEvents.
    /// StateEvents are sorted by Priority.
    /// </summary>
    public interface IStateEvents
    {
        /// <summary>
        /// Adds an event to the list.
        /// </summary>
        /// <param name="stateEvent">Time event to add.</param>
        void AddEvent(IStateEvent stateEvent);


        /// <summary>
        /// Checks whether any state event occurred
        /// </summary>
        /// <returns>True if a state event occurred</returns>
        bool HasEvent();


        /// <summary>
        /// Checks whether a state event occurred, if it does the event is removed
        /// from the queue and returned to the caller.
        /// Note: there might be more state events, but only the first is returned.
        /// The simulation environment handles state events by repeatedly calling 
        /// GetEvent() until it returns null.
        /// </summary>
        /// <returns>The occurred state event or nil.</returns>
        IStateEvent GetEvent();


        /// <summary>
        /// CLears the list, removes all events.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes a specific event. This method is called by the simulation environment
        /// when a Simulation object is removed.
        /// </summary>
        /// <param name="stateEvent">StateEvent</param>
        void RemoveEvent(IStateEvent stateEvent);

        /// <summary>
        /// Removes all events that reference this object.
        /// </summary>
        /// <param name="simObj">Simulation object</param>
        void RemoveObject(ISimObj simObj);
    }

}
