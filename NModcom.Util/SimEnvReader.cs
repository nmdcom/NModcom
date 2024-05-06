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
using System.Reflection;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using YamlDotNet.Serialization;


namespace NModcom.Util
{
    /// <summary>
    /// NModcom.Util is the namespace where various utility classes 
    /// and interfaces are declared.
    /// </summary>
    class NamespaceDoc
    {
    }

    /// <summary>
    /// ISimEnvReader is the interface of classes that read an XML-description of a 
    /// simulation and instantiate it.
    /// </summary>
    public interface ISimEnvReader
    {
        /// <summary>
        /// Creates a SimEnv and loads it according to the instructions in filename.
        /// </summary>
        /// <param name="filename">Name of the file describing the simulation.</param>
        /// <returns>A SimEnv.</returns>
        public ISimEnv LoadJson(string filename);

        /// <summary>
        /// Creates a SimEnv and loads it according to the instructions in the json argument.
        /// </summary>
        /// <param name="json">JSON string describing the simulation.</param>
        /// <returns>A SimEnv.</returns>
        public ISimEnv ReadJson(string json);
    }

    /// <summary>
    /// This class has a method that reads an XML-description of a simulation, 
    /// instantiates the necessary classes and creates the necessary links, and then
    /// returns a ISimEnv instance ready to be populated with data and run.
    /// </summary>
    public class SimEnvReader : ISimEnvReader
    {

        private void LoadSimObjs(ISimEnv simEnv, JSimEnv jsimenv)
        {
            foreach (JSimObj jsimobj in jsimenv.SimObj)
            {
                // create class according to protocol
                ISimObj simobj = LoadNetClass(jsimobj.Assembly, jsimobj.Class);
                // mark up new instance
                if (simobj == null)
                    throw new Exception("failed to create instance of simobj");

                simobj.Name = jsimobj.Name;
                
                // add new object to SimEnv
                simEnv.Add(simobj, true);
            }
            //Console.WriteLine(simEnv.Count + " objects registered");
        }


        private ISimObj LoadNetClass(string assemblyFile, string classname)
        {
            //Console.WriteLine("loading class \"" + classname + "\" from \"" + assemblyFile + "\"");

            Assembly assembly = RunInfo.Info().LoadAssembly(assemblyFile);

            // instantiate the class
            ISimObj simobj = (assembly.CreateInstance(classname, true) as ISimObj);
            if (simobj == null)
                throw new Exception("Failed to load class " + classname);

            return simobj;
        }

        private void LinkSimObjs(ISimEnv simEnv, JSimEnv jsimenv)
        {
            foreach (JSimObjLink link in jsimenv.SimObjLink)
            {
                // we've got all the info to make one link
                //Console.WriteLine("Establishing link from [{0}].[{1}] to [{2}].[{3}]"
                //    , link.SourceSimObj
                //    , link.SourceName
                //    , link.DestinationSimObj
                //    , link.DestinationName);

                ISimObj dest = simEnv[link.DestinationSimObj];
                if (dest == null) Console.WriteLine("component \"" + link.DestinationSimObj + "\" not found.");

                ISimObj src = simEnv[link.SourceSimObj];
                if (src == null) Console.WriteLine("component \"" + link.SourceSimObj + "\" not found.");

                IInput inp = dest.Inputs[link.DestinationName];
                if (inp == null) Console.WriteLine("input \"" + link.DestinationName + "\" not found.");

                IOutput outp = src.Outputs[link.SourceName];
                if (outp == null) Console.WriteLine("output \"" + link.SourceName + "\" not found.");

                IData d = outp.Data;
                if (d == null) Console.WriteLine("data object is null");

                inp.Data = d;
            }
        }

        private void LinkIntegrators(ISimEnv simEnv, JSimEnv jsimenv)
        {
            // temporary: only look at first integrator, only look at timestep attribute
            if (jsimenv.Integrator != null)
            {
                simEnv.Integrator.IntegrationTimeStep = jsimenv.Integrator.TimeStep;
                Console.WriteLine("setting timestep to " + Convert.ToDouble(jsimenv.Integrator.TimeStep));
            }
        }
        public ISimEnv LoadJson(string filename)
        {
            // load description of simulation
            using (StreamReader sr = new StreamReader(filename))
            {
                string json = sr.ReadToEnd();
                return ReadJson(json);
            }
        }

