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
    /// Uses an integrator and drives it with the default event system.
    /// This allows for larger stepsizes independent of the time events.
    /// </summary>
    public class IntegratorStepper : SimObj
    {
        private IIntegrator integrator;

        public IntegratorStepper(IIntegrator integrator)
        {
            this.integrator = integrator;
        }

        public IIntegrator Integrator
        {
            get { return integrator; }
            set { integrator = value; }
        }

        public override void StartRun()
        {
            base.StartRun();

            if (integrator != null)
            {
                integrator.StartRun();
                SimEnv.RegisterEvent(new TimeEvent(this, this, (int)EventPriority.System, 0, SimEnv.StartTime));
            }
        }

        public override void EndRun()
        {
            base.EndRun();

            if (integrator != null)
                integrator.EndRun();
        }

        public override void HandleEvent(ISimEvent simEvent)
        {
            base.HandleEvent(simEvent);

            double time = SimEnv.CurrentTime;
            if (integrator != null)
            {
                integrator.Step(ref time, SimEnv.StopTime);
                SimEnv.RegisterEvent(new TimeEvent(this, this, (int)EventPriority.System, 0, time));
            }
        }

    }
}
