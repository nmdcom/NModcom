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
using System.Collections.Generic;
using System.Data;
using System.IO;
using NModcom.Util;
using NModcom.Examples;
using NUnit.Framework;

namespace NModcom.Tests
{
    [TestFixture]
    public class SimOutput
    {
        List<double> t;
        List<double> d;

        const double delta = 1E-9;

        double[] expected_output = new double[] { 1.00, 1.10, 1.21 };

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// The output function should be triggered at the start of the simulation and at 
        /// after each integration step. We check this with a simple exponential growth model
        /// for which we can easily calculate model state for the first few time steps.
        /// </summary>
        [Test]
        public void Test1()
        {
            const double startTime = 0.0;
            const double stopTime = 5.0;
            const double step = 1.0;

            t = new List<double>();
            d = new List<double>();

            ISimEnv simenv = new SimEnv() { StartTime = startTime, StopTime = stopTime };
            simenv.Integrator.IntegrationTimeStep = step;

            ISimObj prey = new PredOrPrey();
            simenv.Add(prey);
            prey.Inputs["density"].Data.AsFloat = 1;
            prey.Inputs["rgr"].Data.AsFloat = 0.1;
            prey.Inputs["k"].Data.AsFloat = 0; // this will switch off the influence of the other species
            prey.Inputs["density of other species"].Data.AsFloat = 0;

            simenv.OutputEvent += Simenv_OutputEvent;

            simenv.Run();

            // check the output times
            Assert.AreEqual(startTime,            t[0], delta);
            Assert.AreEqual(startTime + step * 1, t[1], delta);
            Assert.AreEqual(startTime + step * 2, t[2], delta);

            // check the output values
            Assert.AreEqual(expected_output[0], d[0], delta); // initial value
            Assert.AreEqual(expected_output[1], d[1], delta);
            Assert.AreEqual(expected_output[2], d[2], delta);
        }

        private void Simenv_OutputEvent(object sender, EventArgs e)
        {
            ISimEnv simenv = sender as ISimEnv;
            double _t = simenv.CurrentTime;
            double _d = simenv[0].Outputs["density"].Data.AsFloat;
            t.Add(_t);
            d.Add(_d);
            Console.WriteLine("time={0}  density={1}", _t, _d);
        }

        [Test]
        public void Test2()
        {
            const double startTime = 0.0;
            const double stopTime = 5.0;
            const double step = 1.0;

            t = new List<double>();
            d = new List<double>();

            ISimEnv simenv = new SimEnv() { StartTime = startTime, StopTime = stopTime };
            simenv.Integrator.IntegrationTimeStep = step;

            ISimObj prey = new PredOrPrey();
            simenv.Add(prey);
            prey.Inputs["density"].Data.AsFloat = 1;
            prey.Inputs["rgr"].Data.AsFloat = 0.1;
            prey.Inputs["k"].Data.AsFloat = 0; // this will switch off the influence of the other species
            prey.Inputs["density of other species"].Data.AsFloat = 0;

            string fn = "output.csv";
            CSVOutputWriter ow = new CSVOutputWriter(fn);
            ow.AddSimEnv(simenv);

            simenv.Run();

            ow.Close();

            //DataTable table = ow.GetTable();
            //DataRowCollection rows = table.Rows;
            //DataRow row = rows[0];
            //object o = row[0];

            // open file and read header line
            StreamReader sr = new StreamReader(fn);
            sr.ReadLine();

            // now read output lines and check
            char sep = ',';
            string line;
            for (int i = 0; i < 3; i++)
            {
                line = sr.ReadLine();
                string[] cols = line.Split(sep);

                // check the output times
                Assert.AreEqual(startTime + i, Convert.ToDouble(cols[1]), delta);

                //// check the output values
                Assert.AreEqual(expected_output[i], Convert.ToDouble(cols[2]), delta); 
            }
            sr.Close();

        }
    }
}
