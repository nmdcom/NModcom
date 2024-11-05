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
    internal class SimObjHowToUse
    {
        public static void RunSimulation()
        {
            Console.WriteLine("Simulation with SimEnv and SimObj");

            ISimEnv simenv = new SimEnv()
            {
                StartTime = 0,
                StopTime = 5
            };

            ISimObj mySimObj = new MyFirstSimObj()
            {
                Name = "Test"
            };

            simenv.Add(mySimObj);

            simenv.RegisterEvent(new TimeEvent(mySimObj, mySimObj, 0, 0, 3.0 ));

            simenv.OutputEvent += Simenv_OutputEvent;
            simenv.Run();
        }

        private static void Simenv_OutputEvent(object sender, EventArgs e)
        {
            ISimEnv ?simenv = sender as ISimEnv;
            DateTime dateTime = CalendarTime.ToDateTime(simenv.CurrentTime);
            Console.WriteLine($"{dateTime.ToString("yyyy-MM-dd")}\t{simenv[0].Outputs[0].Data.AsFloat}");

            simenv[0].Outputs[0].Data.AsFloat = 1;
            simenv[0].Outputs["my output"].Data.AsFloat = 1;

        }
    }
}
