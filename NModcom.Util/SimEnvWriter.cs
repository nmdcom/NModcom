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

using System.IO;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace NModcom.Util
{
    public class SimEnvWriter
    {
        public static string WriteJson(ISimEnv simenv)
        {
            JSimEnv jsimenv = new JSimEnv(simenv);
            return JsonConvert.SerializeObject(
                  jsimenv
                , Formatting.Indented
                , new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );
        }
        public static void WriteJson(ISimEnv simenv, string filename)
        {
            string json = WriteJson(simenv);
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine(json);
            sw.Close();
        }
        public static string WriteYaml(ISimEnv simenv)
        {
            JSimEnv jsimenv = new JSimEnv(simenv);
            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(jsimenv);
            return yaml;
        }
        public static void WriteYaml(ISimEnv simenv, string filename)
        {
            string yaml = WriteYaml(simenv);
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine(yaml);
            sw.Close();
        }
    }
}
