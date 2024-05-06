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
    /// Abstract base class for Simulation Events
    /// </summary>
    public abstract class SimEvent : ISimEvent
    {
        private ISimEnv simenv;
        private ISimObj source;
        private ISimObj target;
        private int priority;
        private int message;
        private bool canceled;


        protected SimEvent()
        {
        }

        protected SimEvent(ISimObj source, ISimObj target, int priority, int message)
        {
            this.source = source;
            this.target = target;
            this.priority = priority;
            this.message = message;
            canceled = false;
        }

        /// <summary>
        /// Sets or gets the simenv.
        /// </summary>
        public ISimEnv SimEnv
        {
            get { return simenv; }
            set { simenv = value; }
        }

        /// <summary>
        /// Gets the source simobj.
        /// </summary>
        public ISimObj Source
        {
            get { return source; }
        }

        /// <summary>
        /// Gets the target simobj.
        /// </summary>
        public ISimObj Target
        {
            get { return target; }
            set
            {
                if (target == null)
                    target = value;
                else
                    throw new Exception("target is not null and cannot be re-assigned");
            }
        }

        /// <summary>
        /// Events are handled by priority with higher priorities first.
        /// When two time events are schedued at the same time, the one with the highest
        /// priority is handled first.
        /// When they have the same time and the same priority, the first that was
        /// scheduled will be handled first.
        /// </summary>
        public int Priority
        {
            get { return priority; }
        }

        /// <summary>
        /// A possible message for the target object.
        /// NOTE: THIS PROPERTY WILL VERY LIKELY BE REMOVED AFTER JUNE 2006.
        /// </summary>
        public int Message
        {
            get { return message; }
        }

        /// <summary>
        /// When this flag is True, the event has been canceled and should not be rescheduled.
        /// </summary>
        public bool Canceled
        {
            get { return canceled; }
        }

        /// <summary>
        /// Handle the event. When there is no target an exception is raised.
        /// </summary>
        public virtual void HandleEvent()
        {
            if (target == null)
                throw new Exception("No target specified to handle event.");

            target.HandleEvent(this);
        }

        /// <summary>
        /// Calling cancel when the event is handled will stop it from rescheduling.
        /// </summary>
        public void Cancel()
        {
            canceled = true;
        }
    }


}
