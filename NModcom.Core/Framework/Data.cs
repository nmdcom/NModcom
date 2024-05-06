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
    /// IData is the interface through which data exchange is effectuated.
    /// </summary>
    public interface IData
    {
        /// <summary>
        /// The ISimObj that is the owner of this data. 
        /// </summary>
        ISimObj Owner { get; }

        /// <summary>
        /// Type of data stored in this <see cref="IData"/> instance
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// Provides access to the boolean representation of the data. If the 
        /// data cannot be represented as a boolean, this property
        /// will cause an exception to be thrown.
        /// </summary>
        /// <remarks>The actual value is stored in <see cref="AsInt"/></remarks>
        bool AsBoolean { get; set; }

        /// <summary>
        /// Provides access to the datetime representation of the data. If the 
        /// data cannot be represented as a datetime, this property
        /// will cause an exception to be thrown.
        /// </summary>
        /// <remarks>The actual value is stored in <see cref="AsFloat"/></remarks>
        DateTime AsDateTime { get; set; }

        /// <summary>
        /// Provides access to the floating-point representation of the data. If the 
        /// data cannot be represented as a floating-point number, using this property
        /// will cause an exception to be thrown.
        /// </summary>
        double AsFloat { get; set; }

        /// <summary>
        /// Provides access to the int representation of the data. If the 
        /// data cannot be represented as an int number, using this property
        /// will cause an exception to be thrown.
        /// </summary>
        int AsInt { get; set; }

        /// <summary>
        /// Provides access to the string representation of the data. If the 
        /// data cannot be represented as a string, using this property
        /// will cause an exception to be thrown.
        /// </summary>
        string AsString { get; set; }

        double[] AsFloatArray { get; set; }
    }
}
