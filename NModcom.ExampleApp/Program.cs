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
    internal class Program
    {
        static void Main()
        {
            ISimEnv simenv = new SimEnv
            {
                StartTime = 0,
                StopTime = 4
            };

            ISimObj w = new Weather();
            simenv.Add(w);

            ISimObj crop = new Crop();
            simenv.Add(crop);

            crop.Inputs["I"].Data = w.Outputs["I"].Data;
            crop.Inputs["k"].Data = new ConstFloatSimData(0.7);
            crop.Inputs["e"].Data = new ConstFloatSimData(3.0);
            crop.Inputs["frleaves"].Data = new ConstFloatSimData(0.5);
            crop.Inputs["SLA"].Data = new ConstFloatSimData(0.002);

            simenv.OutputEvent += Simenv_OutputEvent;

            simenv.Run();

            Console.WriteLine();

        }

        private static void Simenv_OutputEvent(object sender, EventArgs e)
        {
            ISimEnv simenv = sender as ISimEnv;
            Console.WriteLine("{0}\t{1}\t{2}\t{3}"
                , simenv.CurrentTime
                , simenv[0].Outputs[0].Data.AsFloat
                , simenv[1].Outputs[0].Data.AsFloat
                , simenv[1].Outputs[1].Data.AsFloat
                );
        }
    }
}
