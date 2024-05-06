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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NModcom.Util
{
    public enum OutputWriterEventKind
    {
        Output,
        AfterTime
    }

    public class CSVOutputWriter
    {
        private OutputWriterEventKind _eventKind;
        private string[] _ignoreOutputs;
        private string _separator;
        private StreamWriter outputWriter;
        public bool writeheader;

        List<ISimEnv> list;

        public bool UseDates = false;

        public CSVOutputWriter(string filepath, bool append = false, OutputWriterEventKind eventKind = OutputWriterEventKind.Output, string[] ignoreOutputs = null, string separator = ",")
        {
            _eventKind = eventKind;
            _ignoreOutputs = ignoreOutputs ?? new string[0];
            _separator = separator;
            writeheader = !(append && File.Exists(filepath));

            // open output file
            outputWriter = new StreamWriter(filepath, append);

            list = new List<ISimEnv>();
        }

        public void AddSimEnv(ISimEnv simenv)
        {
            if (list.Count == 0 && writeheader)
            {
                // write header
                outputWriter.Write($"sim{_separator}date");
                for (int c = 0; c < simenv.Count; c++)
                {
                    var simenvName = simenv[c].Name;

                    for (int o = 0; o < simenv[c].Outputs.Count; o++)
                    {
                        var outputName = simenv[c].Outputs[o].Name;

                        if (!_ignoreOutputs.Any(i => i == outputName))
                        {
                            IData data = simenv[c].Outputs[o].Data;
                            if (data is IFloatArray)
                            {
                                IFloatArray floatArray = data as IFloatArray;
                                double[] d = floatArray.AsFloatArray;
                                for (int i = 0; i < d.Length; i++)
                                    outputWriter.Write($"{_separator}{simenvName}_{outputName}_{i}");
                            }
                            else if (data is IArray)
                            {
                                IArray arr = (data as IArray);
                                for (int j = 0; j < arr.GetLength(); j++)
                                    outputWriter.Write($"{_separator}{simenvName}_{outputName}[{j}]");
                            }
                            else
                                outputWriter.Write($"{_separator}{simenvName}_{outputName}");
                        }
                    }
                }
                outputWriter.WriteLine();
            }

            //TODO:
            //ISimEnv found = list.Find(simenv);
            //if (found == null)
            //{
            // hook up to the output event
            list.Add(simenv);
            AddEventHandler(simenv);
            //}
        }

        private void AddEventHandler(ISimEnv simenv)
        {
            if (_eventKind == OutputWriterEventKind.AfterTime)
                simenv.AfterTimeEventEvent += Simenv_OutputEvent;
            else
                simenv.OutputEvent += Simenv_OutputEvent;
        }

        public void Remove(ISimEnv simenv)
        {
            list.Remove(simenv);
            RemoveEventHandler(simenv);
        }

        private void RemoveEventHandler(ISimEnv simenv)
        {
            if (_eventKind == OutputWriterEventKind.AfterTime)
                simenv.AfterTimeEventEvent -= Simenv_OutputEvent;
            else
                simenv.OutputEvent -= Simenv_OutputEvent;
        }

        public void RemoveAll()
        {
            while (list.Count > 0)
                Remove(list[0]);
        }

        public void Close()
        {
            RemoveAll();
            outputWriter.Close();
            outputWriter = null;
        }

        private void Simenv_OutputEvent(object sender, EventArgs e)
        {
            ISimEnv simenv = sender as ISimEnv;
            string timestr;

            if (UseDates)
            {
                DateTime simTime = CalendarTime.ToDateTime(simenv.CurrentTime);
                timestr = simTime.ToString("yyyy-MM-dd");
            }
            else
                timestr = simenv.CurrentTime.ToString();

            outputWriter.Write($"{simenv.Name}{_separator}{timestr}");
            for (int c = 0; c < simenv.Count; c++)
                for (int o = 0; o < simenv[c].Outputs.Count; o++)
                {
                    if (!_ignoreOutputs.Any(i => i == simenv[c].Outputs[o].Name))
                    {
                        IData data = simenv[c].Outputs[o].Data;
                        if (data is IFloatArray)
                        {
                            IFloatArray floatArray = data as IFloatArray;
                            double[] d = floatArray.AsFloatArray;
                            for (int i = 0; i < d.Length; i++)
                                outputWriter.Write($"{_separator}{d[i]}");
                        }
                        else if (data is IArray)
                        {
                            IArray arr = (data as IArray);
                            for (int j = 0; j < arr.GetLength(); j++)
                                outputWriter.Write($"{_separator}{arr.GetElement(j)}");
                        }
                        else
                            outputWriter.Write($"{_separator}{simenv[c].Outputs[o].Data.AsString}");
                    }
                }
            outputWriter.WriteLine();
        }
    }
}
