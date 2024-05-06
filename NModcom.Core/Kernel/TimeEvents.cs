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

using NModcom.Framework;
using System;
using System.Collections;

namespace NModcom
{
    /// <summary>
    /// Implements a sorted list of TimeEvents.
    /// TimeEvents are sorted by EventTime and Priority.
    /// </summary>
    public class TimeEvents : ITimeEvents, IEnumerable
    {
        /// <summary>
        /// Node type for the Linked List
        /// </summary>
        private class Node
        {
            public ITimeEvent timeEvent;
            public Node next;
        }

        private class TimeEventEnumerator : IEnumerator
        {
            private bool start;
            private Node first;
            private Node current;

            public TimeEventEnumerator(Node first)
            {
                this.first = first;
                Reset();
            }

            #region IEnumerator Members

            public void Reset()
            {
                current = null;
                start = true;
            }

            public object Current
            {
                get
                {
                    if (current == null)
                        throw new InvalidOperationException("No current TimeEvent");
                    return current.timeEvent;
                }
            }

            public bool MoveNext()
            {
                if (start)
                {
                    current = first;
                    start = false;
                }
                else if (current != null)
                    current = current.next;

                return current != null;
            }

            #endregion

        }


        /// <summary>
        /// First node
        /// </summary>
        private Node first = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public TimeEvents()
        {
        }

        /// <summary>
        /// Adds an event to the list.
        /// </summary>
        /// <param name="timeEvent">Time event to add.</param>
        public void AddEvent(ITimeEvent timeEvent)
        {
            Node before = null;
            Node current = first;

            // Locate place to insert, behind entries
            // whith lower or same When with higher priority.
            while ((current != null) && ((current.timeEvent.EventTime < timeEvent.EventTime) ||
                                          ((current.timeEvent.EventTime == timeEvent.EventTime) &&
                                           (current.timeEvent.Priority >= timeEvent.Priority))))
            {
                before = current;
                current = current.next;
            }

            Node node = new Node();
            node.timeEvent = timeEvent;

            node.next = current;
            if (before != null)
                before.next = node;
            else
                first = node;
        }

        /// <summary>
        /// Returns the first event in the list. The event is removed.
        /// </summary>
        /// <returns>An event. Null if the list is empty.</returns>
        public ITimeEvent RemoveFirst()
        {
            if (first == null)
                return null;

            Node node = first;
            first = node.next;

            return node.timeEvent;
        }

        /// <summary>
        /// returns the first event in the list, but leaves it in the list.
        /// </summary>
        /// <returns>First event, null if the list is empty.</returns>
        public ITimeEvent GetFirst()
        {
            if (first != null)
                return first.timeEvent;
            else
                return null;
        }

        /// <summary>
        /// CLears the list, removes all events.
        /// </summary>
        public void Clear()
        {
            // Just set first to null, garbage collector will do the rest
            first = null;
        }

        /// <summary>
        /// Removes a specific event. This method is called by the simulation environment
        /// when a Simulation object is removed.
        /// </summary>
        /// <param name="timeEvent"></param>
        public void RemoveEvent(ITimeEvent timeEvent)
        {
            Node before = null;
            Node current = first;

            while (current != null)
            {
                if (current.timeEvent == timeEvent)
                {
                    if (before != null)
                        before.next = current.next;
                    else
                        first = current.next;

                    break;
                }

                before = current;
                current = current.next;
            }
        }


        /// <summary>
        /// Removes all events that reference this object.
        /// </summary>
        /// <param name="simObj">Simulation object</param>
        public void RemoveObject(ISimObj simObj)
        {
            Node before = null;
            Node current = first;

            while (current != null)
            {
                if (current.timeEvent.Source == simObj ||
                     current.timeEvent.Target == simObj)
                {
                    if (before != null)
                        before.next = current.next;
                    else
                        first = current.next;

                    break;
                }

                before = current;
                current = current.next;
            }
        }

        #region Implementation of IEnumerable
        public IEnumerator GetEnumerator()
        {
            return new TimeEventEnumerator(first);
        }
        #endregion
    }
}
