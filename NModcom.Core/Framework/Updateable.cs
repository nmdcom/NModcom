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

namespace NModcom.Framework
{
    /// <summary>
    /// An enumerated type to specify the kind of update. 
    /// </summary>
    public enum UpdateMethod
    {
        None,
        Once,
        Recurring
    };

    /// <summary>
    /// IDiscreteModel combines the functionality of discrete-time and discrete-event models.
    /// If UpdateMethod is set to None, no events are registered when the model is added to an ISimEnv (discrete-event)
    /// If UpdateMethod is set to Once, one event is registered .. (discrete-event)
    /// If UpdateMethod is set to Recurring, an event is registered that will be re-registered after is has been handled (discrete-time)
    /// </summary>
    public interface IDiscreteModel
    {

        /// <summary>
        /// TimeStep that will be used.
        /// </summary>
        double TimeStep
        {
            get;
            set;
        }

        /// <summary>
        /// Priority of the Updateable. 
        /// </summary>
        int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// The used updatemethod.
        /// </summary>
        UpdateMethod UpdateMethod
        {
            get;
            set;
        }
    }
}