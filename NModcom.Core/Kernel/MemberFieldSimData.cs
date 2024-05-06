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
    public class MemberFieldSimData : SimData, IBool
    {
        #region Fields

        private readonly FieldInfo _fieldInfo;

        #endregion

        #region Methods

        /// <summary>
        /// <see cref="SimData"/> object wrapping a memberfield of <paramref name="owner"/>.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="fieldInfo">FieldInfo of the memberfield</param>
        public MemberFieldSimData(ISimObj owner, FieldInfo fieldInfo)
            : base(owner)
        {
            _fieldInfo = fieldInfo;
        }

        public override Type DataType => _fieldInfo.FieldType;

        /// <summary>
        /// Converts the value of this instance to and from a string.
        /// </summary>
        public override string AsString
        {
            get { return this.ToString(); }
            set { SetValue(typeof(string), nameof(AsString), value); }
        }

        /// <summary>
        /// Converts the value of this instance to and from a boolean value.
        /// </summary>
        public override bool AsBoolean
        {
            get { return Convert.ToBoolean(GetValue(typeof(bool), nameof(AsBoolean))); }
            set { SetValue(typeof(bool), nameof(AsBoolean), value); }
        }
        bool IBool.Value { get => AsBoolean; set => AsBoolean = value; }

        /// <summary>
        /// Converts the value of this instance to and from a datetime value.
        /// </summary>
        public override DateTime AsDateTime
        {
            get { return Convert.ToDateTime(GetValue(typeof(DateTime), nameof(AsDateTime))); }
            set { SetValue(typeof(DateTime), nameof(AsDateTime), value); }
        }

        /// <summary>
        /// Converts the value of this instance to and from a floating-point number.
        /// </summary>
        public override double AsFloat
        {
            get { return Convert.ToDouble(GetValue(typeof(double), nameof(AsFloat))); }
            set { SetValue(typeof(double), nameof(AsFloat), value); }
        }

        /// <summary>
        /// Converts the value of this instance to and from an int number.
        /// </summary>
        public override int AsInt
        {
            get { return Convert.ToInt32(GetValue(typeof(int), nameof(AsInt))); }
            set { SetValue(typeof(int), nameof(AsInt), value); }
        }

        public override double[] AsFloatArray
        {
            get { return (double[])GetValue(typeof(double[]), nameof(AsFloatArray)); }
            set { SetValue(typeof(double[]), nameof(AsFloatArray), value); }
        }

        /// <summary>
        /// Converts the value of this instance to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetValue()?.ToString();
        }

        #region .Supporting

        private object GetValue() => _fieldInfo.GetValue(Owner);

        private object GetValue(Type type, string propertyName)
        {
            if (DataType != type)
                throw new FieldAccessException($"Getting value through {propertyName} not allowed for memberfield of type {DataType.Name}");

            return GetValue();
        }

        private void SetValue(Type type, string propertyName, object value)
        {
            if (DataType != type)
                throw new FieldAccessException($"Setting value through {propertyName} not allowed for memberfield of type {DataType.Name}");

            _fieldInfo.SetValue(Owner, value);
        }

        #endregion

        #endregion
    }
}