        public ISimEnv LoadYaml(string filename)
        {
            // load description of simulation
            using (StreamReader sr = new StreamReader(filename))
            {
                string yaml = sr.ReadToEnd();
                return ReadYaml(yaml);
            }
        }

        private JSimEnv DeserializeYaml(string yaml)
        {
            var deserializer = new DeserializerBuilder().Build();
            JSimEnv jsimenv = deserializer.Deserialize<JSimEnv>(yaml);
            return jsimenv;
        }

        private JSimEnv DeserializeJson(string json)
        {
            JSimEnv jsimenv = JsonConvert.DeserializeObject<JSimEnv>(json);
            return jsimenv;
        }

        private ISimEnv JSimEnvToSimEnv(JSimEnv jsimenv)
        {
            ISimEnv simenv = new SimEnv
            {
                StartTime = jsimenv.StartTime,
                StopTime = jsimenv.StopTime,
                Name = jsimenv.Name
            };

            // load all simobj's and add them to the simenv
            LoadSimObjs(simenv, jsimenv);

            // set up integrators
            LinkIntegrators(simenv, jsimenv);

            //// read data
            LoadData(simenv, jsimenv);

            // connect simobj's 
            LinkSimObjs(simenv, jsimenv);

            //// all done - return a ISimEnv that's ready to be connected with data and run
            return simenv;
        }

        public ISimEnv ReadJson(string json)
        {
            JSimEnv jsimenv = DeserializeJson(json);
            ISimEnv simenv = JSimEnvToSimEnv(jsimenv);
            //// all done - return a ISimEnv that's ready to be connected with data and run
            return simenv;
        }

        public ISimEnv ReadYaml(string yaml)
        {
            JSimEnv jsimenv = DeserializeYaml(yaml);
            ISimEnv simenv = JSimEnvToSimEnv(jsimenv);
            //// all done - return a ISimEnv that's ready to be connected with data and run
            return simenv;
        }

        private void LoadData(ISimEnv simEnv, JSimEnv jsimenv)
        {
            //Console.WriteLine("reading data");

            foreach (JSimObj jsimobj in jsimenv.SimObj)
            {
                ISimObj simobj = simEnv[jsimobj.Name];
                if (simobj == null)
                    throw new Exception(string.Format("error: cannot find simobj [{0}]", jsimobj.Name));

                foreach (JInput jinput in jsimobj.Inputs)
                {
                    //Console.WriteLine("input [{0}].[{1}] will get value {2}", jsimobj.Name, jinput.Name, jinput.Value);

                    string datatype = jinput.DataType;
                    if (datatype == null || datatype.Length == 0)
                        datatype = "IDATA";
                    else
                        datatype = datatype.ToUpper();

                    IData data = null;
                    if (datatype.Equals("FLOAT"))
                        data = new ConstFloatSimData(null, jinput.Value);
                    else if (datatype.Equals("STRING"))
                        data = new ConstStringSimData(null, jinput.Value);
                    else if (datatype.Equals("INTEGER"))
                        data = new ConstIntSimData(null, jinput.Value);
                    else if (datatype.Equals("IDATA"))
                    {
                        if (   jinput.Assembly != null
                            && jinput.Assembly.Length > 0
                            && jinput.Class != null
                            && jinput.Class.Length > 0
                        )
                        {
                            Assembly assembly = RunInfo.Info().LoadAssembly(jinput.Assembly);
                            Type t = assembly.GetType(jinput.Class);
                            ConstructorInfo c = t.GetConstructor(new Type[] { typeof(string) });
                            object o = c.Invoke(new object[] { jinput.Value });
                            data = o as IData;
                        }
                    }
                    else
                        throw new Exception("data type \"" + datatype + "\""
                            + "with value=" + jinput.Value
                            + " is not recognized for input \"" + jsimobj.Name + "." + jinput.Name + "\"");

                    IInput input = simobj.Inputs[jinput.Name];
                    if (input == null)
                        throw new Exception(string.Format("error: cannot find input [{0}].[{1}]", jsimobj.Name, jinput.Name));

                    input.Data = data;
                }
            }
        }

    }
}
