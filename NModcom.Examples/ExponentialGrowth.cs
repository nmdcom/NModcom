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
	/// Summary description for ExponentialGrowth.
	/// </summary>
	public class ExponentialGrowth: SimObj, IOdeProvider
	{
		private double x, rgr;

		[Input("Initial amount", "Initial amount", "MyUniversalUnit")]
		private IData init_x;

		[Input("rgr", "-", "MyUniversalUnit")]
		private IData in_rgr;

		[Output("Amount", "Not_Empty", "MyUnit")]
		private IData out_x;

		public ExponentialGrowth()
		{
			init_x = new ConstFloatSimData(this, 1);
			in_rgr = new ConstFloatSimData(this, 0.1);
			out_x = new ConstFloatSimData(this, 1);
		}

		public override void StartRun()
		{
			x = init_x.AsFloat;
			rgr = in_rgr.AsFloat;
		}

		public int GetCount()
		{
			return 1;
		}

		public void GetState(double[] state, int index)
		{
			state[index] = x;
		}
		
		public void SetState(double[] state, int index)
		{
			x = state[index];
		}

		public void GetDerivatives(double[] deriv, int index)
		{
			deriv[index] = x * rgr;
		}

	}
}

