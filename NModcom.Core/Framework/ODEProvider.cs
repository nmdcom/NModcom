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

namespace NModcom
{
    /// <summary>
    /// SimObjs that needs integration should implement this interface.
    /// </summary>
    public interface IOdeProvider
    {
        /// <summary>
        /// The number of state variables.
        /// </summary>
        /// <returns>This method returns the number of state variables.</returns>
        int GetCount();

        /// <summary>
        /// Called by the integrator to get the current state.
        /// The integrator will update the state values and pass the same array
        /// to the <see cref="SetState"/> method.
        /// </summary>
        /// <param name="state">state array of the integrator</param>
        /// <param name="index">base index to which the simObj should copy its state values</param>
        void GetState(double[] state, int index);

        /// <summary>
        /// Called by the integrator with the newly calculated state.
        /// </summary>
        /// <param name="state"></param>
        void SetState(double[] state, int index);

        /// <summary>
        /// Called by the framework to obtain rates for all state variables.
        /// </summary>
        /// <param name="deriv">Array to hold rates upon return.</param>
        /// <param name="index">The index into deriv where the rate for the first
        /// state variable should be stored. Subsequent rates go in adjacent elements.</param>
        void GetDerivatives(double[] deriv, int index);

    }
}