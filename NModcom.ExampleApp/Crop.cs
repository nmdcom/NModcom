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

namespace NModcom.ExampleApp
{
    public class Crop : SimObj, IOdeProvider
    {
        [Input("k")] IData k;
        [Input("e")] IData e;
        [Input("I")] IData I;
        [Input("frleaves")] IData f;
        [Input("SLA")] IData sla;

        [Output("B")] IData B = new ConstFloatSimData(0);
        [Output("L")] IData L = new ConstFloatSimData(0);

        public int GetCount() { return 2; }

        public void GetDerivatives(double[] deriv, int index)
        {
            double g = I.AsFloat * e.AsFloat * (1 - Math.Exp(-k.AsFloat * L.AsFloat));
            deriv[index] = g;
            deriv[index + 1] = g * f.AsFloat * sla.AsFloat;
        }

        public void GetState(double[] state, int index) { state[index] = B.AsFloat; state[index + 1] = L.AsFloat; }

        public void SetState(double[] state, int index) { B.AsFloat = state[index]; L.AsFloat = state[index + 1]; }

        public override void StartRun() { B.AsFloat = 1000; L.AsFloat = 0.3; }
    }
}