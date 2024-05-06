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
    /// SimData implements IData; it is an abstract class and doesn't provide 
    /// data storage.
    /// Type specific SimData objects like  <see cref="NModcom.ConstFloatSimData"/>
    /// and <see cref="ConstStringSimData"/> provide data storage
    /// and can be instantiated. In addition, users can provide their own implementations
    /// of <see cref="IData"/> to provide specialized data exchange.
    /// </summary>
    public abstract class SimData : IData
    {
        private ISimObj owner;

        protected SimData(ISimObj owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// The ISimObj that is the owner of this data. 
        /// </summary>
        public ISimObj Owner
        {
            get { return owner; }
        }

        public abstract Type DataType { get; }

        public virtual bool AsBoolean { get => (AsInt == 1); set => AsInt = (value ? 1 : 0); }

        public virtual DateTime AsDateTime { get => DateTime.FromOADate(AsFloat); set => AsFloat = value.ToOADate(); }

        /// <summary>
        /// Data represented as a float.
        /// </summary>
        public abstract double AsFloat
        {
            get;
            set;
        }

        /// <summary>
        /// Data represented as a string.
        /// </summary>
        public abstract string AsString
        {
            get;
            set;
        }

        /// <summary>
        /// Data represented as an integer.
        /// </summary>
        public abstract int AsInt
        {
            get;
            set;
        }

        public abstract double[] AsFloatArray
        {
            get;
            set;
        }
    }
}
