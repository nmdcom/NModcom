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

namespace NModcom.Examples
{
	/// <summary>
	/// Summary description for SimpleSoil.
	/// </summary>
	public class SimpleSoil: SimObj, IOdeProvider
	{

		[Output("bd")]
		protected IData bd = new ConstFloatSimData(null, 0);

		[Output("soilwc")]
		protected IData soilwc = new ConstFloatSimData(null, 0.1);

		public SimpleSoil()
		{
		}

		public override void StartRun()
		{
			// set initial value of state variable
			bd.AsFloat = 1.2;
			soilwc.AsFloat = 0.11;
		}

		public int GetCount()
		{
			// number of state variables
			return 1;
		}

		public void GetState(double[] state, int index)
		{
			// send value(s) of state variable(s) to framework
			// 1st goes into state[index], 2nd into state[index+1], etc.
			state[index] = soilwc.AsFloat;
		}
		
		public void SetState(double[] state, int index)
		{
			// set value of state variable
			soilwc.AsFloat = state[index];
		}

		public void GetDerivatives(double[] deriv, int index)
		{
			// calculate rate
			deriv[index] = 0.0234;
		}

		public virtual void BeforeIntegration(double currentTime)
		{
		}

		public virtual void AfterIntegration(double currentTime)
		{
			// nothing needs to be done for this particular model
		}

		public override void HandleEvent(ISimEvent simEvent)
		{
			if (simEvent is TillageWepp)
			{
				SimEnv.LogMessage("\ntillage is happening at time=" + simEvent.SimEnv.Time.CurrentDate.ToLongDateString());
				TillageWepp t = (simEvent as TillageWepp);
				SimEnv.LogMessage("now tilling to a depth of " + t.TillageDepth + " m");
			}
			else if (simEvent is IrrigationEvent)
			{
				SimEnv.LogMessage("\nirrigation is happening at time=" + simEvent.SimEnv.Time.CurrentDate.ToLongDateString());
				IrrigationEvent irr = (simEvent as IrrigationEvent);
				SimEnv.LogMessage("irrigating with volume=" + irr.Volume + ", intensity=" + irr.Intensity + ", salinity=" + irr.Salinity);
			}
		}

	}
	
}
