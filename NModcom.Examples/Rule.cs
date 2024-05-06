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
using System.Reflection;
using NModcom;
using NModcom.Util;

namespace NModcom.Examples
{

	/// <summary>
	/// Interface for classes that represent an APES management rule. A rule
	/// contains the logic to determine when to schedule management events.
	/// 
	/// <seealso cref="Manager"/>
	/// </summary>
	public interface IRule
	{
		ISimEnv SimEnv { get; set; } 
		ISimObj Source { get; set; } 
		ISimObj Target { get; set; } 
		void SetEvent(string eventAssembly, string eventClass);
		void SetParam(string paramName, object paramValue);
		void SetEventParam(string paramName, object paramValue);
		void StartRun();
		void Evaluate(ISimEnv simenv);
	}

	/// <summary>
	/// Base class implementing IRule.
	/// 
	/// <seealso cref="Manager"/>
	/// </summary>
	public abstract class BasicRule: IRule
	{
		private ISimEnv simenv;
		private ISimObj source, target;
		protected string eventAssembly;
		protected string eventClass;
		public Hashtable ruleHash = new Hashtable();
		public Hashtable eventHash = new Hashtable();

		public BasicRule()
		{
		}

		public override string ToString()
		{
			string s = base.ToString() + ":\n";
			foreach (object o in ruleHash.Keys)
				s = s + "   " + o + "=" + ruleHash[o] + "\n";
			s = s + "   " + eventAssembly + ", " + eventClass;
			foreach (object o in eventHash.Keys)
				s = s + "   " + o + "=" + eventHash[o] + "\n";
			return s;
		}

		public virtual void StartRun()
		{
		}

		public virtual void FireEvent()
		{
			FireEvent(0);
		}

		public virtual void FireEvent(int delay)
		{
			// use reflection to find out the names of the arguments 
			// of the constructor and then search for parameters with that name

			// load assembly
			Assembly assembly = RunInfo.Info().LoadAssembly(eventAssembly);
			if (assembly == null) 
				throw new Exception("Could not load assembly " + eventAssembly);

			// find type for our class
			Type type = assembly.GetType(eventClass);
			if (type == null) 
				throw new Exception("Could not find class " + eventClass);

			// get constructor for our class
			ConstructorInfo[] ctor = type.GetConstructors();
			if ((ctor == null) || (ctor.Length != 1))
				throw new Exception("Could not find constructor " 
					+ "(or found more than one and don't know which one to use) for event " 
					+ eventClass);

			// what parameters for our constructor
			ParameterInfo[] param = ctor[0].GetParameters();

			// All management events inherit from NModcom.TimeEvent, thus
			// the first five parameters of the constructur must be the 
			// parameters of NModcom.TimeEvent constructor
			if 
				(
				(param[0].ParameterType != typeof(ISimObj)) 
				||	(param[1].ParameterType != typeof(ISimObj)) 
				||	(param[2].ParameterType != typeof(int)) 
				||	(param[3].ParameterType != typeof(int)) 
				||	(param[4].ParameterType != typeof(double)) 
				)
				throw new Exception("Parameter does not have the expected type");

			object[] values = new object[param.Length];

			// fill in the parameters of the basic TimeEvent
			values[0] = Source;
			values[1] = Target;
			values[2] = 0;
			values[3] = 0;
			values[4] = SimEnv.CurrentTime + delay;

			// fill in the event-specific parameters
			for (int i = 5; i < values.Length; i++)
			{
				object o = eventHash[param[i].Name];
				if (o == null)
					throw new Exception("Could not find a value for \"" + param[i].Name + "\" of rule " + GetType().FullName);
				else
					values[i] = o;
			}

			// create the event
			TimeEvent evt = (TimeEvent)ctor[0].Invoke(values);
			if (evt == null) throw new Exception("Failed to create " + eventClass);

			SimEnv.RegisterEvent(evt);
		}

		public abstract void Evaluate(ISimEnv simenv);
		
		protected object GetParam(string name)
		{
			object o = ruleHash[name];
			if (o == null) 
				throw new Exception("Could not find param \"" + name + "\" for rule " + GetType().Name);
			else
				return o;
		}

		#region IRule Members

		public virtual void SetEvent(string eventAssembly, string eventClass)
		{
			this.eventAssembly = eventAssembly;
			this.eventClass= eventClass;
		}

		public virtual void SetParam(string paramName, object paramValue)
		{
			ruleHash[paramName] = paramValue;
		}

		public virtual void SetEventParam(string paramName, object paramValue)
		{
			eventHash[paramName] = paramValue;
		}

		public ISimEnv SimEnv
		{
			get { return simenv; }
			set	{ simenv = value; }
		}

		public ISimObj Source
		{
			get { return source; }
			set	{ source = value; }
		}

		public ISimObj Target
		{
			get { return target; }
			set	{ target = value; }
		}

		#endregion
	}

	/// <summary>
	/// This example APES management rule schedules its associated 
	/// management event once a year, namely when simulated time matches
	/// a given month and day-of-month.
	/// </summary>
	public class Rule1: BasicRule
	{
		int triggerday, triggermonth;

		public override void StartRun()
		{
			triggerday = Convert.ToInt32(GetParam("day"));
			triggermonth = Convert.ToInt32(GetParam("month"));
		}

		public override void Evaluate(ISimEnv simenv)
		{
			int year, month, day;
			SimEnv.Time.GetCurrentYMD(out year, out month, out day);
			if (month==triggermonth && day == triggerday)
				FireEvent();
		}
	}

	/// <summary>
	/// This example APES management rule schedules its associated 
	/// management event whenever a set of rules about current time and water 
	/// content of the soil are satisfied. Additionally, events cannot occur sooner 
	/// than "interval" time units (days?) after the previously scheduled event.
	/// </summary>
	public class Rule2: BasicRule
	{
		IData soilwc = null;
		double threshold;
		double last;
		int interval, firstDOY, lastDOY;

		public override void StartRun()
		{
			// last time an event was executed
			last = -999;

            ////// use the ontology to find the inputs that we need
            ////// let's say we need soil water content as identified by string SOILWC
            ////for (int s = 0; s < SimEnv.Count; s++)
            ////    for (int i = 0; i < SimEnv[s].Outputs.Count; i++)
            ////        if (SimEnv[s].Outputs[i].URI.Equals(Manager.SOILWC))
            ////        {
            ////            soilwc = SimEnv[s].Outputs[i].Data;
            ////            break;
            ////        }

			// the threshold is a parameter
			threshold = Convert.ToDouble(GetParam("soilwc"));
			interval = Convert.ToInt32(GetParam("interval"));
			firstDOY = Convert.ToInt32(GetParam("firstDOY"));
			lastDOY = Convert.ToInt32(GetParam("lastDOY"));
		}

		public override void Evaluate(ISimEnv simenv)
		{
			if 
			(
					(simenv.CurrentTime - last >= interval)
				&&	(simenv.Time.CurrentDate.DayOfYear >= firstDOY) 
				&&	(simenv.Time.CurrentDate.DayOfYear <= lastDOY) 
				&&	(soilwc.AsFloat > threshold)

			)
			{
				// remember the current time
				last = simenv.CurrentTime;

				// fire the event
				FireEvent();
			}
		}

	}
}

