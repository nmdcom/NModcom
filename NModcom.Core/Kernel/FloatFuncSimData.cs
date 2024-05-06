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
    /// IData implementation that wraps a double precision <see cref="Func{TResult}"/>. See <see cref="NModcom.SimData"/>.
    /// </summary>
    public class FloatFuncSimData : AFuncSimData<double>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="valueFunc"></param>
        public FloatFuncSimData(ISimObj owner, Func<double> valueFunc)
            : base(owner, valueFunc)
        { }

        public FloatFuncSimData(Func<double> valueFunc)
            : this(null, valueFunc)
        { }

        /// <summary>
        /// Converts the value of this instance to a floating-point number.
        /// </summary>
        public override double AsFloat
        {
            get { return ValueFunc(); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        /// <summary>
        /// Converts the value of this instance to an int number.
        /// </summary>
        public override int AsInt
        {
            get { return Convert.ToInt32(ValueFunc()); }
            set { throw new Exception("NOT ALLOWED"); }
        }
    }
}
