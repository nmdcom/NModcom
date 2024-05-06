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

namespace NModcom
{
    /// <summary>
    /// TimeEvents are scheduled in a time event queue and are fired by the simulation environment.
    /// </summary>
    public class TimeEvent : SimEvent, ITimeEvent
    {
        private double eventTime;
        public TimeEvent()
        {

        }
        /// <summary>
        /// The constructor of this class.
        /// </summary>
        /// <param name="source">The source simobj.</param>
        /// <param name="target">The target simobj.</param>
        /// <param name="priority">Priority of the event.</param>
        /// <param name="message">A message.</param>
        /// <param name="eventTime">Time of the event.</param>
        public TimeEvent(ISimObj source, ISimObj target, int priority, int message, double eventTime) :
            base(source, target, priority, message)
        {
            this.eventTime = eventTime;
        }

        /// <summary>
        /// Time of the event.
        /// </summary>
        public double EventTime
        {
            get { return eventTime; }
            set { eventTime = value; }
        }
    }

    /// <summary>
    /// A Recurring time event, reschedules itself with a specified interval.
    /// </summary>
    public class RecurringTimeEvent : TimeEvent, IRecurringTimeEvent
    {
        private double interval;

        /// <summary>
        /// The constructor of this class.
        /// </summary>
        /// <param name="source">The source simobj.</param>
        /// <param name="target">The target simobj.</param>
        /// <param name="priority">Priority of the event.</param>
        /// <param name="message">A message.</param>
        /// <param name="eventTime">Time of the event.</param>
        /// <param name="interval">Time interval between two occurrences of the event.</param>
        public RecurringTimeEvent(ISimObj source, ISimObj target, int priority, int message, double eventTime, double interval) :
            base(source, target, priority, message, eventTime)
        {
            Interval = interval;
        }

        //UITLEG
        /// <summary>
        /// Interval between two successive (recurring) time events.
        /// </summary>
        public double Interval
        {
            get { return interval; }
            set
            {
                if (value <= 0)
                    throw new Exception("Recurring event intervals must be > 0");

                interval = value;
            }
        }

        /// <summary>
        /// Handles the event and reschedules it .
        /// </summary>
        public override void HandleEvent()
        {
            base.HandleEvent();

            if (!Canceled)
            {
                EventTime += interval;
                SimEnv.RegisterEvent(this);
            }
        }
    }


    /// <summary>
    /// A Recurring time event, reschedules itself based on the TimeStep property
    /// of the IUpdateble interface
    /// </summary>
    public class SystemRecurringTimeEvent : TimeEvent
    {
        /// <summary>
        /// The constructor of this class.
        /// </summary>
        /// <param name="source">The source simobj.</param>
        /// <param name="target">The target simobj.</param>
        /// <param name="priority">Priority of the event.</param>
        /// <param name="message">A message.</param>
        /// <param name="eventTime">Time of the event.</param>
        public SystemRecurringTimeEvent(ISimObj source, ISimObj target, int priority, int message, double eventTime) :
            base(source, target, priority, message, eventTime)
        {
        }

        /// <summary>
        /// Handles the event and reschedules it 
        /// </summary>
        public override void HandleEvent()
        {
            base.HandleEvent();

            IDiscreteModel upd = Target as IDiscreteModel;
            if (upd == null)
                throw new Exception("Target does not implement IUpdateble");

            double timeStep = upd.TimeStep;
            if (timeStep > 0)
            {
                EventTime += timeStep;

                if (!Canceled)
                    SimEnv.RegisterEvent(this);
            }
        }
    }
}
