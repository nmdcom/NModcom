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
    /// Abstract class for State events.
    /// Derived classes should override the abstract CheckState method to
    /// signal state transitions.
    /// </summary>
    public abstract class StateEvent : SimEvent, IStateEvent
    {
        public StateEvent(ISimObj source, ISimObj target, int priority, int message) :
            base(source, target, priority, message)
        {
        }

        public abstract bool CheckState(double time);
    }


    /// <summary>
    /// Delegate definition for checking state events.
    /// </summary>
    public delegate bool StateChecker(IStateEvent stateEvent, double time);

    /// <summary>
    /// State event that checks the state by calling a method on a simobj.
    /// Simulation objects should pass a method to the constructor that can check the state.
    /// </summary>
    public class DelegateStateEvent : StateEvent
    {
        private StateChecker stateChecker;

        public DelegateStateEvent(ISimObj source, ISimObj target, int priority, int message, StateChecker stateChecker) :
            base(source, target, priority, message)
        {
            if (stateChecker == null)
                throw new ArgumentNullException();

            this.stateChecker = stateChecker;
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public override bool CheckState(double time)
        {
            return stateChecker(this, time);
        }
    }
}
