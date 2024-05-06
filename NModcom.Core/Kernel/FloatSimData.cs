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
    /// IData implementation that wraps a double precision number. See <see cref="NModcom.SimData"/>.
    /// </summary>
    public class ConstFloatSimData : SimData
    {
        private double value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="value"></param>
        public ConstFloatSimData(ISimObj owner, double value) : base(owner)
        {
            this.value = value;
        }

        public ConstFloatSimData(ISimObj owner, string value) : base(owner)
        {
            this.value = Convert.ToDouble(value);
        }

        public ConstFloatSimData(double value) : this(null, value)
        {
        }

        public ConstFloatSimData(string value) : this(null, Convert.ToDouble(value))
        {
        }

        public ConstFloatSimData(ISimObj owner) : this(owner, 0)
        {
        }

        public override Type DataType => typeof(double);

        /// <summary>
        /// Converts the value of this instance to and from a string.
        /// </summary>
        public override string AsString
        {
            get { return value.ToString(); }
            set { this.value = Convert.ToDouble(value); }
        }

        /// <summary>
        /// Converts the value of this instance to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// Converts the value of this instance to and from a floating-point number.
        /// </summary>
        public override double AsFloat
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Converts the value of this instance to and from an int number.
        /// </summary>
        public override int AsInt
        {
            get { return Convert.ToInt32(value); }
            set { this.value = value; }
        }

        public override double[] AsFloatArray
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

    }
}
