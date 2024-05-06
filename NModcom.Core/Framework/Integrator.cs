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

using System.Collections;

namespace NModcom
{
    /// <summary>
    /// Interface that must be implemented by classes that provide numerical 
    /// integration services to SimObjs.
    /// </summary>
    public interface IIntegrator
    {
        /// <summary>
        /// The number of ISimObjs that are registered with (and will be integrated by)
        /// this Integrator.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// The ISimObj with this index number (zero-based).
        /// </summary>
        ISimObj this[int index]
        {
            get;
        }

        /// <summary>
        /// The size of the integration steps taken by this integrator. This number
        /// may have a slightly different meaning for different implementations. For 
        /// example, variable-step-size integrators do by definition not have 
        /// a fixed time step.
        /// </summary>
        double IntegrationTimeStep
        {
            get;
            set;
        }

        /// <summary>
        /// Implementation-dependent specification of the integration inaccuracy allowed.
        /// </summary>
        double Tolerance
        {
            get;
            set;
        }

        /// <summary>
        /// Register a ISimObj with this integrator. See <see cref="Remove"/>.
        /// </summary>
        /// <param name="simObj">The ISimObj to be registered with the integrator.</param>
        void Add(ISimObj simObj);

        /// <summary>
        /// Remove (un-register) a ISimObj from this integrator. See <see cref="Add"/>.
        /// </summary>
        /// <param name="simObj"></param>
        void Remove(ISimObj simObj);

        /// <summary>
        /// Remove (un-register) all ISimObjs from this integrator.
        /// </summary>
        void Clear();

        /// <summary>
        /// The GetEnumerator method supports the .NET Framework infrastructure
        /// and is not intended to be used directly from your code.
        /// </summary>
        /// <returns>This method returns an IEnumerator object.</returns>
        IEnumerator GetEnumerator();

        /// <summary>
        /// Called by the simulation environment when the simulation starts.
        /// </summary>
        void StartRun();

        /// <summary>
        /// Called by the simulation environment when the simulation finishes
        /// </summary>
        void EndRun();

        /// <summary>
        /// Perform one integration step.
        /// </summary>
        /// <param name="currentTime">Current simulation time.</param>
        /// <param name="endTime">Maximum time at the end of the simulation step.</param>
        void Step(ref double currentTime, double endTime);

        /// <summary>
        /// Undo the last integration step. This is necessary when iterating to find
        /// the moment when a state event occurred.
        /// </summary>
        void StepBack();

    }



}
