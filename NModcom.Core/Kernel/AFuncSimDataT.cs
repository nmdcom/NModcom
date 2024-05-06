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
    /// IData implementation that wraps a T <see cref="Func{T}"/>. See <see cref="NModcom.SimData"/>.
    /// </summary>
    public abstract class AFuncSimData<T> : SimData
    {
        protected Func<T> ValueFunc { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="valueFunc"></param>
        public AFuncSimData(ISimObj owner, Func<T> valueFunc)
            : base(owner)
        {
            this.ValueFunc = valueFunc;
        }

        public AFuncSimData(Func<T> valueFunc)
            : this(null, valueFunc)
        { }

        public override Type DataType => typeof(T);

        public override bool AsBoolean
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        public override DateTime AsDateTime
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        /// <summary>
        /// Converts the value of this instance to a string.
        /// </summary>
        public override string AsString
        {
            get { return ToString(); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        /// <summary>
        /// Converts the value of this instance to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ValueFunc().ToString();
        }

        /// <summary>
        /// Converts the value of this instance to a floating-point number.
        /// </summary>
        public override double AsFloat
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        /// <summary>
        /// Converts the value of this instance to an int number.
        /// </summary>
        public override int AsInt
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        public override double[] AsFloatArray
        {
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }
    }
}
