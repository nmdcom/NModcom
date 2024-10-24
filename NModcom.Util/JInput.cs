﻿/*
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

namespace NModcom.Util
{
    public class JInput
    {
        public string DataType { get; set; }
        public string Class { get; set; }
        public string Assembly { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public JInput() { }
        public JInput(IInput input, ISimObj simobj)
        {
            Name = input.Name;

            IData d = input.Data;
            if (d != null)
            {
                Assembly = d.GetType().Assembly.GetName().Name; // FKVE not sure if this correct
                Class = d.GetType().FullName;
                Value = d.AsString;
                DataType = "IDATA";

                // Setting field value in case of param attributes
                foreach (FieldInfo fieldInfo in simobj.GetType().GetFields())
                {
                    Object[] simAttributes = fieldInfo.GetCustomAttributes(true);

                    if (simAttributes.Length > 0)
                    {
                        Object simAttr = simAttributes[0];

                        if (simAttr.GetType() == typeof(ParamAttribute))
                        {
                            if (((ParamAttribute)simAttr).Name == input.Name)
                            {
                                Value = fieldInfo.GetValue(simobj).ToString();
                            }
                        }
                    }
                }
            }
        }
    }

   
}
