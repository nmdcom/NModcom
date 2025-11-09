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
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// This test creates a simulation, serializes it to a JSON file, deserializes from the same file, serializes 
        /// to a second file, and compares the two files. The test is failed when the two files have dissimilar content.
        /// </summary>
        [Test]
        public void TestJson()
        {
            // create a simple simulation
            ISimEnv simenv = CreateSimEnv();

            // write simulation to file
            string sim = SimEnvWriter.WriteJson(simenv);
            Console.WriteLine(sim);
            SimEnvWriter.WriteJson(simenv, "pp.json");

            // create simulation from file
            SimEnvReader sr = new SimEnvReader();
            ISimEnv simenv2 = sr.LoadJson("pp.json");

            // write the new simulation to file
            string sim2 = SimEnvWriter.WriteJson(simenv2);
            SimEnvWriter.WriteJson(simenv2, "pp2.json");

            Assert.That(sim, Is.EqualTo(sim2));
        }

        /// <summary>
        /// This test creates a simulation, serializes it to a YAML file, deserializes from the same file, serializes 
        /// to a second file, and compares the two files. The test is failed when the two files have dissimilar content.
        /// </summary>
        [Test]
        public void TestYaml()
        {
            // create a simple simulation
            ISimEnv simenv = CreateSimEnv();

            // write simulation to file
            string yaml = SimEnvWriter.WriteYaml(simenv);
            Console.WriteLine(yaml);
            SimEnvWriter.WriteYaml(simenv, "pp.yaml");

            // create simulation from file
            SimEnvReader sr = new SimEnvReader();
            ISimEnv simenv2 = sr.LoadYaml("pp.yaml");

            // write the new simulation to file
            string yaml2 = SimEnvWriter.WriteYaml(simenv2);

            Assert.That(yaml, Is.EqualTo(yaml2));
        }

        private ISimEnv CreateSimEnv()
        {
            ISimEnv simenv = new SimEnv();
            simenv.StartTime = 3;
            simenv.StopTime = 22;

            ISimObj prey = new Prey();
            prey.Inputs["rgr"].Data = null;
            simenv.Add(prey);

            ISimObj pred = new Predator();
            simenv.Add(pred);

            prey.Inputs["density of other species"].Data = pred.Outputs["density"].Data;
            pred.Inputs["density of other species"].Data = prey.Outputs["density"].Data;

            return simenv;
        }


    }
}
