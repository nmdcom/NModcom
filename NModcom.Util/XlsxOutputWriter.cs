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
using ClosedXML.Excel;

namespace NModcom.Util
{
    public class XlsxOutputWriter
    {
        private OutputWriterEventKind _eventKind;
        private FileInfo _fileInfo;
        private string[] _ignoreOutputs;
        private int _row;
        private XLWorkbook _workbook;
        private IXLWorksheet _worksheet;
        private bool writeheader;

        List<ISimEnv> list;

        public bool UseDates = false;

        public XlsxOutputWriter(string filepath, OutputWriterEventKind eventKind = OutputWriterEventKind.Output, string[] ignoreOutputs = null)
        {
            _eventKind = eventKind;
            _ignoreOutputs = ignoreOutputs ?? new string[0];
            writeheader = true;

            // open output file
            _fileInfo = new FileInfo(filepath);
            _workbook = new XLWorkbook();
            _worksheet = _workbook.AddWorksheet(_fileInfo.Name);
            _row = 1;

            list = new List<ISimEnv>();
        }

        public void AddSimEnv(ISimEnv simenv)
        {
            if (list.Count == 0 && writeheader)
            {
                var col = 0;

                // write header
                _worksheet.Cell(_row, ++col).SetValue("sim");
                _worksheet.Cell(_row, ++col).SetValue("time");
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
                                    _worksheet.Cell(_row, ++col).SetValue($"{simenv[c].Name}_{simenv[c].Outputs[o].Name}_{i}");
                            }
                            else if (data is IArray)
                            {
                                IArray arr = (data as IArray);
                                for (int j = 0; j < arr.GetLength(); j++)
                                    _worksheet.Cell(_row, ++col).SetValue($"{simenv[c].Name}_{simenv[c].Outputs[o].Name}[{j}]");
                            }
                            else
                                _worksheet.Cell(_row, ++col).SetValue($"{simenv[c].Name}_{simenv[c].Outputs[o].Name}");
                        }
                    }

                var rng = _worksheet.Range(_row, 1, _row, col);
                rng.Style.Alignment.WrapText = true;
                rng.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                rng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                rng.Style.Fill.PatternType = XLFillPatternValues.Solid;
                rng.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;

                _worksheet.SheetView.Freeze(_row, 0);

                _row++;
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
            AdjustContent();

            _workbook.SaveAs(_fileInfo.FullName);
            _workbook.Dispose();
            _workbook = default(XLWorkbook);

            RemoveAll();
        }

        private void AdjustColumn(int col, Type dataType)
        {
            if (dataType == typeof(DateTime))
            {
                var rng = _worksheet.Range(2, col, _row, col);
                rng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                rng.Style.DateFormat.Format = "dd-mm-yyyy hh:mm:ss";
            }
            else if (dataType == typeof(int) || dataType == typeof(float) || dataType == typeof(double))
            {
                var rng = _worksheet.Range(2, col, _row, col);
                rng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            }
        }

        private void AdjustContent()
        {
            _worksheet.Columns().AdjustToContents(1, Math.Max(_row, 250), 5.0, double.MaxValue);

            var col = 0;

            foreach (var simenv in list)
            {
                AdjustColumn(++col, typeof(string));
                AdjustColumn(++col, typeof(int));

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
                                    AdjustColumn(++col, typeof(double));
                            }
                            else if (data is IArray)
                            {
                                IArray arr = (data as IArray);
                                for (int j = 0; j < arr.GetLength(); j++)
                                    AdjustColumn(++col, typeof(string));
                            }
                            else
                            {
                                AdjustColumn(++col, (simenv[c].Outputs[o] as Output).InnerType);
                            }
                        }
                    }
            }
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

            var col = 0;
            _worksheet.Cell(_row, ++col).SetValue(simenv.Name);
            _worksheet.Cell(_row, ++col).SetValue(timestr);

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
                                _worksheet.Cell(_row, ++col).SetValue(d[i]);
                        }
                        else if (data is IArray)
                        {
                            IArray arr = (data as IArray);
                            for (int j = 0; j < arr.GetLength(); j++)
                                _worksheet.Cell(_row, ++col).SetValue(arr.GetElement(j).ToString());
                        }
                        else
                            _worksheet.Cell(_row, ++col).SetValue(simenv[c].Outputs[o].Data.AsString);
                    }
                }

            _row++;
        }
    }
}
