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

namespace NModcom
{
    public interface IArray
    {
        object GetElement(int index);
        int GetLength();
    }

    /// <summary>
    /// Interface for simulation data objects that wrap an array of doubles.
    /// </summary>
    public interface IFloatArray : IArrayData<double>
    { }

    /// <summary>
    /// IData implementation that wraps an array of double-precision numbers. 
    /// </summary>
    public class FloatArraySimData : SimData, IFloatArray, IArray
    {
        private double[] value;

        public FloatArraySimData(ISimObj owner, double[] value) : base(owner)
        {
            this.value = value;
        }

        /// <summary>
        /// Constructs an instance and initializes it with the data represented by
        /// the value argument.
        /// </summary>
        /// <param name="value">The data with which this object will be initialized.
        /// A comma-separated string of the form "1.2,4.5,9.43" is expected.</param>
        //TODO: handle localized string info; or make sure Convert.ToDouble() 
        // always uses decimal points.
        public FloatArraySimData(string value) : base(null)
        {
            string[] x = value.Split(',');
            this.value = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
                this.value[i] = Convert.ToDouble(x[i]);
        }

        /// <summary>
        /// Constructs an instance and initializes it with the data represented by
        /// the value argument.
        /// </summary>
        /// <param name="value"></param>
        public FloatArraySimData(double[] value) : this(null, value)
        {
        }

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
            if (value == null)
                return "";
            else
            {
                string s = "\"";
                for (int i = 0; i < value.Length; i++)
                {
                    if (i != 0)
                        s = s + ",";
                    s = s + value[i].ToString();
                }
                return s + "\"";
            }
        }

        public override Type DataType => typeof(double[]);

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
            get { return value; }
            set { throw new Exception("Not implemented");}
        }

        /// <summary>
        /// Gets or sets the value of the i-th element of the array.
        /// </summary>
        public double this[int index]
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
                if (value == null)
                    return 0;
                else
                    return value.Length;
            }
            set
            {
                if (this.value == null)
                {
                    this.value = new double[value];
                }
                else
                {
                    if (this.value.Length != value)
                    {
                        double[] temp = this.value;
                        this.value = new double[value];
                        for (int i = 0; i < Math.Min(temp.Length, value); i++)
                            this.value[i] = temp[i];
                    }
                }
            }
        }

        /// <summary>
        /// The entire array of values. Note: if you need to access the elements of the 
        /// array, it may be better to use the default property.
        /// </summary>
        public double[] Value
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

    public class FloatArrayOutput : IData, IFloatArray, IArray
    {
        private ISimObj owner;
        private FieldInfo fieldInfo;

        public FloatArrayOutput(ISimObj owner, FieldInfo fieldInfo)
        {
            this.owner = owner;
            this.fieldInfo = fieldInfo;
        }

        #region IData Members

        public ISimObj Owner
        {
            get
            {
                return owner;
            }
        }

        public Type DataType => fieldInfo.FieldType;

        public bool AsBoolean
        {
            get
            {
                throw new Exception("NOT ALLOWED");
            }
            set
            {
                throw new Exception("NOT ALLOWED");
            }
        }

        public DateTime AsDateTime
        {
            get
            {
                throw new Exception("NOT ALLOWED");
            }
            set
            {
                throw new Exception("NOT ALLOWED");
            }
        }

        public double AsFloat
        {
            get
            {
                throw new Exception("NOT ALLOWED");
            }
            set
            {
                throw new Exception("NOT ALLOWED");
            }
        }

        public int AsInt
        {
            get
            {
                throw new Exception("NOT ALLOWED");
            }
            set
            {
                throw new Exception("NOT ALLOWED");
            }
        }

        /// <summary>
        /// Returns a string of the form "{1,2,3,4}".
        /// </summary>
        public string AsString
        {
            get
            {
                double[] x = (double[])fieldInfo.GetValue(owner);
                if (x == null)
                    return "";
                else
                {
                    string s = "\"";
                    for (int i = 0; i < x.Length; i++)
                    {
                        if (i != 0)
                            s = s + ",";
                        s = s + x[i];
                    }
                    return s + "\"";
                }
            }
            set
            {
                throw new Exception("NOT ALLOWED");
            }
        }

        public double[] AsFloatArray
        {
            get
            {
                return (double[])fieldInfo.GetValue(owner);
            }
            set
            {
                throw new Exception("NOT IMPLEMENTED YET");
            }
        }


        #endregion

        #region IFloatArray Members

        public double this[int index]
        {
            get
            {
                return ((double[])fieldInfo.GetValue(owner))[index];
            }
            set
            {
                throw new Exception("NOT ALLOWED");
            }
        }

        public int Length
        {
            get
            {
                return ((double[])fieldInfo.GetValue(owner)).Length;
            }
            set
            {
                throw new Exception("NOT ALLOWED");
            }
        }

        public double[] Value
        {
            get
            {
                return (double[])fieldInfo.GetValue(owner);
            }
            set
            {
                throw new Exception("NOT ALLOWED");
            }
        }

        #endregion

        #region IArray Members

        public object GetElement(int index)
        {
            return ((double[])fieldInfo.GetValue(owner))[index];
        }

        public int GetLength()
        {
            return ((double[])fieldInfo.GetValue(owner)).Length;
        }

        #endregion
    }
}
