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
using System.IO;
using NUnit.Framework;
using NModcom;
using NModcom.Util;
using NModcom.Examples;


namespace NModcom.Tests
{
    [TestFixture]
    public class SimEnvReaderWriterTests
    {
        private ISimEnv CreateSim()
        {
            // create a simple simulation
            ISimEnv simenv = new SimEnv();
            simenv.StartTime = 3;
            simenv.StopTime = 22;

            ISimObj simobj = new SimObjWithManyInputs();
            simenv.Add(simobj);

            return simenv;
        }

        [Test]
        public void TestManyInputs()
        {
            ISimEnv simenv = CreateSim();
            // write simulation to file
            string sim = SimEnvWriter.WriteJson(simenv);
            File.WriteAllText("testmanyinputs.json", sim); // for debugging

            // create simulation from file
            SimEnvReader sr = new SimEnvReader();
            ISimEnv simenv2 = sr.ReadJson(sim);

            // write the new simulation to file
            string sim2 = SimEnvWriter.WriteJson(simenv2);
            File.WriteAllText("testmanyinputs2.json", sim2); // for debugging

            Assert.That(sim, Is.EqualTo(sim2));
        }

        [Test]
        public void TestManyInputsDataSetToNull()
        {
            ISimEnv simenv = CreateSim();
            for (int i = 0; i < simenv[0].Inputs.Count; i++)
                simenv[0].Inputs[i].Data = null;

            // write simulation to file
            string sim = SimEnvWriter.WriteJson(simenv);
            File.WriteAllText("testmanyinputsnull.json", sim); // for debugging

            // create simulation from file
            SimEnvReader sr = new SimEnvReader();
            ISimEnv simenv2 = sr.ReadJson(sim);

            // write the new simulation to file
            string sim2 = SimEnvWriter.WriteJson(simenv2);
            File.WriteAllText("testmanyinputs2null.json", sim2); // for debugging

            Assert.That(sim, Is.EqualTo(sim2));
        }

        [Test]
        public void TestManyInputsValueSetToNull()
        {
            ISimEnv simenv = CreateSim();
            for (int i = 0; i < simenv[0].Inputs.Count; i++)
                simenv[0].Inputs[i].Data.AsString = null;

            // write simulation to file
            string sim = SimEnvWriter.WriteJson(simenv);
            File.WriteAllText("testmanyinputsnull.json", sim); // for debugging

            // create simulation from file
            SimEnvReader sr = new SimEnvReader();
            ISimEnv simenv2 = sr.ReadJson(sim);

            // write the new simulation to file
            string sim2 = SimEnvWriter.WriteJson(simenv2);
            File.WriteAllText("testmanyinputs2null.json", sim2); // for debugging

            Assert.That(sim, Is.EqualTo(sim2));
        }

    }
}
