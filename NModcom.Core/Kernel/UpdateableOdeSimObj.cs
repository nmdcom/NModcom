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

using NModcom.Framework;

namespace NModcom
{
    /// <summary>
    /// This class is the easiest parent class to use if you are writing a simple
    /// rate-based model. 
    /// </summary>
    /// <remarks>
    /// Derived classes must override <see cref="GetRates"/> to provide rate calculations.
    /// Derived classes may want to override <see cref="StartRun"/> and 
    /// EndRun as well as AfterIntegration.
    /// </remarks>
    public abstract class UpdateableOdeSimObj : SimObj, IOdeProvider, IDiscreteModel
    {
        private double startTime;
        private double stopTime;
        private double timeStep;
        private UpdateMethod updateMethod;
        private int priority;

        public UpdateableOdeSimObj()
        {
            startTime = 0;
            stopTime = 0;
            timeStep = 1;
            priority = (int)EventPriority.Update;
            updateMethod = UpdateMethod.Recurring;
        }

        /// <summary>
        /// Called by the simulation environment just before the simulation starts.
        /// </summary>
        public override void StartRun()
        {
            // indicate that we need to count the number of state variables
            cStateVar = -1;
        }

        private void CountStates()
        {
            cStateVar = 0;
            foreach (StateAndRateFields f in stateInfo)
            {
                if (f.State.FieldType == typeof(double))
                    cStateVar++;
                else if (f.State.FieldType == typeof(double[]))
                {
                    double[] field = (double[])f.State.GetValue(this);
                    if (field != null) // 
                        cStateVar = cStateVar + field.Length;
                }
            }
        }
        public double StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public double StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }

        public double TimeStep
        {
            get { return timeStep; }
            set { timeStep = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public UpdateMethod UpdateMethod
        {
            get { return updateMethod; }
            set { updateMethod = value; }
        }


        #region IOdeProvider Members

        public int GetCount()
        {
            if (cStateVar < 0)
                CountStates();
            return cStateVar;
        }

        public void GetState(double[] state, int index)
        {
            int i = 0;
            foreach (StateAndRateFields f in stateInfo)
            {
                if (f.State.FieldType == typeof(double))
                {
                    state[index + i] = (double)f.State.GetValue(this);
                    i++;
                }
                else // only other possibility is double[]
                {
                    double[] s = (double[])f.State.GetValue(this);
                    for (int j = 0; j < s.Length; j++)
                    {
                        state[index + i] = s[j];
                        i++;
                    }
                }
            }
        }

        public void SetState(double[] state, int index)
        {
            int i = 0;
            foreach (StateAndRateFields f in stateInfo)
            {
                if (f.State.FieldType == typeof(double))
                {
                    f.State.SetValue(this, state[index + i]);
                    i++;
                }
                else // only other possibility is double[]
                {
                    double[] s = (double[])f.State.GetValue(this);
                    for (int j = 0; j < s.Length; j++)
                    {
                        s[j] = state[index + i];
                        i++;
                    }
                    f.State.SetValue(this, s);
                }
            }
        }

        /// <summary>
        /// Calls <see cref="GetRates"/>.
        /// </summary>
        /// <param name="deriv"></param>
        /// <param name="index"></param>
        public void GetDerivatives(double[] deriv, int index)
        {
            // calculate rates
            GetRates();

            // put rate values into array
            int i = 0;
            foreach (StateAndRateFields f in stateInfo)
            {
                if (f.State.FieldType == typeof(double))
                {
                    deriv[index + i] = (double)f.Rate.GetValue(this);
                    i++;
                }
                else // only other possibility is double[]
                {
                    double[] s = (double[])f.Rate.GetValue(this);
                    for (int j = 0; j < s.Length; j++)
                    {
                        deriv[index + i] = s[j];
                        i++;
                    }
                }
            }

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public abstract void GetRates();

    }
}
