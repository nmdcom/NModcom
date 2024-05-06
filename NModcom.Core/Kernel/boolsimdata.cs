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
    /// Interface for simulation data objects that wrap a boolean. 
    /// </summary>
    public interface IBool : IData
    {
        /// <summary>
        /// Provides the value of this data object ("true" or "false").
        /// </summary>
        bool Value
        {
            get;
            set;
        }
    }

    /// <summary>
    /// IData implementation that wraps a boolean. See <see cref="NModcom.SimData"/>.
    /// </summary>
    public class ConstBoolSimData : SimData, IBool
    {
        private bool value;

        /// <summary>
        /// Creates a new instance of ConstBoolSimData.
        /// </summary>
        /// <param name="owner">The ISimObj that owns this data.</param>
        /// <param name="value">The value with which this instance is initialized.</param>
        public ConstBoolSimData(ISimObj owner, bool value) : base(owner)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of ConstBoolSimData.
        /// </summary>
        /// <param name="value">If this parameter is "true", the instance will be initialized 
        /// with a boolean "true"; otherwise, it will be initialized with 
        /// boolean "false".</param>
        public ConstBoolSimData(string value) : base(null)
        {
            this.value = value.Equals("true");
        }

        /// <summary>
        /// Creates a new instance of ConstBoolSimData.
        /// </summary>
        /// <param name="value">The value with which this instance is initialized.</param>
        public ConstBoolSimData(bool value) : this(null, value)
        {
        }

        /// <summary>
        /// Creates a new instance of ConstBoolSimData.
        /// </summary>
        /// <param name="owner">The ISimObj that owns this data.</param>
        public ConstBoolSimData(ISimObj owner) : this(owner, true)
        {
        }

        public override Type DataType => typeof(bool);

        /// <summary>
        /// See <see cref="IData"/>. Setting this value will cause an exception to be thrown.
        /// </summary>
        public override string AsString
        {
            get { return ToString(); }
            set { throw new Exception("Cannot do this conversion"); }
        }

        /// <summary>
        /// Returns a string representation of the value. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (value)
                return "true";
            else
                return "false";
        }

        /// <summary>
        /// Throws an exception because the conversion is not useful for this datatype.
        /// </summary>
        public override double AsFloat
        {
            get { throw new Exception("Cannot do this conversion"); }
            set { throw new Exception("Cannot do this conversion"); }
        }

        /// <summary>
        /// Throws an exception because the conversion is not useful for this datatype.
        /// </summary>
        public override int AsInt
        {
            get { return this.value ? 1 : 0; }
            set { this.value = (value > 0) ? true : false; }
        }

        public override double[] AsFloatArray
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        /// <summary>
        /// Provides the value of this data object ("true" or "false").
        /// </summary>
        public bool Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
