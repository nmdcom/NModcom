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
    /// Implements a sorted list of StateEvents.
    /// StateEvents are sorted by Priority.
    /// </summary>
    public class StateEvents : IStateEvents, IEnumerable
    {
        /// <summary>
        /// Node type for the Linked List
        /// </summary>
        private class Node
        {
            public IStateEvent StateEvent;
            public Node next;
        }

        private class StateEventEnumerator : IEnumerator
        {
            private bool start;
            private Node first;
            private Node current;

            public StateEventEnumerator(Node first)
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
                        throw new InvalidOperationException("No current StateEvent");
                    return current.StateEvent;
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
        /// Owner
        /// </summary>
        private SimEnv simEnv;

        /// <summary>
        /// Constructor
        /// </summary>
        public StateEvents(SimEnv simEnv)
        {
            this.simEnv = simEnv;
        }

        /// <summary>
        /// Adds an event to the list.
        /// </summary>
        /// <param name="StateEvent">Time event to add.</param>
        public void AddEvent(IStateEvent StateEvent)
        {
            Node before = null;
            Node current = first;

            // Locate place to insert, behind entries
            // whith lower or same When with higher priority.
            while ((current != null) && (current.StateEvent.Priority >= StateEvent.Priority))
            {
                before = current;
                current = current.next;
            }

            Node node = new Node();
            node.StateEvent = StateEvent;

            node.next = current;
            if (before != null)
                before.next = node;
            else
                first = node;
        }


        /// <summary>
        /// Checks whether any state event occurred
        /// </summary>
        /// <returns>True if a state event occurred</returns>
        public bool HasEvent()
        {
            Node current = first;
            double time = simEnv.CurrentTime;

            while (current != null)
            {
                if (current.StateEvent.CheckState(time))
                    return true;

                current = current.next;
            }

            return false;
        }


        /// <summary>
        /// Checks whether a state event occurred, if it does the event is removed
        /// from the queue and returned to the caller.
        /// Note: there might be more state events, but only the first is returned.
        /// The simulation environment handles state events by repeatedly calling 
        /// GetEvent until it returns null.
        /// </summary>
        /// <returns>The occurred state event or nil</returns>
        public IStateEvent GetEvent()
        {
            Node before = null;
            Node current = first;
            double time = simEnv.CurrentTime;

            while (current != null)
            {
                if (current.StateEvent.CheckState(time))
                {
                    if (before != null)
                        before.next = current.next;
                    else
                        first = current.next;

                    return current.StateEvent;
                }

                before = current;
                current = current.next;
            }

            return null;
        }


        /// <summary>
        /// Clears the list, removes all events.
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
        /// <param name="StateEvent"></param>
        public void RemoveEvent(IStateEvent StateEvent)
        {
            Node before = null;
            Node current = first;

            while (current != null)
            {
                if (current.StateEvent == StateEvent)
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
                if (current.StateEvent.Source == simObj ||
                    current.StateEvent.Target == simObj)
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
            return new StateEventEnumerator(first);
        }
        #endregion
    }
}
