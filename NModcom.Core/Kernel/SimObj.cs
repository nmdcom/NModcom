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

using NModcom.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace NModcom
{
    #region InputOutput

    /// <summary>
    /// Base class for the description of an input.
    /// </summary>
    public abstract class Input : IInput
    {
        private ISimObj owner;
        private string name;
        private string description;
        private string units;
        private InputKind inputKind;

        public Input(ISimObj owner, InputKind inputKind, string name, string description, string units)
        {
            this.owner = owner;
            this.name = name;
            this.description = description;
            this.units = units;
            this.inputKind = inputKind;
        }

        /// <summary>
        /// The owner of the input.
        /// </summary>
        public ISimObj Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// A short, descriptive name of the input.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// A short description of the item, for informational purposes.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// The units in which the value of the data is expressed 
        /// (for example, kg/m2 or ppm).
        /// </summary>
        public string Units
        {
            get { return units; }
        }

        /// <summary>
        /// Reference to the data item. 
        /// </summary>
        public abstract IData Data
        {
            get;
            set;
        }

        /// <summary>
        /// The kind of this input. See <see cref="NModcom.InputKind"/>.
        /// </summary>
        public InputKind InputKind
        {
            get { return inputKind; }
        }
    }


    /// <summary>
    /// Holds a pointer to an IData, but that IData is not related to a field of 
    /// the SimObj-derived class.
    /// </summary>
    public class SimpleInput : Input
    {
        private IData data;

        public SimpleInput(ISimObj owner, InputKind inputKind, string name, string description, string units, IData newdata) :
            base(owner, inputKind, name, description, units)
        {
            this.Data = newdata;
        }

        /// <summary>
        /// Reference to the data item. 
        /// </summary>
        public override IData Data
        {
            get { return data; }
            set { data = value; }
        }
    }

    /// <summary>
    /// Base class for the description of an input with data storage in a member of the class
    /// for which the input is defined.
    /// The field that this class wraps implements IData.
    /// </summary>
    public class FieldInput : Input
    {
        protected FieldInfo fieldInfo;

        public FieldInput(ISimObj owner, InputKind inputKind, string name, string description, string units, FieldInfo fieldInfo, IData data) :
            base(owner, inputKind, name, description, units)
        {
            this.fieldInfo = fieldInfo;
            this.Data = data; // this could be a default value specified via an attribute
        }

        /// <summary>
        /// Reference to the data item. 
        /// </summary>
        public override IData Data
        {
            get { return (IData)fieldInfo.GetValue(Owner); }
            set { fieldInfo.SetValue(Owner, (IData)value); }
        }
    }

    /// <summary>
    /// Base class for the description of an output with data storage in a member of the class
    /// for which the output is defined.
    /// </summary>
    public abstract class Output : IOutput
    {
        private ISimObj owner;
        private string name;
        private string description;
        private string units;
        private OutputKind outputKind;
        private Type innerType;

        protected Output(ISimObj owner, OutputKind outputKind, string name, string description, string units, Type innerType)
        {
            this.owner = owner;
            this.name = name;
            this.description = description;
            this.units = units;
            this.outputKind = outputKind;
            this.innerType = innerType;
        }

        /// <summary>
        /// The owner of the output.
        /// </summary>
        public ISimObj Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// A short, descriptive name of the output.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// A short description of the item, for informational purposes.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        public string Units
        {
            get { return units; }
        }

        /// <summary>
        /// Reference to the data item. 
        /// </summary>
        public abstract IData Data
        {
            get;
        }

        /// <summary>
        /// The kind of output. See <see cref="NModcom.OutputKind"/>.
        /// </summary>
        public OutputKind OutputKind
        {
            get { return outputKind; }
        }

        /// <summary>
        /// The kind of output. See <see cref="NModcom.OutputKind"/>.
        /// </summary>
        public Type InnerType
        {
            get { return innerType; }
        }

    }


    /// <summary>
    /// This output holds itself the output IData object.
    /// </summary>
    public class SimpleOutput : Output
    {
        private IData data;

        public SimpleOutput(ISimObj owner, OutputKind outputKind, string name, string description, string units, IData data, Type innerType) :
            base(owner, outputKind, name, description, units, innerType)
        {
            this.data = data;
        }

        /// <summary>
        /// Reference to the data item. 
        /// </summary>
        public override IData Data
        {
            get { return data; }
        }
    }

    /// <summary>
    /// Description of an output with data storage in an IData-member of the class
    /// for which the output is defined.
    /// </summary>
    public class FieldOutput : Output
    {
        protected FieldInfo fieldInfo;

        public FieldOutput(ISimObj owner, OutputKind outputKind, string name, string description, string units, FieldInfo fieldInfo) :
            base(owner, outputKind, name, description, units, fieldInfo.FieldType)
        {
            this.fieldInfo = fieldInfo;
        }

        /// <summary>
        /// Reference to the data item. 
        /// </summary>
        public override IData Data
        {
            get
            {
                return (IData)fieldInfo.GetValue(Owner);
            }
        }
    }

    /// <summary>
    /// Wrapper for simulation object outputs
    /// </summary>
    public class OutputList : IOutputList, IEnumerable
    {
        ArrayList outputs;

        public OutputList(ArrayList outputs)
        {
            this.outputs = outputs;
        }

        protected int OutputIndex(string name)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Name == name)
                    return i;

            return -1;
        }

        public int Count
        {
            get { return outputs.Count; }
        }

        public IOutput this[int index]
        {
            get
            {
                if (index == -1)
                    return null;
                else
                    return (IOutput)outputs[index];
            }
        }

        public IOutput this[string name]
        {
            get { return this[OutputIndex(name)]; }
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return outputs.GetEnumerator();
        }

        #endregion
    }


    /// <summary>
    /// Wrapper for simulation object inputs
    /// </summary>
    public class InputList : IInputList, IEnumerable
    {
        ArrayList inputs;

        public InputList(ArrayList inputs)
        {
            this.inputs = inputs;
        }

        /// <summary>
        /// Searches for the input with the given name. If the input exists,
        /// its index is returned; if it does not exist, "-1" is returned.
        /// </summary>
        /// <param name="name">The name of the input to search for.</param>
        /// <returns>If the input exists, its index is returned; if it does not exist, "-1" is returned.</returns>
        protected int InputIndex(string name)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Name == name)
                    return i;

            return -1;
        }

        public int Count
        {
            get { return inputs.Count; }
        }

        /// <summary>
        /// Searches for the input with the given index. If the input does
        /// not exist, null is returned. 
        /// 
        /// This method does not throw an exception
        /// if there is no input with the given index. Instead it returns null
        /// which means that the input doesn't exist.
        /// </summary>
        /// <param name="index">Index of the input.</param>
        /// <returns>The input if "index" is within range; otherwise null is returned.</returns>
        public IInput this[int index]
        {
            get
            {
                if (index == -1)
                    return null;
                else
                    return ((IInput)inputs[index]);
            }
        }

        public IInput this[string name]
        {
            get { return this[InputIndex(name)]; }
        }


        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return inputs.GetEnumerator();
        }

        #endregion
    }

    #endregion

    public struct StateAndRateFields
    {
        public readonly FieldInfo State, Rate;

        public StateAndRateFields(FieldInfo state, FieldInfo rate)
        {
            this.State = state;
            this.Rate = rate;
        }
    }

    /// <summary>
    /// Base class for implementing simulation objects
    /// </summary>
    public abstract class SimObj : ISimObj
    {
        private string name;
        private ISimEnv simenv;
        private ArrayList inputDefinitions;
        private ArrayList outputDefinitions;
        private IInputList inputs;
        private IOutputList outputs;
        // stateInfo is used in Get/SetState() and in GetCount()
        protected List<StateAndRateFields> stateInfo;

        protected int cStateVar = -1;

        /// <summary>
        /// </summary>
        protected SimObj()
        {
            // default object name is the short name of the class
            this.name = this.GetType().Name;

            inputDefinitions = new ArrayList();
            inputs = new InputList(inputDefinitions);

            outputDefinitions = new ArrayList();
            outputs = new OutputList(outputDefinitions);

            CollectInputOutputFields();
        }

        /// <summary>
        /// A short, descriptive name of the instance.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The simulation environment in which the object is registered.
        /// Do not set this property, the simulation environment will set it when
        /// the object is registered.
        /// </summary>
        public ISimEnv SimEnv
        {
            get { return simenv; }
            set { simenv = value; }
        }

        /// <summary>
        /// A list of inputs.
        /// </summary>
        public IInputList Inputs
        {
            get { return inputs; }
        }

        /// <summary>
        /// A list of outputs.
        /// </summary>
        public IOutputList Outputs
        {
            get { return outputs; }
        }

        public virtual void StartRun()
        {
        }

        public virtual void EndRun()
        {
        }

        public virtual void HandleEvent(ISimEvent simEvent)
        {
        }

        public virtual void OutputFunction()
        {
        }

        /// <summary>
        /// Collect the fields with Input-, Output-, Param-, Signal-, or State-attributes
        /// </summary>
        protected virtual void CollectInputOutputFields()
        {
            Type type = GetType();
            stateInfo = new List<StateAndRateFields>();

            // examine each field
            foreach (FieldInfo f in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {

                // find all the attributes that are applied to our field
                object[] inputs = f.GetCustomAttributes(typeof(InputAttribute), true);
                object[] parms = f.GetCustomAttributes(typeof(ParamAttribute), true);
                object[] signals = f.GetCustomAttributes(typeof(SignalAttribute), true);
                object[] outputs = f.GetCustomAttributes(typeof(OutputAttribute), true);
                object[] states = f.GetCustomAttributes(typeof(StateAttribute), true);

                bool IsSimData =
                    (f.FieldType.FullName.Equals("NModcom.IData"))
                    || (f.FieldType.GetInterface("NModcom.IData") != null);

                // expect at most one attribute 
                if ((inputs.Length + outputs.Length + states.Length + parms.Length + signals.Length) > 1)
                    throw new Exception("Two or more attributes (Input, Output, Param, Signal, State) were applied to field " + f.Name + " in class " + this.GetType().FullName);

                // check that State is only declared in SimObj's that implement IOdeProvider
                if (states.Length > 0)
                    if (this.GetType().GetInterface("NModcom.IOdeProvider") == null)
                        throw new Exception("The [State] attribute is meaningful only in classes that implement NModcom.IOdeProvider");

                // if we found an Input attribute for our field, ..
                foreach (InputAttribute inp in inputs)
                {
                    if (f.FieldType == typeof(System.String) || f.FieldType == typeof(System.Double) || f.FieldType == typeof(System.Int32) || 
                                f.FieldType == typeof(System.Boolean) || f.FieldType == typeof(System.DateTime))
                    {
                        AddInput(new SimpleInput(this, InputKind.Signal, inp.Name, inp.Description, inp.Units, new MemberFieldSimData(this, f)));
                    }
                    else if (IsSimData)
                        AddInput(new FieldInput(this, InputKind.Signal, inp.Name, inp.Description, inp.Units, f, (IData)f.GetValue(this)));
                    else
                        throw new Exception("Attribute Input was applied to field \"" + inp.Name + "\" of type " + f.FieldType.FullName);
                }

                // if we found a Param attribute for our field, ..
                foreach (ParamAttribute inp in parms)
                {
                    if (f.FieldType == typeof(System.String) || f.FieldType == typeof(System.Double) || f.FieldType == typeof(System.Int32) ||
                                f.FieldType == typeof(System.Boolean) || f.FieldType == typeof(System.DateTime))
                    {
                        AddInput(new SimpleInput(this, InputKind.Signal, inp.Name, inp.Description, inp.Units, new MemberFieldSimData(this, f)));
                    }
                    else if (IsSimData)
                        AddInput(new FieldInput(this, InputKind.Param, inp.Name, inp.Description, inp.Units, f, (IData)f.GetValue(this)));
                    else
                        throw new Exception("Attribute Param was applied to field \"" + inp.Name + "\" of type " + f.FieldType.FullName);
                }

                // if we found a Signal attribute for our field, ..
                foreach (SignalAttribute inp in signals)
                {
                    if (f.FieldType == typeof(System.String) || f.FieldType == typeof(System.Double) || f.FieldType == typeof(System.Int32) ||
                                f.FieldType == typeof(System.Boolean) || f.FieldType == typeof(System.DateTime))
                    {
                        AddInput(new SimpleInput(this, InputKind.Signal, inp.Name, inp.Description, inp.Units, new MemberFieldSimData(this, f)));
                    }
                    else if (IsSimData)
                        AddInput(new FieldInput(this, InputKind.Signal, inp.Name, inp.Description, inp.Units, f, (IData)f.GetValue(this)));
                    else
                        throw new Exception("Attribute Signal was applied to to field \"" + inp.Name + "\" of type " + f.FieldType.FullName);
                }

                // if we found an Output attribute for our field, ..
                foreach (OutputAttribute outp in outputs)
                {
                    if (IsSimData)
                    {
                        AddOutput(new FieldOutput(this, OutputKind.Other, outp.Name, outp.Description, outp.Units, f));
                    }
                    else
                    {
                        var g = new MemberFieldSimData(this, f);
                        AddOutput(OutputKind.Other, outp.Name, outp.Description, outp.Units, g, f.FieldType);
                    }
                }

                // if we found a State attribute for our field, ..
                foreach (StateAttribute state in states)
                {
                    // find the corresponding rate-field
                    FieldInfo rateField = null;
                    MemberInfo[] members = GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    for (int i = 0; i < members.Length; i++)
                    {
                        object[] rates = members[i].GetCustomAttributes(typeof(RateAttribute), true);

                        if (rates.Length >= 2)
                            throw new Exception("Shouldn't have more than one [Rate] for a given field");

                        if (rates.Length == 1)
                        {
                            RateAttribute rate = (RateAttribute)rates[0];
                            if (rate.StateField.Equals(f.Name))
                                rateField = (FieldInfo)members[i];
                        }

                        if (rateField != null)
                            break;
                    }
                    if (rateField == null)
                        throw new Exception("Unable to find rate-field for state variable with name " + state.Name + " which is stored in field " + f.Name);

                    if (f.FieldType == typeof(System.Double))
                    {
                        // Get/SetState() must know about state fields
                        AddStateField(f, rateField);

                        // add output for the actual value of this state variable
                        NonDataFieldSimData g = new NonDataFieldSimData(this, f);
                        AddOutput(OutputKind.State, state.Name, state.Description, state.Units, g, f.FieldType);

                        // add input for initial value of this state variable
                        double x = (double)f.GetValue(this);
                        AddInput(new SimpleInput(this, InputKind.InitialState, state.Name, state.Description, state.Units, new ConstFloatSimData(x)));
                    }
                    else if (f.FieldType == typeof(double[]))
                    {
                        // Get/SetState() must know about state fields
                        AddStateField(f, rateField);

                        // add output for the actual value of this state variable
                        IData g = new FloatArrayOutput(this, f);
                        AddOutput(OutputKind.State, state.Name, state.Description, state.Units, g, f.FieldType);

                        // add input for initial value of this state variable
                        double[] x = (double[])f.GetValue(this);
                        //TODO
                        //AddInput(new DoubleFieldInput(this, InputKind.InitialState, state.Name, state.Description, state.Units, f, new ConstFloatSimData(x)));
                    }
                    else
                        throw new Exception("Sorry, fields for state variables must be either of type System.double or of type System.Double[]");
                }

            }

            FindOutputMethods();
        }

        private void AddStateField(FieldInfo f, FieldInfo rate)
        {
            stateInfo.Add(new StateAndRateFields(f, rate));
        }

        private void FindOutputMethods()
        {
            // examine each field
            foreach (MethodInfo m in this.GetType().GetMethods(
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.Instance
                ))
            {

                // find all the attributes that are applied to our field
                object[] outputs = m.GetCustomAttributes(typeof(OutputAttribute), true);

                // if we found an Output attribute for our field, ..
                foreach (OutputAttribute outp in outputs)
                {
                    if (m.ReturnType == typeof(double))
                    {
                        IData data = new MethodData(this, m);
                        AddOutput(new SimpleOutput(this, OutputKind.Other, outp.Name, outp.Description, outp.Units, data, new Double().GetType()));
                    }
                }
            }
        }

        #region Input Configuration

        protected IInput AddInput(IInput inputDef)
        {
            // check that we don't already have an input with this name
            foreach (IInput inp in Inputs)
                if (inp.Name.Equals(inputDef.Name))
                    throw new Exception("While attempting to add an input with name \""
                        + inp.Name + "\" to in instance of class \""
                        + this.GetType().FullName
                        + "\" another input with that name has already been defined."
                    );

            // ok, add the input
            inputDefinitions.Add(inputDef);
            return inputDef;
        }

        protected IInput AddInput(InputKind inputKind, string name, string description, string units, double value)
        {
            IInput inputDef = new SimpleInput(this, inputKind, name, description, units, null);
            inputDef.Data = new ConstFloatSimData(this, value);
            return AddInput(inputDef);
        }

        protected IInput AddInput(InputKind inputKind, string name, string description, string units, string value)
        {
            IInput inputDef = new SimpleInput(this, inputKind, name, description, units, null);
            inputDef.Data = new ConstStringSimData(this, value);
            return AddInput(inputDef);
        }

        #endregion

        #region Output Configuration

        protected IOutput AddOutput(IOutput outputDef)
        {
            // check that we don't already have an output with this name
            foreach (IOutput outp in Outputs)
                if (outp.Name.Equals(outputDef.Name))
                    throw new Exception("While attempting to add an output with name \""
                        + outp.Name + "\" to in instance of class \""
                        + this.GetType().FullName
                        + "\" another output with that name has already been defined."
                    );

            // ok, add the output
            outputDefinitions.Add(outputDef);
            return outputDef;
        }

        protected IOutput AddOutput(OutputKind outputKind, string name, string description, string units, IData data, Type innerType)
        {
            return AddOutput(new SimpleOutput(this, outputKind, name, description, units, data, innerType));
        }

        protected IOutput AddOutput(OutputKind outputKind, string name, string description, string units, double value)
        {
            return AddOutput(outputKind, name, description, units, new ConstFloatSimData(this, value), new Double().GetType());
        }

        protected IOutput AddFloatFuncOutput(string name, string description, string units, Func<double> valueFunc)
        {
            return AddOutput(OutputKind.Other, name, description, units, new FloatFuncSimData(valueFunc), typeof(double));
        }

        protected IOutput AddIntFuncOutput(string name, string description, string units, Func<int> valueFunc)
        {
            return AddOutput(OutputKind.Other, name, description, units, new IntFuncSimData(valueFunc), typeof(int));
        }

        protected IOutput AddStringFuncOutput(string name, string description, string units, Func<string> valueFunc)
        {
            return AddOutput(OutputKind.Other, name, description, units, new StringFuncSimData(valueFunc), typeof(string));
        }

        #endregion

        #region Event Helpers

        /// <summary>
        /// Registers a state event for this object.
        /// </summary>
        /// <param name="message">A message that will be put in the Message property of the 
        /// simulation event</param>
        /// <param name="stateChecker">A method of this object that will be called each time the simulation environment needs to check the state of the event</param>
        protected void RegisterEvent(int message, StateChecker stateChecker)
        {
            SimEnv.RegisterEvent(new DelegateStateEvent(this, this, (int)EventPriority.Update, message, stateChecker));
        }

        /// <summary>
        /// Register a time event at a specified time
        /// </summary>
        /// <param name="message">Message ID, will be put in the events Message property.</param>
        /// <param name="when">Event time</param>
        protected void RegisterEvent(int message, double when)
        {
            SimEnv.RegisterEvent(new TimeEvent(this, this, (int)EventPriority.Update, message, when));
        }

        /// <summary>
        /// Schedules a repeating time event.
        /// </summary>
        /// <param name="message">Will be put in the Message proeprty of the event.</param>
        /// <param name="when">First event time</param>
        /// <param name="interval">Interval, must be > 0</param>
        protected void RegisterEvent(int message, double when, double interval)
        {
            SimEnv.RegisterEvent(new RecurringTimeEvent(this, this, (int)EventPriority.Update, message, when, interval));
        }

        #endregion

    }
}
