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
using NModcom;
//using NModcom.Kernel;
using NModcom.Framework;
using System.IO;
using System.Collections;


namespace NModcom.Util 
{
	/// <summary>
	/// OutputWriter writes the values of its IData inputs to a comma-separated file. 
	/// At each timestep,
	/// each connected input's Data is written to file using the ToString() method,
	/// along with the time at which the values were collected. If the Data represents
	/// an array of values (for example, soil water content in layers), then a separate 
	/// output column is written for each element of the array.
	/// The name of the 
	/// outputfile can be set through the input with name "filename".
	/// </summary>
	//TODO: make separate output for each element of an array
	public class OutputWriter: UpdateableSimObj
	{
		private StreamWriter sw;
		
		public OutputWriter() //this is the constructor
		{
			AddInput(InputKind.Param, "filename", "name of output file", "", "myoutputfile.txt");
		}

		/// <summary>
		/// Creates a new input for the outputwriter and connects it to the given instance of IData.
		/// </summary>
		/// <param name="columnName">Name of the new input (will be used to name the corresponding column in the output file).</param>
		/// <param name="data">The IData that will be connected to the new input.</param>
		/// <example>
		/// The following example shows how to use OutputWriter. NModcom.Examples.Prey is a
		/// simple population dynamics model that is distributed with NModcom.
		/// <code>
		/// using NModcom;
		/// 
		/// ISimObj prey = new NModcom.Examples.Prey();
		/// prey.Name = "Rabbits";
		/// 
		/// NModcom.Util.OutputWriter ow = new NModcom.Util.OutputWriter();
		/// ow.Inputs["filename"].Data.AsString = "myoutfile.csv";
		/// ow.AddInput("Rabbit population density", prey.Inputs["density"].Data);
		/// </code>
		/// </example>
		public void AddInput(string columnName, IData data)
		{
			IInput inputDef = new SimpleInput(this, InputKind.Signal, columnName, "", "", data);
			AddInput(inputDef);
		}

        /// <summary>
        /// Adds all outputs of all simobjs registered in the same ISimEnv as this object.
        /// </summary>
        public void AddAllInputs()
        {
            // check that we are registered with an ISimEnv
            if (SimEnv == null)
                throw new Exception("This method can only be called when the object has been registered with a simulation environment.");

            // add all outputs
            foreach (ISimObj simobj in SimEnv)
            {
                foreach (IOutput outp in simobj.Outputs)
                {
                    AddInput(simobj.Name + "." + outp.Name, outp.Data);
                }
            }

        }

		/// <summary>
		/// Opens a file for writing and writes a header line with column names into this file.
		/// </summary>
		public override void StartRun()
		{
			// open file
			string fileName = Inputs["filename"].Data.AsString;  
			sw = new StreamWriter(fileName);

			// write header
			sw.Write("time");
			sw.Write(",date");
			for( int i = 1; i < Inputs.Count; i++ )
			{
				IData data = Inputs[i].Data;
				// the IArray interface is the signal for writing more than one output column
				if (data is IArray)
				{
					IArray fa = (data as IArray);
					for (int j = 0; j < fa.GetLength(); j++)
						sw.Write("," + Inputs[i].Name + "[" + j + "]");
				}
				else
				{
					sw.Write("," + Inputs[i].Name);
				}
			}
			sw.WriteLine();

			// using flush-after-every-line could slow things down, 
			// so use flush-after-every-line only if you are debugging 
			// and you don't want to lose output in case of a crash
			//sw.Flush();
		}

		/// <summary>
		/// Closes the output file.
		/// </summary>
		public override void EndRun()
		{
			sw.Close();
		}

		/// <summary>
		/// Write a line to the output file that contains current values of all inputs.
		/// </summary>
		/// <param name="simEvent"></param>
		public override void HandleEvent(NModcom.ISimEvent simEvent)
		{
			// write all outputs, on one line
			sw.Write(simEvent.SimEnv.CurrentTime);
			sw.Write(",\"" + simEvent.SimEnv.Time.CurrentDate.ToShortDateString() + "\"");
			for( int i = 1; i < Inputs.Count; i++ )
			{
				IInput inp = Inputs[i];

				// catch likely errors
				if (inp == null) 
					 throw new Exception("Null input found in " + this.GetType().FullName);
				IData data = inp.Data;
				if (data == null) 
					 throw new Exception("Null data found in " + this.GetType().FullName + ": column name=" + inp.Name);

				// the IArray interface is the signal for writing more than one output column
				if (data is IArray)
				{
					IArray fa = (data as IArray);
					for (int j = 0; j < fa.GetLength(); j++)
						sw.Write("," + fa.GetElement(j).ToString());
				}
				else
					sw.Write("," + data.AsString);
			}
			sw.WriteLine();
		}

	}
	
}
