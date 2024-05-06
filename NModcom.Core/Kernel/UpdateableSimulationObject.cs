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
    /// Base class for component models that implement IUpdateable.
    /// </summary>
    public class UpdateableSimObj : SimObj, IDiscreteModel
    {
        private double startTime;
        private double stopTime;
        private double timeStep;
        private UpdateMethod updateMethod;
        private int priority;

        public UpdateableSimObj()
        {
            startTime = 0;
            stopTime = 0;
            timeStep = 1;
            priority = (int)EventPriority.Update;
            updateMethod = UpdateMethod.Recurring;
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

    }
}
