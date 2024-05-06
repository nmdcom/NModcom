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
    /// IData implementation that wraps a string. See <see cref="NModcom.SimData"/>.
    /// </summary>
    public class ConstStringSimData : SimData
    {
        private string value;

        /// <summary>
        /// Implementation of IData that holds its own string value.
        /// </summary>
        /// <param name="owner">The ISimObj that owns this value.</param>
        public ConstStringSimData(ISimObj owner) :
            this(owner, "")
        {
        }

        /// <summary>
        /// Constructs a new instance and sets the value of the wrapped string.
        /// </summary>
        /// <param name="value">The initial value of the wrapped string.</param>
        public ConstStringSimData(string value) :
            this(null, value)
        {
        }

        /// <summary>
        /// Constructs a new instance and sets the value of the wrapped string and
        /// the owner.
        /// </summary>
        /// <param name="owner">The owner of this SimData.</param>
        /// <param name="value">The initial value of the wrapped string.</param>
        public ConstStringSimData(ISimObj owner, string value) :
            base(owner)
        {
            this.value = value;
        }

        public override Type DataType => typeof(string);

        /// <summary>
        /// Representation of the value of this instance 
        /// as a string. 
        /// </summary>
        public override string AsString
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Returns the string value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return value;
        }

        /// <summary>
        /// Representation of the value of this instance 
        /// as a double-precision floating point number.
        /// </summary>
        public override double AsFloat
        {
            get
            {
                return Convert.ToDouble(this.value);
            }
            set
            {
                this.value = value.ToString();
            }
        }

        /// <summary>
        /// Representation of the value of this instance 
        /// as an int number.
        /// </summary>
        public override int AsInt
        {
            get
            {
                return Convert.ToInt32(this.value);
            }
            set
            {
                this.value = value.ToString();
            }
        }

        public override double[] AsFloatArray
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

    }
}
