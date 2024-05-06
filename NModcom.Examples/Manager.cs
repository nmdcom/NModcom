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
using NModcom.Util;
using System.Xml;

namespace NModcom.Examples
{

	/// <summary>
	/// Example APES management class. This class works closely together
	/// with IRule implementers and places a special requirement on the constructor 
	/// of the ITimeEvent descendants that are used with it.
	/// Manager is parameterized with all info about management.
	/// Manager creates any number of IRule implementers.
	/// IRule implementers decide on when to fire ITimeEvent implements.
	/// ITimeEvent implementers describe the management action that is taken.
	/// Effect of management actions is implemented by the (ISimObj) receivers of ITimeEvents.
	/// In effectuating management, the ISimObj receivers use the information that is 
	/// carried by the specialized ITimeEvent implementers.
	/// 
	/// <seealso cref="IRule"/>
	/// <seealso cref="ITimeEvent"/>
	/// </summary>
	public class Manager: UpdateableSimObj
	{
		public const string SOILWC = "uri for soil water content";

		[Param("management")]
		private string management;

		ArrayList rules = new ArrayList();

		public Manager()
		{
		}

		public override void StartRun()
		{
			// preamble
			SimEnv.LogMessage("manager");
			
			// naive, temporary way of finding targets
			// this will have to be done in a more flexible way soon
			ISimObj crop = null, soil = null;
			for (int i = 0; i < SimEnv.Count; i++)
				if (SimEnv[i] is SimpleCrop)
					crop = SimEnv[i];
				else if (SimEnv[i] is SimpleSoil)
					soil = SimEnv[i];

			// read information about rules
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(management);

			// 
			RunInfo info = RunInfo.Info();

			// initialize rules
			rules.Clear();
			XmlNodeList list = doc.SelectNodes("management/rule");
			foreach (XmlNode node in list)
			{
				// find out what rule class, create it and add it to our list
				string ruleAssembly = node.Attributes["assembly"].Value; // e.g. "NModcom.dll";
				string ruleClass = node.Attributes["class"].Value; // e.g. "NModcom.Examples.Rule1";
				IRule rule = (IRule)info.CreateInstance(ruleAssembly, ruleClass);
				rules.Add(rule);
				
				// configure rule: basic stuff
				rule.Source = this;
				rule.Target = soil; // TODO: how do we know this? ANSWER: get it via an input
				rule.SimEnv = this.SimEnv;

				// configure rule: parameters
				XmlNodeList parlist = node.SelectNodes("param");
				foreach (XmlNode param in parlist)
					rule.SetParam(param.Attributes["name"].Value, param.Attributes["value"].Value);

				// find out what event class and pass the info on to the rule object
				XmlNodeList evtlist = node.SelectNodes("event");
				XmlNode evtnode = evtlist[0];
				string eventAssembly = evtnode.Attributes["assembly"].Value;
				string eventClass = evtnode.Attributes["class"].Value; 
				rule.SetEvent(eventAssembly, eventClass);

				// parameters for the event 
				parlist = evtnode.SelectNodes("param");
				foreach (XmlNode param in parlist)
					rule.SetEventParam(param.Attributes["name"].Value, param.Attributes["value"].Value);

				Console.WriteLine(rule.ToString());
			}

			SimEnv.Time.RotationLength = 2; //TODO: this doesn't belong here, but in SimEnvReader or so

//			ruleParamName[3] = "firstTime"; ruleParamValue[3] = Convert.ToString(SimEnv.StartTime + 80); 
//			ruleParamName[4] = "lastTime"; ruleParamValue[4] = Convert.ToString(SimEnv.StartTime + 100); 

			// start all rules
			foreach (IRule r in rules)
                r.StartRun();

		}
		
		public override void HandleEvent(ISimEvent simEvent)
		{
			foreach (IRule r in rules)
				r.Evaluate(SimEnv);
		}
	
	}
}
