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
    /// Implementation of the Euler (rectangular) algorithm for numerical 
    /// integration of an ordinary differential equation.
    /// </summary>
    public class EulerIntegrator : Integrator
    {
        double[] y;
        double[] state;
        double[] d;

        public EulerIntegrator()
        {
            y = new double[0];
            state = new double[0];
            d = new double[0];
            IntegrationTimeStep = 1;
        }

        //UITLEG
        protected override void SetStateLength(int length)
        {
            y = new double[length];
            state = new double[length];
            d = new double[length];
        }

        //UITLEG
        /// <summary>
        /// Perform one integration step.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="endTime">The endtime of the integration step.</param>
        public override void Step(ref double currentTime, double endTime)
        {
            int n = DoCountStates();

            if (n == 0)
            {
                currentTime = endTime;
                return;
            }

            double h = IntegrationTimeStep;
            if ((currentTime + h) > endTime)
                h = endTime - currentTime;

            DoGetState(y);
            DoGetDerivatives(currentTime, d);

            for (int i = 0; i < n; i++)
                state[i] = y[i] + h * d[i];

            DoSetState(state);

            currentTime = currentTime + h;
        }

        //UITLEG
        /// <summary>
        /// Go back one integration step. 
        /// </summary>
        public override void StepBack()
        {
            DoSetState(y);
        }
    }
}
