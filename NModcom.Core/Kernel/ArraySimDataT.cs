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
    public class ArraySimData<T> : SimData, IArrayData<T>, IArray
    {
        #region Fields

        private T[] _data;

        #endregion

        #region Properties

        public override Type DataType => typeof(T[]);

        /// <summary>
        /// Provides a string representation of the value. Calls <see cref="ToString"/>.
        /// </summary>
        public override string AsString
        {
            get { return ToString(); }
            set { throw new Exception("Not implemented"); }
        }

        /// <summary>
        /// Throws an exception because this conversion is not useful.
        /// </summary>
        public override double AsFloat
        {
            get { throw new Exception("Not implemented"); }
            set { throw new Exception("Not implemented"); }
        }

        /// <summary>
        /// Throws an exception because this conversion is not useful.
        /// </summary>
        public override int AsInt
        {
            get { throw new Exception("Not implemented"); }
            set { throw new Exception("Not implemented"); }
        }

        public override double[] AsFloatArray
        {
            get { throw new Exception("Not implemented"); }
            set { throw new Exception("Not implemented"); }
        }

        /// <summary>
        /// Gets or sets the value of the i-th element of the array.
        /// </summary>
        public T this[int index]
        {
            get { return _data[index]; }
            set { this._data[index] = value; }
        }

        /// <summary>
        /// The number of elements of the array. When this property is assigned to,
        /// any elements that are part of the new array as well as of the old array, 
        /// keep their value.
        /// </summary>
        public int Length
        {
            get => GetLength();
            set
            {
                if (this._data == null)
                {
                    this._data = new T[value];
                }
                else
                {
                    if (this._data.Length != value)
                    {
                        T[] temp = this._data;
                        this._data = new T[value];
                        for (int i = 0; i < Math.Min(temp.Length, value); i++)
                            this._data[i] = temp[i];
                    }
                }
            }
        }

        public T[] Value
        {
            get { return this._data; }
            set { this._data = value; }
        }

        #endregion

        #region Methods

        public ArraySimData()
            : this(null, new T[0])
        { }

        public ArraySimData(ISimObj owner, T[] data)
            : base(owner)
        {
            this._data = data;
        }

        /// <summary>
        /// Constructs an instance and initializes it with the data represented by
        /// the <paramref name="data"/> argument.
        /// </summary>
        /// <param name="data"></param>
        public ArraySimData(T[] data)
            : this(null, data)
        { }

        public override string ToString()
        {
            return (_data == null) ? string.Empty : string.Join(", ", _data);
        }

        #endregion

        #region .IArray Members

        public object GetElement(int index)
        {
            if (_data != null && index < GetLength())
                return _data[index];
            else
                return null;
        }

        public int GetLength()
        {
            return (_data == null) ? 0 : _data.Length;
        }

        #endregion
    }
}
