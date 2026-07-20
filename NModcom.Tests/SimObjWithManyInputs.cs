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

namespace NModcom.Tests
{
    /// <summary>
    /// A SimObj with a diverse set of inputs, and values for these inputs, to allow testing of SimEnvReader and SimEnvWriter.
    /// </summary>
    public class SimObjWithManyInputs : SimObj
    {
        [Input("iString")]
        IData iString = new ConstStringSimData("abcdefd");

        [Input("iFloat")]
        IData iFloat = new ConstFloatSimData(1.2);

        [Input("iInt")]
        IData iInt = new ConstIntSimData(789);

        [Input("iBoolTrue")]
        IData iBoolTrue = new ConstBoolSimData(true);

        [Input("iBoolFalse")]
        IData iBoolFalse = new ConstBoolSimData(false);

        [Input("iFloatArray")]
        IData iFloatArray = new FloatArraySimData(new double[] { 1.1, 1.2, 1.3 });

        [Input("iIntArray")]
        IData iIntArray = new IntArraySimData(new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21 });

        [Input("iFloatArrayNull")]
        IData iFloatArrayNull = new FloatArraySimData(new double[] { });

        [Input("iIntArrayNull")]
        IData iIntArrayNull = new IntArraySimData(new int[] { });

        [Input("iFloatArray1El")]
        IData iFloatArray1El = new FloatArraySimData(new double[] { 12.3 });

        [Input("iIntArray1El")]
        IData iIntArray1El = new IntArraySimData(new int[] { 742 });
    }
}
