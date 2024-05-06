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
	/// This namespace holds example classes that highlight many uses of the MODCOM framework.
	/// </summary>
	class NamespaceDoc
	{
	}

	/// <summary>
	/// PredOrPrey represent a predator or a prey in a predator-prey model.
	/// See also PredOrPrey2.
	/// This example is taken from Leffelaar(1993), p56.
	/// Reference: Leffelaar, P.A., 1993. On systems analysis and simulation of 
	/// ecological processes. Kluwer, Dordrecht.
	/// </summary>
	public class PredOrPrey: SimObj, IOdeProvider
	{
		[Input("density", "density", "Number of individuals")]
        [Concept("http://www.owl-ontologies.com/unnamed.owl#GSrad_Instance_20009")]
        protected IData init_p;

		[Input("rgr", "Relative growth rate", "per unit time")]
        [Concept("http://www.owl-ontologies.com/unnamed.owl#GSrad_Instance_20010")]
        protected IData in_rgr;

		[Input("k", "interaction factor", "(units of the interaction factor)")]
        [Concept("http://www.owl-ontologies.com/unnamed.owl#GSrad_Instance_20011")]
		protected IData in_k;

		[Input("density of other species", "density of other species", "Number of individuals")]
        [Concept("http://www.owl-ontologies.com/unnamed.owl#GSrad_Instance_20012")]
        protected IData in_p2;

		[Output("density", "", "")]
        [Concept("http://www.owl-ontologies.com/unnamed.owl#GSrad_Instance_20009")]
        protected IData p;

		private double rgr, k;

		public PredOrPrey()
		{
			// as a courtesy to the user, provide default values for inputs and outputs

			// initial state
			init_p = new ConstFloatSimData(this, 3);
			
			// parameters
			in_rgr = new ConstFloatSimData(this, -0.5);
			in_k = new ConstFloatSimData(this, -0.5);
			
			// dynamic inputs
			in_p2 = new ConstFloatSimData(this, 0);
			
			// output
			p = new ConstFloatSimData(this, 1);
		}

		public override void StartRun()
		{
			// set initial value of state variable
			p.AsFloat = init_p.AsFloat;

			// copy parameter values to local variables
			rgr = in_rgr.AsFloat;
		    k = in_k.AsFloat;
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
			state[index] = p.AsFloat;
		}
		
		public void SetState(double[] state, int index)
		{
			// set value of state variable
			p.AsFloat = state[index];
		}

		public void GetDerivatives(double[] deriv, int index)
		{
			// Console.WriteLine("GetDerivatives at time =" + SimEnv.Time.ToString());
			// calculate rate
			deriv[index] = rgr * p.AsFloat + k * p.AsFloat * in_p2.AsFloat;
		}

	}
}
