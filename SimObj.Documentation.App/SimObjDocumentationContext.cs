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
using System.Runtime.Loader;

namespace SimObj.Documentation.App
{
    public class SimObjDocumentationContextt : AssemblyLoadContext
    {
        private readonly string _location;
        private readonly AssemblyDependencyResolver _resolver;

        public SimObjDocumentationContextt(string name, string location, bool isCollectible = false)
            : base(name, isCollectible)
        {
            _location = location;
            _resolver = new AssemblyDependencyResolver(location);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name.Equals("NModcom", StringComparison.InvariantCultureIgnoreCase))
                return null;

            var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

            if (string.IsNullOrEmpty(assemblyPath))
                assemblyPath = Path.Combine(_location, $"{assemblyName.Name}.dll");

            if (File.Exists(assemblyPath))
                return LoadFromAssemblyPath(assemblyPath);
            else
                return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);

            if (libraryPath != null)
                return LoadUnmanagedDllFromPath(libraryPath);

            return IntPtr.Zero;
        }
    }
}
