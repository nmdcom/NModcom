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

namespace NModcom
{
    /// <summary>
    /// When specified on a field, it makes that field a state variable, i.e. it makes 
    /// that field an output and sets its <see cref="OutputKind"/> to "State".
    /// This attribute can be applied to fields of type "double" and to 
    /// fields of type "double[]".
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StateAttribute : SimObjAttribute
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="name">A short, descriptive name for the attribute</param>
        /// <param name="description">A fairly complete description</param>
        /// <param name="units">The physical units in which the values of the field to which this attribute is applied, are expressed.</param>
        public StateAttribute(string name, string description, string units)
            : base(name, description, units)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="name">A short, descriptive name for the attribute</param>
        public StateAttribute(string name)
            : this(name, "?", "?")
        {
        }

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class RateAttribute : Attribute
    {
        public readonly string StateField;

        /// <summary>
        /// Constructor to make a new RateAttribute.
        /// </summary>
        /// <param name="StateField">The name of the state-field for which this field holds the rate.</param>
        public RateAttribute(string StateField)
        {
            this.StateField = StateField;
        }
    }
}
