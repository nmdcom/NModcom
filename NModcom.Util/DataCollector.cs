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
using System.Collections;
using System.Collections.Generic;

namespace NModcom.Util
{
	/// <summary>
	/// At each timestep,
	/// each connected input's Data is cast to IData and stored in memory,
	/// along with the time at which the values were collected.
	/// </summary>
	public class DataCollector: UpdateableSimObj
	{
		private List<double> time;
        private ArrayList data;

		public DataCollector()
		{
		}
		
		public override void StartRun()
		{
			time = new List<double>();
			data = new ArrayList();
			for (int i = 0; i < Inputs.Count; i++)
				data.Add(new ArrayList());	
		}

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

        public override void HandleEvent(NModcom.ISimEvent simEvent)
		{
			// collect inputs
			time.Add(simEvent.SimEnv.CurrentTime);
			for (int i = 0; i < Inputs.Count; i++)
			{
				if ((Inputs[i] != null) && (Inputs[i].Data != null))
				{
                    double x;
                    try
                    {
                        x = Inputs[i].Data.AsFloat;
                    }
                    catch
                    {
                        /// not all IData implementations implement AsFloat, but we 
                        /// don't want that to cause a crash
                        x = -9999;
                    }
					(data[i] as ArrayList).Add(x);
				}
			}
		}

		public double Value(int t, int i)
		{
			return (double)(data[i] as ArrayList)[t];
		}

		public double Time(int t)
		{
			return (double)time[t];
		}

        public double[] TimePoints()
        {
            double[] x = new double[time.Count];
            time.CopyTo(x);
            return x;
        }

        public double[] TimeSeries(int i)
        {
            double[] x = new double[time.Count];
            for (int j = 0; j < time.Count; j++)
                x[j] = (double)((data[i] as ArrayList)[j]);
            return x;
        }

	}
}

