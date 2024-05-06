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
using System.Reflection;

namespace NModcom.Util
{
    /// <summary>
    /// RunInfo provides some general run-time utilities to classes.
    /// </summary>
    public class RunInfo
    {
        private static RunInfo runInfo;
        private string[] path;

        public RunInfo()
        {
            // we want at most one instance of this class
            if (runInfo != null)
                throw new Exception("use static method Info() to instantiate class " + this.GetType().FullName);

            // the default search path for assemblies is 
            // the current directory and in the directory where NModcom.dll was loaded from
            path = new string[]
            {
                ".", 
                (new System.IO.FileInfo(this.GetType().Module.FullyQualifiedName).Directory.ToString())
            };
        }

        public static RunInfo Info()
        {
            if (runInfo == null)
                runInfo = new RunInfo();
            return runInfo;
        }

        public string[] AssemblyPath
        {
            get { return path; }
            set { path = value; }
        }

        public Assembly LoadAssembly(string assemblyFile)
        {
            if (!assemblyFile.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                assemblyFile += ".dll";
            // locate the file
            string filename = null;
            for (int i = 0; i < path.Length; i++)
            {
                filename = Path.Combine(path[i],  assemblyFile);
                //Console.WriteLine(".. trying \"" + filename + "\"");
                if (System.IO.File.Exists(filename))
                    break;
                else
                    filename = null;
            }

            // did we succeed in locating the file?
            if (filename == null)
            {
                string p = "";
                for (int i = 0; i < path.Length; i++)
                    p += path[i] + ";";
                throw new Exception("Cannot find assembly \"" + assemblyFile + "\" in path " + p);
            }

            // now load the assembly 
            Assembly assembly = Assembly.LoadFrom(filename);
            if (assembly == null)
                throw new Exception("Failed to load assembly from " + filename);

            return assembly;
        }

        public object CreateInstance(string assemblyFile, string className)
        {
            Assembly assembly = LoadAssembly(assemblyFile);
            object obj = assembly.CreateInstance(className, true);
            if (obj == null)
                throw new Exception("Failed to load class " + className + " from assembly " + assemblyFile);
            return obj;
        }

    }
}
