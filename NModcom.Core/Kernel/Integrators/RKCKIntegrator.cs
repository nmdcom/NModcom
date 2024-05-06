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

namespace NModcom
{
    /// <summary>
    /// Implementation of the Runge-Kutta variable-stepsize algorithm for numerical 
    /// integration of an ordinary differential equation. This particular 
    /// implementation uses Cash-Karp coefficients and is described by:
    /// 
    /// J. R. Cash, A. H. Karp. "A variable order Runge-Kutta method for initial value problems with rapidly varying right-hand sides", ACM Transactions on Mathematical Software 16: 201-222, 1990
    /// </summary>
    public class RKCKIntegrator : Integrator
    {

        #region Constants
        const double SAFETY = 0.9;
        const double PGROW = -0.2;
        const double PSHRINK = -0.25;
        const double ERRCON = 1.89e-4;
        const double TINY = 1.0e-30;

        const double rkvMinTimeStep = 1e-15;

        // Cash-Karp constants
        const double a2 = 0.2;
        const double a3 = 0.3;
        const double a4 = 0.6;
        const double a5 = 1.0;
        const double a6 = 0.875;

        const double b21 = 0.2;
        const double b31 = 3 / 40.0;
        const double b32 = 9 / 40.0;
        const double b41 = 0.3;
        const double b42 = -0.9;
        const double b43 = 1.2;
        const double b51 = -11 / 54.0;
        const double b52 = 2.5;
        const double b53 = -70 / 27.0;
        const double b54 = 35 / 27.0;
        const double b61 = 1631 / 55296.0;
        const double b62 = 175 / 512.0;
        const double b63 = 575 / 13824.0;
        const double b64 = 44275 / 110592.0;
        const double b65 = 253 / 4096.0;

        const double c1 = 37 / 378.0;
        const double c3 = 250 / 621.0;
        const double c4 = 125 / 594.0;
        const double c6 = 512 / 1771.0;

        const double dc1 = c1 - 2825 / 27648.0;
        const double dc3 = c3 - 18575 / 48384.0;
        const double dc4 = c4 - 13525 / 55296.0;
        const double dc5 = -277 / 14336.0;
        const double dc6 = c6 - 0.25;
        #endregion

        double[] y;
        double[] state;
        double[] d;
        double[] yErr;
        double[] yScale;
        double[] k1, k2, k3, k4, k5, k6;

        public RKCKIntegrator()
        {
            SetStateLength(0);
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        protected override void SetStateLength(int length)
        {
            y = new double[length];
            state = new double[length];
            d = new double[length];
            yErr = new double[length];
            yScale = new double[length];
            k1 = new double[length];
            k2 = new double[length];
            k3 = new double[length];
            k4 = new double[length];
            k5 = new double[length];
            k6 = new double[length];
        }


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

            double eps;
            double err;
            double errMax;
            double hTemp;

            eps = Tolerance;

            double h = IntegrationTimeStep;
            if ((currentTime + h) > endTime)
                h = endTime - currentTime;

            // Initial state
            DoGetState(y);
            DoGetDerivatives(currentTime, d);


            //-- set scaling values --//
            for (int i = 0; i < n; i++)
                yScale[i] = Math.Abs(y[i]) + Math.Abs(d[i] * h) + TINY;

            while (true)
            {
                //-------------------------------------------------------------
                // Call the actual integrator to take a step.
                // This returns the local truncation estimate for the
                // step in yErr.  It also updates the state variables values
                // pointed to in the stateVar array
                //-------------------------------------------------------------
                _IntRKCK(h, n, d, yErr, currentTime);

                //-- evaluate accuracy --//
                errMax = 0.0;
                for (int i = 0; i < n; i++)
                {
                    err = Math.Abs(yErr[i] / yScale[i]);
                    if (err > errMax)
                        errMax = err;
                }

                errMax = errMax / eps;          // scale relative to required tolerance

                if (errMax > 1.0)     // truncation error too large, reduce stepsize
                {
                    hTemp = SAFETY * h * Math.Pow(errMax, PSHRINK);
                    if (h >= 0)
                        h = Math.Max(hTemp, 0.1 * h);
                    else
                        h = Math.Min(hTemp, 0.1 * h);

                    if (h < rkvMinTimeStep)
                    {
                        throw new Exception("Fatal stepsize underflow in RKCK()");
                    };

                    //-- return state variables to their initial value before retry --//
                    DoSetState(y);
                    continue;      // for another try
                }
                else  // SUCCESS! step succeeded, compute size of next step
                {
                    if (errMax > ERRCON)
                        IntegrationTimeStep = SAFETY * h * Math.Pow(errMax, PGROW);
                    else
                        IntegrationTimeStep = 5.0 * h;  // increase no more than 5 times orig. value

                    break;
                };
            };

            if (h < 0)
            {
                throw new Exception("stepsize < 0 in RKCK()");
            };

            currentTime = currentTime + h;
        }

        /// <summary>
        /// Go back one integration step.
        /// </summary>
        public override void StepBack()
        {
            DoSetState(y);
        }

        /// <summary>
        /// Performs the actual integration step
        /// </summary>
        /// <param name="h">Step size</param>
        /// <param name="svCount">State length</param>
        /// <param name="d"></param>
        /// <param name="yErr"></param>
        /// <param name="currentTime"></param>
        void _IntRKCK(double h, int svCount, double[] d, double[] yErr, double currentTime)
        {
            //------ Step 1 -------//
            for (int i = 0; i < svCount; i++)
                state[i] = y[i] + b21 * h * d[i];
            DoSetState(state);

            //------ Step 2 -------//
            DoGetDerivatives(currentTime + a2 * h, k2);

            for (int i = 0; i < svCount; i++)
                state[i] = y[i] + h * (b31 * d[i] + b32 * k2[i]);
            DoSetState(state);

            //------ Step 3 -------//
            DoGetDerivatives(currentTime + a3 * h, k3);

            for (int i = 0; i < svCount; i++)
                state[i] = y[i] + h * (b41 * d[i] + b42 * k2[i] + b43 * k3[i]);
            DoSetState(state);

            //------ Step 4 -------//
            DoGetDerivatives(currentTime + a4 * h, k4);

            for (int i = 0; i < svCount; i++)
                state[i] = y[i] + h * (b51 * d[i] + b52 * k2[i] + b53 * k3[i] + b54 * k4[i]);
            DoSetState(state);

            //----- Step 5 --------//
            DoGetDerivatives(currentTime + a5 * h, k5);

            for (int i = 0; i < svCount; i++)
                state[i] = y[i] + h * (b61 * d[i] + b62 * k2[i] + b63 * k3[i] + b64 * k4[i] + b65 * k5[i]);
            DoSetState(state);

            //----- Step 6 --------//
            DoGetDerivatives(currentTime + a6 * h, k6);

            for (int i = 0; i < svCount; i++)
                state[i] = y[i] + h * (c1 * d[i] + c3 * k3[i] + c4 * k4[i] + c6 * k6[i]);
            DoSetState(state);


            //-- estimate error between fourth and fifth order methods --//
            for (int i = 0; i < svCount; i++)
                yErr[i] = h * (dc1 * d[i] + dc3 * k3[i] + dc4 * k4[i] + dc5 * k5[i] + dc6 * k6[i]);
        }
    }

}
