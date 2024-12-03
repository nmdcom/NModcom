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
    /// Default implementation of ISimEnv.
    /// </summary>
    public class SimEnv : ISimEnv, IEnumerable
    {
        private string name;
        private double currentTime;
        private double startTime;
        private double stopTime;
        private double accuracy;
        private ArrayList simObjects;
        private SimulationStatus status;
        private TimeEvents timeEvents;
        private StateEvents stateEvents;
        private IIntegrator integrator;
        private ITime time;

        public SimEnv()
        {
            name = GetType().Name;
            status = SimulationStatus.Idle;
            simObjects = new ArrayList();
            timeEvents = new TimeEvents();
            stateEvents = new StateEvents(this);

            startTime = 0;
            stopTime = 5;
            accuracy = 0.01;

            this.time = new CalendarTime(this);

            integrator = new EulerIntegrator();
            //integrator = new RKCKIntegrator();
        }

        /// <summary>
        /// Event will fire when the simulation status changes.
        /// </summary>
        public event SimEnvStatusChangeEventHandler StatusChange;

        /// <summary>
        /// Event will fire when an event is about to be handled.
        /// </summary>
        public event SimEventHandler SimEvent;

        /// <summary>
        /// Event will fire just after handling the event.
        /// </summary>
        public event SimEventHandler AfterSimEvent;

        /// <summary>
        /// Event will fire after each completed integration step.
        /// </summary>
        public event IntegrationStepEventHandler IntegrationStep;

        /// <summary>
        /// Event will fire when a component model has a (short)
        /// message for the user.
        /// </summary>
        public event LogHandler Log;

        public event EventHandler<EventArgs> AfterTimeEventEvent;

        public event OutputEventHandler OutputEvent;

        /// <summary>
        /// Name of this environment
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The default integrator.
        /// </summary>
        public IIntegrator Integrator
        {
            get { return integrator; }
            set
            {
                // Copy existing registrations to new integrator
                if (value != null && integrator != null)
                    foreach (ISimObj simobj in integrator)
                        value.Add(simobj);

                integrator = value;
            }
        }

        /// <summary>
        /// Register a simulation object. If it is an Updateable SimObj, it will
        /// receive regular updates according to its timestep.
        /// </summary>
        /// <param name="simObj">Simulation object</param>
        public void Add(ISimObj simObj)
        {
            Add(simObj, true);
        }

        public void Add(ISimObj simObj, bool addToDefaultIntegrator)
        {
            // If it was registered somewhere, unregister it there.
            if (simObj.SimEnv != null)
                simObj.SimEnv.Remove(simObj);

            simObj.SimEnv = this; // It's ours now
            simObjects.Add(simObj);

            if (simObj is IOdeProvider && addToDefaultIntegrator)
                integrator.Add(simObj);

            // If a simulation is running, let the object know
            // it should start immediately
            if (status == SimulationStatus.Running)
                simObj.StartRun();
        }

        /// <summary>
        /// Remove the simulation object from the environment.
        /// </summary>
        /// <param name="simObj">Simulation object to remove.</param>
        public void Remove(ISimObj simObj)
        {
            if (simObj.SimEnv != this)
                throw new Exception(string.Format("Simulation object {0} registered somewhere else", simObj.Name));

            // No more events for this object
            timeEvents.RemoveObject(simObj);
            stateEvents.RemoveObject(simObj);

            if (status == SimulationStatus.Running)
                simObj.EndRun();

            if (simObj is IOdeProvider)
                integrator.Remove(simObj);

            simObjects.Remove(simObj);
            simObj.SimEnv = null;
        }

        /// <summary>
        /// removes all simulation objects and events.
        /// In fact this will clear the environment.
        /// </summary>
        public void Clear()
        {
            if (status == SimulationStatus.Running)
                EndRun();

            foreach (ISimObj obj in simObjects)
                obj.SimEnv = null;

            integrator.Clear();

            simObjects.Clear();
            UnregisterAllEvents();
        }

        /// <summary>
        /// The number of registered Simulation objects.
        /// </summary>
        public int Count
        {
            get { return simObjects.Count; }
        }

        /// <summary>
        /// Indexer for registered simulation objects.
        /// </summary>
        public ISimObj this[int index]
        {
            get { return ((ISimObj)simObjects[index]); }
        }

        /// <summary>
        /// Indexer for simulation objects by name
        /// </summary>
        public ISimObj this[string name]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                    if (this[i].Name.Equals(name))
                        return this[i];
                return null;
            }
        }

        /// <summary>
        /// The scheduled time events.
        /// </summary>
        public TimeEvents TimeEvents
        {
            get { return timeEvents; }
        }

        //UITLEG
        /// <summary>
        /// StateEvents
        /// </summary>
        public StateEvents StateEvents
        {
            get { return stateEvents; }
        }

        //		public SimTimeFormat TimeFormat
        //		{
        //			get { return timeFormat; }
        //			set { timeFormat = value; }
        //		}

        /// <summary>
        /// Current simulation status.
        /// </summary>
        public SimulationStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// Internal, called when the simulation status changes.
        /// Notifies the outside world.
        /// </summary>
        /// <param name="status"></param>
        protected void SetStatus(SimulationStatus status)
        {
            if (status != this.status)
            {
                this.status = status;

                // Signal event
                OnStatusChange();
            }
        }

        /// <summary>
        /// During the simulation this property will tell the current time.
        /// </summary>
        public double CurrentTime
        {
            get { return currentTime; }
        }

        /// <summary>
        /// Time at which the simulation starts.
        /// Defaults to 0.
        /// </summary>
        public double StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        /// <summary>
        /// Time at which the simulation will stop.
        /// </summary>
        public double StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }

        /// <summary>
        /// Accuracy of the time interval in which a state event occurred.
        /// When a state event occurs during an integration step, the simulation environment
        /// steps back and iterates the last integration step around the time the state event occurred.
        /// The state event occurred with a time interval wich size does not exceed Accuracy.
        /// </summary>
        public double Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        public ITime Time
        {
            get { return time; }
        }

        /// <summary>
        /// Initializes the simulation and notifies all simulation objects.
        /// StartRun sets the status to SimulationStatus.StartRun.
        /// Calls StartRun on all registered simulation objects.
        /// Then it schedules time events for all registered simulation objects that implement IUpdateable.
        /// Finally it sets the status to SimulationStatus.Running
        /// </summary>
        public void StartRun()
        {
            if (status != SimulationStatus.Idle)
                throw new Exception("SimEnv not Idle, call EndRun first");

            // Set Starting
            SetStatus(SimulationStatus.Starting);

            // Start at the beginning
            currentTime = startTime;
            stopRequested = false;

            if (integrator != null)
                integrator.StartRun();

            // Call StartRun on all objects
            try
            {
                foreach (ISimObj simObj in simObjects)
                    simObj.StartRun();
            }
            catch
            {
                UnregisterAllEvents();
                SetStatus(SimulationStatus.Idle);
                throw;
            }

            // Register the default time events
            ScanUpdateables();

            // Enter the running state
            SetStatus(SimulationStatus.Running);

            // output the state at the beginning of the simulation run
            PerformOutput();
        }

        /// <summary>
        /// Sets the status to SimulationStatus.EndRun.
        /// Then it clears all events and calls EndRun on all simulation objects.
        /// Finally it sets the status to SimulationStatus.Idle
        /// </summary>
        public void EndRun()
        {
            if (Status != SimulationStatus.Starting &&
                Status != SimulationStatus.Running)
                throw new Exception("SimEnv not Running");

            SetStatus(SimulationStatus.Stopping);

            UnregisterAllEvents();

            if (integrator != null)
                integrator.EndRun();

            try
            {
                foreach (ISimObj simObj in simObjects)
                    simObj.EndRun();
            }
            finally
            {
                SetStatus(SimulationStatus.Idle);
            }
        }

        /// <summary>
        /// Runs a complete simulation.
        ///	Calls StartRun(), Resume() and EndRun()
        /// </summary>
        public void Run()
        {
            StartRun();
            try
            {
                Resume();
            }
            finally
            {
                EndRun();
            }
        }

        /// <summary>
        /// Performs one simulation step.
        /// A simulation step consists of integrating to the next scheduled time event.
        /// and the handling of that event.
        /// </summary>
        /// <returns>True if the simulation reached its end time, False if not</returns>
        public bool Step()
        {
            double eventTime;
            double savedCurrentTime;
            ITimeEvent timeEvent;

            if (status != SimulationStatus.Running)
                throw new Exception("Simulation environment not running, call StartRun() first");

            // Finished ?
            if ((currentTime >= stopTime) || (stopRequested))
                return true;

            // Peek in the time events
            timeEvent = timeEvents.GetFirst();
            if (timeEvent != null)
                eventTime = timeEvent.EventTime;
            else
                eventTime = stopTime;

            // Integrate to the next event time
            while ((currentTime < eventTime) && (!stopRequested))
            {
                savedCurrentTime = currentTime;

                integrator.Step(ref currentTime, eventTime);

                if (stateEvents.HasEvent())
                {
                    IterateStateEvent(savedCurrentTime, currentTime);
                    OnIntegrationStep();

                    HandleStateEvents();

                    // Top of the time events might have changed due to
                    // handling state events.
                    timeEvent = timeEvents.GetFirst();
                    if (timeEvent != null)
                        eventTime = timeEvent.EventTime;
                    else
                        eventTime = stopTime;
                }
                else
                {
                    OnIntegrationStep();
                }
                PerformOutput();
            }

            // Handle the topmost time event
            timeEvent = TimeEvents.RemoveFirst();
            if (timeEvent != null)
            {
                OnSimEvent(timeEvent);
                timeEvent.HandleEvent();
                OnAfterSimEvent(timeEvent);
                // Handling an event may cause state events, so handle them
                HandleStateEvents();
                OnAfterTimeEvent();
            }

            return stopRequested;
        }

        private void OnAfterTimeEvent()
        {
            if (AfterTimeEventEvent != null)
                AfterTimeEventEvent(this, new EventArgs());
        }

        private void PerformOutput()
        {
            if (OutputEvent != null)
                OutputEvent(this, new EventArgs());
        }

        bool stopRequested;
        public void RequestStop()
        {
            stopRequested = true;
        }

        /// <summary>
        /// Calls Step until the simulation is finished.
        /// </summary>
        public void Resume()
        {
            if (status != SimulationStatus.Starting &&
                status != SimulationStatus.Running)
                throw new Exception("Call StartRun() first");

            SetStatus(SimulationStatus.Running);

            try
            {
                while (!Step())
                    ;
            }
            finally
            {

                EndRun();
            }
        }

        /// <summary>
        /// Register a time event. The event is scheduled by its event time and priority.
        /// </summary>
        /// <param name="simEvent">Event to register</param>
        public void RegisterEvent(ITimeEvent simEvent)
        {
            // If the system is running, new time events must occur on or after the current time
            if (status == SimulationStatus.Running && simEvent.EventTime < currentTime)
                throw new Exception("Time events can not be scheduled before the current simulation time.");

            simEvent.SimEnv = this;
            timeEvents.AddEvent(simEvent);
        }

        /// <summary>
        /// Register a state event. The event is scheduled by priority.
        /// </summary>
        /// <param name="simEvent">Event to register</param>
        public void RegisterEvent(IStateEvent simEvent)
        {
            simEvent.SimEnv = this;
            stateEvents.AddEvent(simEvent);
        }

        /// <summary>
        /// Removes the time event from the list.
        /// </summary>
        /// <param name="simEvent">Event to remove</param>
        public void UnregisterEvent(ITimeEvent simEvent)
        {
            timeEvents.RemoveEvent(simEvent);
            simEvent.SimEnv = null;
        }

        /// <summary>
        /// Removes the state event from the list.
        /// </summary>
        /// <param name="simEvent">Event to remove</param>
        public void UnregisterEvent(IStateEvent simEvent)
        {
            stateEvents.RemoveEvent(simEvent);
            simEvent.SimEnv = null;
        }

        /// <summary>
        /// Clear the entire event list
        /// </summary>
        public void UnregisterAllEvents()
        {
            timeEvents.Clear();
            stateEvents.Clear();
        }

        /// <summary>
        /// Fire a StatusChange event
        /// </summary>
        protected void OnStatusChange()
        {
            if (StatusChange != null)
                StatusChange(this, EventArgs.Empty);
        }

        /// <summary>
        /// Tell listeners that an event is about to be handled.
        /// </summary>
        /// <param name="simEvent"></param>
        protected void OnSimEvent(ISimEvent simEvent)
        {
            if (SimEvent != null)
                SimEvent(this, new SimEventEventArgs(simEvent));
        }

        /// <summary>
        /// Tell listeners that an event is just handled.
        /// </summary>
        /// <param name="simEvent"></param>
        protected void OnAfterSimEvent(ISimEvent simEvent)
        {
            if (AfterSimEvent != null)
                AfterSimEvent(this, new SimEventEventArgs(simEvent));
        }

        /// <summary>
        /// Tell listeners that an integration step completed.
        /// </summary>
        protected void OnIntegrationStep()
        {
            if (IntegrationStep != null)
                IntegrationStep(this, new EventArgs());
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void LogMessage(object msg)
        {
            OnLog(msg);
        }


        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        protected void OnLog(object msg)
        {
            if (Log == null)
                Console.WriteLine(msg.ToString());
            else
                Log(msg);
        }


        /// <summary>
        /// As long as there are state events, handle them.
        /// </summary>
        protected void HandleStateEvents()
        {
            ISimEvent simEvent = StateEvents.GetEvent();

            while (simEvent != null)
            {
                OnSimEvent(simEvent);
                simEvent.HandleEvent();
                simEvent = stateEvents.GetEvent();
            }
        }


        /// <summary>
        /// Determine the lower and upper boundary in which the state event occurred.
        /// </summary>
        /// <param name="lb">start lower boundary</param>
        /// <param name="ub">maximum upper boundary</param>
        protected void IterateStateEvent(double lb, double ub)
        {
            double savedCurrentTime = lb; // Bottom time for integrator step back
            bool hasEvents;

            do
            {
                currentTime = savedCurrentTime;
                integrator.StepBack();

                if ((ub - lb) < accuracy)
                {
                    // Step to the upper boundary of the interval, so
                    // the state event did occur.
                    integrator.Step(ref currentTime, ub);
                    break;
                }

                // Step to the middle of the interval
                integrator.Step(ref currentTime, (lb + ub) / 2);

                // Adjust lower or upper boundary depending on a state event
                hasEvents = stateEvents.HasEvent();
                if (hasEvents)
                    ub = currentTime; // Shift the upper boundary down
                else
                    lb = currentTime; // Shift the lower boundary up
            }
            while (!hasEvents || ((ub - lb) > accuracy));
        }



        /// <summary>
        /// Loop through the list of registered objects and see what kind of event to schedule.
        /// </summary>
        void ScanUpdateables()
        {
            foreach (ISimObj simObj in simObjects)
            {
                IDiscreteModel upd = (simObj as IDiscreteModel);
                if ((upd != null) && (upd.UpdateMethod != UpdateMethod.None))
                {
                    ITimeEvent simEvent;
                    if (upd.UpdateMethod == UpdateMethod.Recurring)
                        simEvent = new SystemRecurringTimeEvent(simObj, simObj, upd.Priority, 0, startTime);
                    else
                        simEvent = new TimeEvent(simObj, simObj, upd.Priority, 0, startTime);

                    RegisterEvent(simEvent);
                }
            }
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return simObjects.GetEnumerator();
        }

        #endregion
    }
}
