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
using System.Text;
using System.IO;
using NModcom;

namespace NModcom.Util
{
    /// <summary>
    /// ForcingData reads a comma-separated file with time-series data and makes 
    /// these available through its outputs. 
    /// 
    /// The input file should have a header line with contains the names of the columns.
    /// The first column should be called "date". There should be no quotes surrounding
    /// the column names. Items should be separated by a comma.
    /// 
    /// All other lines of the file should contain data values separated by commas.
    /// The first column should contain a date in yyyy-mm-dd format. There should be
    /// no quotes surrounding the values.
    /// 
    /// Lines should be strictly ordered by date, but gaps may exist.
    /// 
    /// The class will make available those values that correspond to the last date 
    /// that is equal to or less than the current simulation time. If simulation time
    /// is earlier than the earliest recorded date, then the earliest recorded values 
    /// will be made available. Similarly, if the simulation time is later than the
    /// latest recorded date, then the latest recorded values will be made available.
    /// Thus, this class will always make available some values.
    /// 
    /// Example usage:
    /// <code>
    /// ISimEnv simenv = new SimEnv();
    /// simenv.Time.StartDate = new DateTime(2007, 4, 12);
    /// simenv.Time.StopDate = new DateTime(2007, 7, 1);
    /// 
    /// ISimObj forcingData = new ForcingData("weather.csv");
    /// simenv.Add(forcingData);
    /// 
    /// ISimObj simobj = new SomeOtherSimObj();
    /// simenv.Add(simobj);
    /// 
    /// simobj.Inputs["rain"].Data = forcingData.Outputs["rain"].Data;
    /// 
    /// simenv.Run();
    /// </code>
    /// </summary>
    public class ForcingData: UpdateableSimObj  
    {
        string[] columns;
        List<Day> days;
        List<IOutput> outputs;

        public ForcingData(string filename)
        {
            StreamReader sr = new StreamReader(filename);

            string line = sr.ReadLine();
            columns = line.Split(new Char[] { ',' });
            if (!columns[0].ToLower().Equals("date"))
                throw new Exception("first column should be named date in file " + filename);

            outputs = new List<IOutput>();
            for (int i = 1; i < columns.Length; i++)
            {
                IOutput outp = AddOutput(OutputKind.Other, columns[i], "", "", 0);
                outputs.Add(outp);
            }

            days = new List<Day>();

            while ((line = sr.ReadLine()) != null)
            {
                Day d = new Day(columns.Length, line);
                if ((days.Count >= 1) && (d.time <= days[days.Count - 1].time))
                    throw new Exception("out-of-order date found in file " + filename);
                days.Add(d);
            }
        }

        public override void HandleEvent(ISimEvent simEvent)
        {
            int d;
            double ct = SimEnv.CurrentTime;

            if (ct <= days[0].time)
                d = 0;
            else if (ct >= days[days.Count - 1].time)
                d = days.Count - 1;
            else
            {
                d = 0;
                while (days[d].time < ct)
                    d++;
            }
            for (int i = 0; i < columns.Length-1; i++)
                outputs[i].Data.AsFloat = days[d].data[i];
        }
    }

    class Day
    {
        public double time;
        public double[] data;

        public Day(int ncols, string line)
        {
            string[] tokens = line.Split(new Char[] { ',' });
            if (tokens.Length != ncols)
                throw new Exception("incorrect number of columns found in datafile");

            DateTime dt = DateTime.Parse(tokens[0]);
            time = (double)dt.Ticks / TICKSPERDAY;

            data = new double[ncols - 1];
            for (int i = 1; i < tokens.Length; i++)
                data[i - 1] = Convert.ToDouble(tokens[i]);
        }

        public const long TICKSPERDAY = 864000000000;

    }
}
