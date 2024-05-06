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
	/// PredOrPrey2 represent a predator or a prey in a predator-prey model.
	/// This example is taken from Leffelaar(1993), p56.
	/// Reference: Leffelaar, P.A., 1993. On systems analysis and simulation of 
	/// ecological processes. Kluwer, Dordrecht.
	/// </summary>
	public class PredOrPrey2: OdeSimObj
	{

		[State("density")]
		private double density = 3;

        [Rate("density")]
        private double d_density;

		[Param("rgr")]
		private double rgr = 1;

		[Param("k")]
		private double k = 0.5;

		[Signal("density of other species")]
		private double otherSpeciesDensity = 0.2;

//		[Output("change")]
//		private double change;

		// rates for all state variables are calculated here
		public override void GetRates()
		{
			// calculate rate
            d_density = rgr * density + k * density * otherSpeciesDensity;
		}

	}
}
