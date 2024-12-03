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
using System.Collections;

namespace NModcom
{
    /// <summary>
    /// NModcom is the base namespace for MODCOM. The most 
    /// frequently used classes and interfaces are declared in this namespace.
    /// </summary>
    class NamespaceDoc
    {
    }

    /// <summary>
    /// Descendant of <see cref="EventArgs"/> that holds a reference to a 
    /// <see cref="ISimEvent"/>
    /// </summary>
    //TODO: do we need this class? If so, why for this handler and not for the others?
    public class SimEventEventArgs : EventArgs
    {
        private readonly ISimEvent simEvent;

        //UITLEG
        public SimEventEventArgs(ISimEvent simEvent)
        {
            this.simEvent = simEvent;
        }

        public ISimEvent SimEvent
        {
            get { return simEvent; }
        }
    }

    /// <summary>
    /// Delegate for event that will fire when a simulation event is about to be handled.
    /// </summary>
    public delegate void SimEventHandler(object sender, SimEventEventArgs e);

    /// <summary>
    /// Delegate for event that will fire when the simulation status changes.
    /// </summary>
    public delegate void SimEnvStatusChangeEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Delegate for event that will fire after each completed integration step.
    /// </summary>
    public delegate void IntegrationStepEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Delegate for event that will fire when a component model has a (short)
    /// message for the user.
    /// </summary>
    public delegate void LogHandler(object msg);

    /// <summary>
    /// Delegate for event that will fire when model output must be done.
    /// </summary>
    public delegate void OutputEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Possible states of the Simulation environment
    /// </summary>
    public enum SimulationStatus
    {
        Idle,       // Idle, not running
        Starting,   // In StartRun
        Running,    // After StartRun and stepping
        Stopping,   // In EndRun
        Error       // An error occurred, leave this state by calling EndRun()
    }


    /// <summary>
    /// Default event priorities.
    /// Add or subtract to these values to fine tune your simulation.
    /// </summary>
    public enum EventPriority : int
    {
        CollectOutput = 300,
        Update = 400,
        Integration = 500,
        System = 10000
    }

    /// <summary>
    /// Simulation environment is responsible for the simulation flow.
    /// Implementations should also implement IEnumerable
    /// </summary>
    public interface ISimEnv
    {
        /// <summary>
        /// Event will fire when the simulation status changes.
        /// </summary>
        event SimEnvStatusChangeEventHandler StatusChange;

        /// <summary>
        /// Event will fire when an event is about to be handled.
        /// </summary>
        event SimEventHandler SimEvent;

        /// <summary>
        /// Event will fire when an event is just handled.
        /// </summary>
        event SimEventHandler AfterSimEvent;

        /// <summary>
        /// Event will fire after each completed integration step.
        /// </summary>
        event IntegrationStepEventHandler IntegrationStep;

        /// <summary>
        /// Event will fire when WriteLine() is called. If Log is not assigned to, 
        /// messages are written to the console.
        /// </summary>
        /// <example>Components that want to talk to the user should call SimEnv.WriteLine.
        /// <code>
        ///		public override void StartRun()
        ///		{
        ///			// set initial value of state variable
        ///			SimEnv.WriteLine("We are starting now!");
        ///		}
        ///
        ///		To make these messages visible in your application, first define a method as follows:
        ///
        ///		// this method is called whenever a component model has
        ///		// something to say
        ///		private void InformTheUser(string msg)
        ///		{
        ///			// this is simple; but you'll write to a Form
        ///			Console.WriteLine(msg);
        ///		}
        ///
        ///		Then hook this method up to the simenv:
        ///
        ///		simenv.Log += new LogHandler(InformTheUser);
        ///
        ///		
        /// </code>
        /// </example>
        event LogHandler Log;

        event EventHandler<EventArgs> AfterTimeEventEvent;

        event OutputEventHandler OutputEvent;

        /// <summary>
        /// Name of this environment
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Default integrator.
        /// The steps of this integrator are synchronised with the time- and
        /// state events of the simulation. This means that at every time- or state event,
        /// the state variables have the correct value.
        /// When a new integrator is assigned, the registrations are copied to the new
        /// integrator.
        /// </summary>
        IIntegrator Integrator
        {
            get;
            set;
        }

        /// <summary>
        /// Register a simulation object.
        /// </summary>
        /// <param name="simObj">Simulation object</param>
        void Add(ISimObj simObj);

        /// <summary>
        /// Register a simulation object. See also <see cref="Remove"/>.
        /// </summary>
        /// <param name="simObj">Simulation object</param>
        /// <param name="addToDefaultIntegrator">if true, the object will be added to the
        /// default integrator</param>
        void Add(ISimObj simObj, bool addToDefaultIntegrator);

