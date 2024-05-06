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
using System.Collections;

namespace NModcom
{
    /// <summary>
    /// Base class for IIntegrator implementers.
    /// </summary>
    public abstract class Integrator : IIntegrator, IEnumerable
    {

        private ArrayList odeProviders;
        private double integrationTimeStep;
        private double tolerance;
        private int stateLength;

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        public Integrator()
        {
            odeProviders = new ArrayList();
            stateLength = 0;
            integrationTimeStep = 0.01;
            tolerance = 1e-4;
        }

        public int Count
        {
            get { return odeProviders.Count; }
        }

        public ISimObj this[int index]
        {
            get { return (ISimObj)odeProviders[index]; }
        }

        public double IntegrationTimeStep
        {
            get { return integrationTimeStep; }
            set { integrationTimeStep = value; }
        }

        public double Tolerance
        {
            get { return tolerance; }
            set { tolerance = value; }
        }

        public void Add(ISimObj simObj)
        {
            IOdeProvider ode = simObj as IOdeProvider;
            if (ode == null)
                throw new Exception("Simulation object does not implement IOdeProvider");

            odeProviders.Add(ode);
        }

        public void Remove(ISimObj simObj)
        {
            IOdeProvider ode = simObj as IOdeProvider;
            if (ode == null)
                throw new Exception("Simulation object does not implement IOdeProvider");

            odeProviders.Remove(ode);
        }

        public void Clear()
        {
            odeProviders.Clear();
        }

        public virtual void StartRun()
        {
        }

        public virtual void EndRun()
        {
        }

        public abstract void Step(ref double currentTime, double endTime);


        public abstract void StepBack();

        #region Helper methods for Integrator implementations


        /// <summary>
        /// Integrator implementations should override this to allocate
        /// required integration buffers.
        /// </summary>
        /// <param name="length"></param>
        protected virtual void SetStateLength(int length)
        {
        }

        /// <summary>
        /// Calculates the total state length and updates required buffer sizes.
        /// </summary>
        protected int DoCountStates()
        {
            int count = 0;
            foreach (IOdeProvider ode in odeProviders)
            {
                count += ode.GetCount();
            }

            if (count != stateLength)
            {
                stateLength = count;
                SetStateLength(stateLength);
            }

            return count;
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        protected void DoGetState(double[] state)
        {
            int index = 0;
            foreach (IOdeProvider ode in odeProviders)
            {
                ode.GetState(state, index);
                index += ode.GetCount();
            }
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        protected void DoSetState(double[] state)
        {
            int index = 0;
            foreach (IOdeProvider ode in odeProviders)
            {
                ode.SetState(state, index);
                index += ode.GetCount();
            }
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deriv"></param>
        protected void DoGetDerivatives(double time, double[] deriv)
        {
            int index = 0;
            foreach (IOdeProvider ode in odeProviders)
            {
                ode.GetDerivatives(deriv, index);
                index += ode.GetCount();
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return odeProviders.GetEnumerator();
        }

        #endregion
    }

}
