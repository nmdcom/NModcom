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

namespace NModcom.Examples
{

	/// <summary>
	/// An example of an event in APES. All events must implement ITimeEvent or IStateEvent.
	/// Easiest way to do that is by deriving from TimeEvent or StateEvent. 
	/// A special requirement for APES management events is that they have only one 
	/// constructor, and that constructor has first the five standard parameters,
	/// in standard order. After that, there may be as many string parameters as necessary.
	/// At run time, the names of these parameters will be used to find values
	/// for them in the input data.
	/// </summary>
	public class IrrigationEvent: TimeEvent
	{
		double volume, intensity, salinity;

		public IrrigationEvent(ISimObj source, ISimObj target, int priority, int message, double time, string volume, string intensity, string salinity)
			: base(source, target, priority, message, time)
		{
			this.volume = Convert.ToDouble(volume);
			this.intensity = Convert.ToDouble(intensity);
			this.salinity = Convert.ToDouble(salinity);
		}

		public double Volume { get { return volume; } set { volume = value; } }

		public double Intensity { get { return intensity; } set { intensity = value; } }

		public double Salinity { get { return salinity; } set { salinity = value; } }

	}

	/// <summary>
	/// A second example of an APES management event. If we can do two, we can do 
	/// however many we want!
	/// </summary>
	public class TillageWepp: TimeEvent
	{
		private double tillageDepth; 

		public TillageWepp(ISimObj source, ISimObj target, int priority, int message, double time, string tillageDepth)
			: base(source, target, priority, message, time)
		{
			this.tillageDepth = Convert.ToDouble(tillageDepth);
		}

		public double TillageDepth 
		{
			get {return tillageDepth; } 
			set {tillageDepth = value; }
		} 

	}

}


