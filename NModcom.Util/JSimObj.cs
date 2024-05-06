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
using System.Text;
using System.Reflection;

namespace NModcom.Util
{
    public class JSimObj
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Assembly { get; set; }
        public List<JInput> Inputs { get; set; }

        public JSimObj() { }
        public JSimObj(ISimObj simobj)
        {
            Name = simobj.Name;
            Assembly = simobj.GetType().Assembly.GetName().Name; // FKVE not sure if this correct
            Class = simobj.GetType().FullName;

            Inputs = new List<JInput>();
            // Note that we don't write info about the inputs now. That will be done later (but only for the inputs 
            // that are not connected to an output from another SimObj in the simulation).
        }
    }

   
}
