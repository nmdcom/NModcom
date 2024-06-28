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

using System.Collections;

namespace NModcom
{
    /// <summary>
    /// Interface through which to access information about an input of a <see cref="ISimObj"/>.
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// The ISimObj that is the owner of this data. 
        /// </summary>
        ISimObj Owner
        {
            get;
        }

        /// <summary>
        /// A short, descriptive name of the item. This name is used to effect links between 
        /// SimObj's.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// A description of the item. This is for informational purposes, and may be shown in
        /// a GUI to help the user understand the meaning of the item.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// The units in which the value of the data is expressed. Examples are kg/m2 and ppm.
        /// This is for informational purposes and may be shown in a GUI to help the user 
        /// decide if linking a particular output to a particular input makes sense.
        /// </summary>
        string Units
        {
            get;
        }

        /// <summary>
        /// Reference to the data item. IData is a generic interface; most implementations
        /// will also provide a more specific interface through which the actual data
        /// can be accessed.
        /// </summary>
        IData Data
        {
            get;
            set;
        }

        /// <summary>
        /// The kind of this input. An input is either a parameter, a signal, or an initial value for a state variable.
        /// See <see cref="NModcom.InputKind"/>.
        /// </summary>
        InputKind InputKind
        {
            get;
        }
    }

    /// <summary>
    /// Interface through which to access information about an output of a <see cref="ISimObj"/>.
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// The ISimObj that is the owner of this data. 
        /// </summary>
        ISimObj Owner
        {
            get;
        }

        /// <summary>
        /// A short, descriptive name of the output.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// A short description of the item, for informational purposes.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Reference to the data item. 
        /// </summary>
        IData Data
        {
            get;
        }

        /// <summary>
        /// The kind of output. See <see cref="NModcom.OutputKind"/>.
        /// </summary>
        OutputKind OutputKind
        {
            get;
        }

        string Units
        {
            get;
        }

    }


    /// <summary>
    /// Defines the outputs of a simulation object.
    /// An implementation of this interface, should also implement IEnumerable.
    /// </summary>
    public interface IOutputList
    {
        /// <summary>
        /// The number of outputs in the list.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// An output referenced by its index.
        /// </summary>
        IOutput this[int index]
        {
            get;
        }

        /// <summary>
        /// An output referenced by its name.
        /// </summary>
        IOutput this[string name]
        {
            get;
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator GetEnumerator();
    }


    /// <summary>
    /// Defines the inputs of a simulation object.
    /// An implementation of this interface, should also implement IEnumerable.
    /// </summary>
    public interface IInputList
    {
        /// <summary>
        /// Number of inputs.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// An input referenced by its index.
        /// </summary>
        IInput this[int index]
        {
            get;
        }

        /// <summary>
        /// An input referenced by its name.
        /// </summary>
        IInput this[string name]
        {
            get;
        }

        //UITLEG
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator GetEnumerator();
    }


    /// <summary>
    /// The ISimObj interface specifies core functionality necessary for all objects that participate in a simulation, 
    /// including describing basic information about the object, basic interaction with the SimEnv, 
    /// and data exposure capabilities. All objects that participate in a simulation are required to implement the
    /// ISimObj interface. 
    /// 
    /// Many simulation components have more specialized requirements than is defined by the ISimObj interface. 
    /// For example, many model components are represented as a system of differential equation-based state equations. 
    /// Because all objects of this type will require numerical integration services to be solved, additional interfaces 
    /// are defined that allows general-purpose integrators to solve these object's state equations without the 
    /// modeler having to implement numerical integration methods. But, at a minimum, all objects must implement 
    /// the ISimObj interface. 
    /// </summary>
    public interface ISimObj
    {
        /// <summary>
        /// The name of the instance implementing ISimObj. This need not be unique relative to all other instances of ISimObj. 
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The simulation environment in which the object is registered.
        /// Do not set this property, the simulation environment will set it when
        /// the object is registered.
        /// </summary>
        ISimEnv SimEnv
        {
            get;
            set;
        }

        /// <summary>
        /// A list of inputs.
        /// </summary>
        IInputList Inputs
        {
            get;
        }

        /// <summary>
        /// A list of outputs.
        /// </summary>
        IOutputList Outputs
        {
            get;
        }


        /// <summary>
        /// Called by the simulation environment just before the simulation starts.
        /// </summary>
        void StartRun();

        /// <summary>
        /// This method is called by the SimEnv 
        /// during ISimEnv.Run just after the simulation finishes, i.e. after event processing has completed. A call to this method indicates that the SimObj 
        /// should do whatever it needs to do after a simulation run has completed (e.g. release resources, write data 
        /// to files, etc). This does not indicate that the simulation environment is terminating, only that a simulation run 
        /// has completed.
        /// </summary>
        void EndRun();

        /// <summary>
        /// Called to notify the object that it should handle an event.
        /// Events can be time, state or simulation specific events.
        /// </summary>
        /// <param name="simEvent"></param>
        void HandleEvent(ISimEvent simEvent);
    }

}
