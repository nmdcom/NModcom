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

    /// <summary>
    /// NonDataFieldSimData wraps a member field and makes its value read-only 
    /// accessible through the IData interface.
    /// </summary>
    public class NonDataFieldSimData : IData
    {
        private ISimObj owner;
        private FieldInfo fieldInfo;

        public NonDataFieldSimData(ISimObj owner, FieldInfo fieldInfo)
        {
            this.owner = owner;
            this.fieldInfo = fieldInfo;
        }

        /// <summary>
        /// The ISimObj that is the owner of this data. 
        /// </summary>
        public ISimObj Owner
        {
            get { return owner; }
        }

        public Type DataType => fieldInfo.FieldType;

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        public string AsString
        {
            get
            {
                //TODO
                //				object o = null;
                try
                {
                    //					o = fieldInfo.GetValue(owner); 
                    return fieldInfo.GetValue(owner).ToString();
                }
                catch
                {
                    //					Console.WriteLine(fieldInfo.Name);
                    //					Console.WriteLine(o);
                    return null;
                }
            }
            set { fieldInfo.SetValue(owner, (string)value); }
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        public bool AsBoolean
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(fieldInfo.GetValue(owner));
                }
                catch (Exception)
                {
                    throw new Exception("Cannot convert to boolean: class="
                        + owner.GetType().Name
                        + ", instance=" + owner.Name
                        + ", output=" + fieldInfo.Name + ".");
                }
            }
            set
            {
                throw new Exception("Sorry, you are not allowed to set this value");
            }
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        public DateTime AsDateTime
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(fieldInfo.GetValue(owner));
                }
                catch (Exception)
                {
                    throw new Exception("Cannot convert to datetime: class="
                        + owner.GetType().Name
                        + ", instance=" + owner.Name
                        + ", output=" + fieldInfo.Name + ".");
                }
            }
            set
            {
                throw new Exception("Sorry, you are not allowed to set this value");
            }
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        public double AsFloat
        {
            get
            {
                try
                {
                    return Convert.ToDouble(fieldInfo.GetValue(owner));
                }
                catch (Exception)
                {
                    throw new Exception("Cannot convert to double: class="
                        + owner.GetType().Name
                        + ", instance=" + owner.Name
                        + ", output=" + fieldInfo.Name + ".");
                }
            }
            set
            {
                throw new Exception("Sorry, you are not allowed to set this value");
            }
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        public int AsInt
        {
            get
            {
                try
                {
                    return Convert.ToInt32(fieldInfo.GetValue(owner));
                }
                catch (Exception)
                {
                    throw new Exception("Cannot convert to int: class="
                        + owner.GetType().Name
                        + ", instance=" + owner.Name
                        + ", output=" + fieldInfo.Name + ".");
                }
            }
            set
            {
                throw new Exception("Sorry, you are not allowed to set this value");
            }
        }

        public double[] AsFloatArray
        {
            get
            {
                try
                {
                    return (double[])fieldInfo.GetValue(owner);
                }
                catch (Exception)
                {
                    throw new Exception("Cannot convert to int: class="
                        + owner.GetType().Name
                        + ", instance=" + owner.Name
                        + ", output=" + fieldInfo.Name + ".");
                }
            }
            set
            {
                throw new Exception("Sorry, you are not allowed to set this value");
            }
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return fieldInfo.GetValue(owner).ToString();
        }

    }

}
