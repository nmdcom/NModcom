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
    /// Interface for simulation data objects that wrap an array of ints.
    /// </summary>
    public interface IIntArray : IData
    {

        /// <summary>
        /// Returns the value of the index-th element in the array. The index of the first 
        /// element is always 0.
        /// </summary>
        int this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// The number of elements in the array. The index of the first element is always 0.
        /// </summary>
        int Length
        {
            get; set;
        }

        /// <summary>
        /// The entire array of values. Note: if you need to access the elements of the 
        /// array, it may be better to use the default property.
        /// </summary>
        int[] Value
        {
            get; set;
        }
    }

    /// <summary>
    /// IData implementation that wraps an array of int numbers. 
    /// </summary>
    public class IntArraySimData : SimData, IIntArray, IArray
    {
        private int[] value;

        public IntArraySimData(ISimObj owner, int[] value) : base(owner)
        {
            this.value = value;
        }

        /// <summary>
        /// Constructs an instance and initializes it with the data represented by
        /// the value argument.
        /// </summary>
        /// <param name="value">The data with which this object will be initialized.
        /// A comma-separated string of the form "1.2,4.5,9.43" is expected.</param>
        //TODO: handle localized string info; or make sure Convert.ToInt32() 
        // always uses decimal points.
        public IntArraySimData(string value) : base(null)
        {
            string[] x = value.Split(',');
            this.value = new int[x.Length];
            for (int i = 0; i < x.Length; i++)
                this.value[i] = Convert.ToInt32(x[i]);
        }

        /// <summary>
        /// Constructs an instance and initializes it with the data represented by
        /// the value argument.
        /// </summary>
        /// <param name="value"></param>
        public IntArraySimData(int[] value) : this(null, value)
        {
        }

        public override Type DataType => typeof(int[]);

        /// <summary>
        /// Provides a string representation of the value. Calls <see cref="ToString"/>.
        /// </summary>
        public override string AsString
        {
            get { return ToString(); }
            set { throw new Exception("Not implemented"); }
        }

        /// <summary>
        /// Returns a string of the form "x,y,z,.." where x,y,z,.. are the 
        /// values stored in this array.
        /// </summary>
        /// <returns>A string representation of the values stored.</returns>
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (i != 0)
                    s = s + ",";
                s = s + value[i].ToString();
            }
            return s;
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
            get { throw new Exception("NOT ALLOWED"); }
            set { throw new Exception("NOT ALLOWED"); }
        }

        /// <summary>
        /// Gets or sets the value of the i-th element of the array.
        /// </summary>
        public int this[int index]
        {
            get { return value[index]; }
            set { this.value[index] = value; }
        }

        /// <summary>
        /// The number of elements of the array. When this property is assigned to,
        /// any elements that are part of the new array as well as of the old array, 
        /// keep their value.
        /// </summary>
        public int Length
        {
            get
            {
                return value.Length;
            }
            set
            {
                if (this.value.Length != value)
                {
                    int[] temp = this.value;
                    this.value = new int[value];
                    for (int i = 0; i < Math.Min(temp.Length, value); i++)
                        this.value[i] = temp[i];
                }
            }
        }

        /// <summary>
        /// The entire array of values. Note: if you need to access the elements of the 
        /// array, it may be better to use the default property.
        /// </summary>
        public int[] Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        #region IArray Members

        public object GetElement(int index)
        {
            return value[index];
        }

        public int GetLength()
        {
            return value.Length;
        }

        #endregion
    }
}
