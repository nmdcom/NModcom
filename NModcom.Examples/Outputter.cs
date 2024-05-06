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
	/// This class writes its inputs to console. Use to show outputs of
	/// classes that don't show their own outputs. Currently limited to 
	/// two inputs. 
	/// </summary>
	public class Outputter2: UpdateableSimObj
	{
		[Input("input1")]
		private double input1;

		[Input("input2")]
		private double input2;

		// here goes the model
		public override void HandleEvent(ISimEvent simEvent)
		{
			// do something
			SimEnv.LogMessage("time=" + SimEnv.CurrentTime);
			SimEnv.LogMessage("input 1: " + input1);
			SimEnv.LogMessage("input 2: " + input2);
		}

	}
}
