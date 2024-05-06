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
    /// IData implementation that wraps an int. See <see cref="NModcom.SimData"/>.
    /// </summary>
    public class ConstIntSimData : SimData
    {
        private int value;

        public ConstIntSimData(ISimObj owner, int value) : base(owner)
        {
            this.value = value;
        }

        public ConstIntSimData(ISimObj owner, string value) : base(owner)
        {
            this.value = Convert.ToInt32(value);
        }

        public ConstIntSimData(int value) : this(null, value)
        {
        }

        public ConstIntSimData(string value) : this(null, Convert.ToInt32(value))
        {
        }

        public ConstIntSimData(ISimObj owner) : this(owner, 0)
        {
        }

        public override Type DataType => typeof(int);

        /// <summary>
        /// Converts the value of this instance to and from a string.
        /// </summary>
        public override string AsString
        {
            get { return value.ToString(); }
            set { this.value = Convert.ToInt32(value); }
        }

        /// <summary>
        /// Returns a string representation of the value of this instance.
        /// </summary>
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
            set { this.value = Convert.ToInt32(value); }
        }

        public override double[] AsFloatArray
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        /// <summary>
        /// The int value of this instance.
        /// </summary>
        public override int AsInt
        {
            get { return value; }
            set { this.value = value; }
        }

    }
}
