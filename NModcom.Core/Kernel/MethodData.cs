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
using System.Reflection;

namespace NModcom.Kernel
{
    /// <summary>
    /// Wraps a function that takes no arguments and returns a double.
    /// </summary>
    public class MethodData : IData
    {
        ISimObj owner;
        MethodInfo method;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="owner">The ISimObj that provides the method from which the 
        /// value returned by this object is obtained.</param>
        /// <param name="method">The MethodInfo object that describes the method that 
        /// will be called.</param>
        public MethodData(ISimObj owner, MethodInfo method)
        {
            this.owner = owner;
            this.method = method;
        }

        public Type DataType => method.ReturnType;

        public double[] AsFloatArray
        {
            get { throw new Exception("Not implemented"); }
            set { throw new Exception("Not implemented"); }
        }
        #region IData Members

        /// <summary>
        /// The ISimObj that is the owner of this data. 
        /// </summary>
        public ISimObj Owner
        {
            get
            {
                return owner;
            }
        }

        /// <summary>
        /// Provides the value of this instance as a <see cref="bool"/>.
        /// </summary>
        public bool AsBoolean
        {
            get
            {
                return (bool)method.Invoke(owner, null);
            }
            set
            {
                throw new Exception("This is a read-only data object.");
            }
        }

        /// <summary>
        /// Provides the value of this instance as a <see cref="DateTime"/>.
        /// </summary>
        public DateTime AsDateTime
        {
            get
            {
                return (DateTime)method.Invoke(owner, null);
            }
            set
            {
                throw new Exception("This is a read-only data object.");
            }
        }

        /// <summary>
        /// Provides the value of this instance as a floating-point number.
        /// </summary>
        public double AsFloat
        {
            get
            {
                return (double)method.Invoke(owner, null);
            }
            set
            {
                throw new Exception("This is a read-only data object.");
            }
        }

        /// <summary>
        /// Attempts to convert the value of this instance to an int. Note: this will typically
        /// fail because objects of this class are meant to wrap methods that return
        /// a double.
        /// </summary>
        public int AsInt
        {
            get
            {
                return Convert.ToInt32(method.Invoke(owner, null));
            }
            set
            {
                throw new Exception("This is a read-only data object.");
            }
        }

        /// <summary>
        /// Converts the value of this instance to a string.
        /// </summary>
        /// <returns></returns>
        public string AsString
        {
            get
            {
                return method.Invoke(owner, null).ToString();
            }
            set
            {
                throw new Exception("This is a read-only data object.");
            }
        }

        #endregion
    }
}
