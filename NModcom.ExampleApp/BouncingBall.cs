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


using System.Xml.Linq;

namespace NModcom.ExampleApp
{
    /// <summary>
    /// Simple model of a bouncing ball 
    /// From: Fritzson, P. (2020). Modelica: Equation-Based, Object-Oriented Modelling of Physical Systems. 
    /// In: Carreira, P., Amaral, V., Vangheluwe, H. (eds) Foundations of Multi-Paradigm Modelling for 
    /// Cyber-Physical Systems. Springer, Cham. doi: 10.1007/978-3-030-43946-0_3
    /// 
    /// Euler with several time steps
    /// RK
    /// state event (for bouncing)
    /// time event (for explosion?)
    /// </summary>
    public class BouncingBall : SimObj, IOdeProvider
    {
        const double g = 9.81; // gravity constant
        private double c = 0.9; // coefficient of restitution
        private double radius = 0.1; // TODO take this into account in determining the moment of bouncing
        private double height; // height of the ball center
        private double velocity; // velocity of the ball

        [Output("height")]
        IData Height = new ConstFloatSimData(0);

        [Output("velocity")]
        IData Velocity = new ConstFloatSimData(0);

        public override void StartRun()
        {
            velocity = 0;
            height = 1.0;
            SetOutput();

            SimEnv.RegisterEvent(new HitGround(this, this, 0, 0));
        }

        public override void HandleEvent(ISimEvent simEvent)
        {
            if (simEvent is HitGround)
            {
                velocity = -c * velocity;
                SetOutput();
                SimEnv.RegisterEvent(new HitGround(this, this, 0, 0));
            }
        }
        public int GetCount()
        {
            return 2;
        }

        public void GetDerivatives(double[] deriv, int index)
        {
            deriv[index = 0] = velocity; // rate of change of height
            deriv[index + 1] = -g;       // rate of change of velocity
        }

        public void GetState(double[] state, int index)
        {
            state[0] = height;
            state[1] = velocity;
        }

        public void SetState(double[] state, int index)
        {
            height = state[0];
            velocity = state[1];
            SetOutput();
        }

        private void SetOutput()
        {
            Velocity.AsFloat = velocity;
            Height.AsFloat = height;
        }

    }
}