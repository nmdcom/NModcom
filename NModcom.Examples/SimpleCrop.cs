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
using NModcom.Framework;

namespace NModcom.Examples
{
    /// <summary>
    /// TSimpleCrop is a very simple crop growth model, based on the crop growth model
    /// described by Campbell and Stockle (1993) but much simplified for demonstration
    /// purposes.
    /// 
    /// Reference: Campbell, G.S., and C.O. Stockle, 1993. Prediction and simulation of water use in agricultural systems. Proc. 1st International Crop Science Congress, pp67-73.
    /// </summary>
    public class SimpleCrop : OdeSimObj, IAssimilable
    {
        [Input("Solar radiation")]
        IData solar = new ConstFloatSimData(20); // Solar radiation, MJ/m2/d

        [Input("Average temperature")]
        IData tave = new ConstFloatSimData(15); // Average temperature, C

        [Input("Actual transpiration rate of the crop")]
        IData at = new ConstFloatSimData(3); // Actual transpiration rate of the crop, mm/d

        [Input("Vapour pressure deficit")]
        IData vpd = new ConstFloatSimData(0.8); // Vapour pressure deficit, Pa

        [State("tdm")]
        double tdm;
        
        [Rate("tdm")]
        double tdm_rate;

        [State("gdd")]
        double gdd;

        [Rate("gdd")]
        double gdd_rate;

        [Output("LAI")]
        double lai;

        [Output("Rooting depth")]
        double rd;

        [Output("Fractional interception of light")]
        double fi;

        double maxlai;

        // params
        [Input("eff")]
        IData eff = new ConstFloatSimData(0.0015);

        [Input("k")]
        IData k = new ConstFloatSimData(0.6);          // dimensionless

        [Input("dwr")]
        IData dwr = new ConstFloatSimData(5.5);

        const double gdd_fl = 1200;    // Cd
        const double gdd_mat = 1800;   // Cd
        const double tbase = 0;        // C
        const double fl1 = 10;
        const double fl2 = 4;
        const double rdmax = 1.5;     // m

        public override void StartRun()
        {
            tdm = 0.008;
            gdd = 0;
            maxlai = 0;
        }

        public override void GetRates()
        {
            rd = 0.1 + (rdmax - 0.1) * Math.Min(1, gdd / gdd_fl);
            lai = fl1 * tdm / (1 + fl2 * tdm);
            if (gdd > gdd_fl)
            {
                if (maxlai <= 0)
                    maxlai = lai;
                else
                    lai = Math.Max(0, maxlai * (gdd_mat - gdd) / (gdd_mat - gdd_fl));
            }
            fi = 1 - Math.Exp(-k.AsFloat * lai);
            double dmis = eff.AsFloat * fi * solar.AsFloat;
            double dmiw = at.AsFloat * dwr.AsFloat / vpd.AsFloat;

            //Rates
            tdm_rate = Math.Min(dmis, dmiw); // rate of tdm
            gdd_rate = Math.Max(0, tave.AsFloat - tbase); // rate of gdd
        }

        public void SetStateVariable(string varname, double val)
        {
            if (varname.Equals("tdm", StringComparison.CurrentCultureIgnoreCase))
            {
                tdm = val;
            }
            else
                throw new Exception(";djifg-89pok");
        }

        public double GetStateVariable(string varname)
        {
            if (varname.Equals("tdm", StringComparison.CurrentCultureIgnoreCase))
            {
                return tdm;
            }
            else
                throw new Exception(";djifg-89pok");
        }
    }

}