        /// <summary>
        /// Remove the simulation object from the environment.
        /// </summary>
        /// <param name="simObj">Simulation object to remove.</param>
        void Remove(ISimObj simObj);

        /// <summary>
        /// Removes all simulation objects and events.
        /// In fact this will clear the environment.
        /// </summary>
        void Clear();

        /// <summary>
        /// The number of registered Simulation objects
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Indexer for registered simulation objects.
        /// </summary>
        ISimObj this[int index]
        {
            get;
        }

        /// <summary>
        /// Indexer for simulation objects by name
        /// </summary>
        ISimObj this[string name]
        {
            get;
        }

        /// <summary>
        /// The GetEnumerator method supports the .NET Framework infrastructure
        /// and is not intended to be used directly from your code.
        /// </summary>
        /// <returns>This method returns an IEnumerator object.</returns>
        IEnumerator GetEnumerator();

        /// <summary>
        /// The scheduled time events.
        /// </summary>
        TimeEvents TimeEvents
        {
            get;
        }

        /// <summary>
        /// List of state events.
        /// </summary>
        StateEvents StateEvents
        {
            get;
        }

        /// <summary>
        /// Current simulation status.
        /// </summary>
        SimulationStatus Status
        {
            get;
        }

        /// <summary>
        /// During the simulation this property will tell the current time.
        /// </summary>
        double CurrentTime
        {
            get;
        }

        /// <summary>
        /// Time at which the simulation starts.
        /// Defaults to 0.
        /// </summary>
        double StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Time at which the simulation will stop.
        /// </summary>
        double StopTime
        {
            get;
            set;
        }

        /// <summary>
        /// Accuracy of the time interval in which a state event occurred.
        /// When a state event occurs during an integration step, the simulation environment
        /// steps back and iterates the last integration step around the time the state event occurred.
        /// The state event occurred with a time interval wich size does not exceed Accuracy.
        /// </summary>
        double Accuracy
        {
            get;
            set;
        }

        // UITLEG
        ITime Time
        {
            get;
        }

        /// <summary>
        /// Initializes the simulation and notifies all simulation objects.
        /// StartRun sets the status to SimulationStatus.StartRun.
        /// Calls StartRun on all registered simulation objects.
        /// Then it schedules time events for all registered simulation objects that implement IUpdateable.
        /// Finally it sets the status to SimulationStatus.Running
        /// </summary>
        void StartRun();

        /// <summary>
        /// Sets the status to SimulationStatus.EndRun.
        /// Then it clears all events and calls EndRun on all simulation objects.
        /// Finally it sets the status to SimulationStatus.Idle
        /// </summary>
        void EndRun();

        /// <summary>
        /// Runs a complete simulation.
        ///	Calls StartRun(), Resume() and EndRun()
        /// </summary>
        void Run();

        /// <summary>
        /// Performs one simulation step.
        /// A simulation step consists of integrating to the next scheduled time event.
        /// and the handling of that event.
        /// </summary>
        /// <returns>True if the simulation reached its end time, False if not.</returns>
        bool Step();

        /// <summary>
        /// Calls Step() until the simulation is finished.
        /// </summary>
        void Resume();

        /// <summary>
        /// Resume the simulation with Resume() and call EndRun() when the simulation is finished. 
        /// Please note that StartRun() should be called before this method is called.
        /// </summary>
        void ResumeToEnd();

        /// <summary>
        /// Register a time event. The event is scheduled by its event time and priority.
        /// </summary>
        /// <param name="simEvent">Event to register.</param>
        void RegisterEvent(ITimeEvent simEvent);

        /// <summary>
        /// Register a state event. The event is scheduled by priority.
        /// </summary>
        /// <param name="simEvent">Event to register.</param>
        void RegisterEvent(IStateEvent simEvent);

        /// <summary>
        /// Removes the time event from the list.
        /// </summary>
        /// <param name="simEvent">Event to remove.</param>
        void UnregisterEvent(ITimeEvent simEvent);

        /// <summary>
        /// Removes the state event from the list.
        /// </summary>
        /// <param name="simEvent">Event to remove.</param>
        void UnregisterEvent(IStateEvent simEvent);

        /// <summary>
        /// Clear the entire event list.
        /// </summary>
        void UnregisterAllEvents();

        /// <summary>
        /// Write an informational message by firing the OnLog event.
        /// </summary>
        /// <param name="msg">The message to be written in the log.</param>
        void LogMessage(object msg);

        /// <summary>
        /// When this method is called, the simulation will be stopped after the current time step.
        /// </summary>
        void RequestStop();

    }


}