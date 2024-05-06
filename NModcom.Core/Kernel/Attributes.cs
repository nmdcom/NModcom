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
    /// Base class for attribute-classes that specify that a field of 
    /// a ISimObj-implementer is an input or output.
    /// </summary>
    public class SimObjAttribute : System.Attribute
    {
        private string name;
        private string description;
        private string units;

        public SimObjAttribute() : this("?", "?", "?")
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="name">A short, descriptive name for the attribute</param>
        public SimObjAttribute(string name)
            : this(name, "?", "?")
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="name">A short, descriptive name for the attribute</param>
        /// <param name="description">A fairly complete description</param>
        /// <param name="units">The physical units in which the values of the field to which this attribute is applied, are expressed.</param>
        public SimObjAttribute(string name, string description, string units)
        {
            this.name = name;
            this.description = description;
            this.units = units;
        }

        /// <summary>
        /// A short, descriptive name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// A short description of the item, for informational purposes.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// The units in which the value of the data is expressed 
        /// (for example, kg/m2 or ppm).
        /// </summary>
        public string Units
        {
            get { return units; }
            set { units = value; }
        }

    }

    /// <summary>
    /// An input is either a parameter, a signal, or an initial value for a state variable.
    /// A parameter has a value at the beginning of a simulation run and does not change 
    /// during the simulation run. ISimObj implementations may enforce this.
    /// The value of a signal may change during a simulation run. An initial value of a state
    /// variable is used to initialize a state variable before the simulation run starts. 
    /// </summary>
    [FlagsAttribute]
    public enum InputKind : short
    {
        /// <summary>
        /// The input represents the initial value of a state variable.
        /// </summary>
        InitialState = 1,
        /// <summary>
        /// The input represents a parameter of the model implementation.
        /// It as assumed that the value of a parameter input is available during
        /// <see cref="ISimObj.StartRun"/> and doesn't change after that time.
        /// </summary>
        Param = 2,
        /// <summary>
        /// The input represents a signal that is assumed to be varying during
        /// the simulated period.
        /// </summary>
        Signal = 4
    };

    /// <summary>
    /// An output is either a state variable or it is computed by the model in some other way.
    /// </summary>
    public enum OutputKind
    {
        /// <summary>
        /// The output represents the value of a state variable.
        /// </summary>
        State,
        /// <summary>
        /// The output does not represent the value of a state variable.
        /// </summary>
        Other
    };

    /// <summary>
    /// When specified on a field, this creates an input on the input list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InputAttribute : SimObjAttribute
    {
        public InputAttribute(string name)
            : base(name, "?", "?")
        { }

        public InputAttribute(string name, string description, string units)
            : base(name, description, units)
        { }
    }


    /// <summary>
    /// When specified on a field, this creates an output on the output list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class OutputAttribute : SimObjAttribute
    {
        public OutputAttribute(string name)
            : base(name, "?", "?")
        { }

        public OutputAttribute(string name, string description, string units)
            : base(name, description, units)
        { }
    }
}
