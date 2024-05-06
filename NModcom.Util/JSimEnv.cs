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

namespace NModcom.Util
{
    public class JSimEnv
    {
        public double StartTime { get; set; }

        public double StopTime { get; set; }

        public string Name { get; set; }

        public JIntegrator Integrator { get; set; }

        public List<JSimObj> SimObj { get; set; }

        public List<JSimObjLink> SimObjLink { get; set; }

        public JSimObjOutput SimObjOutput { get; set; }

        public JSimEnv() { }

        public JSimEnv(ISimEnv simenv)
        {
            // simenv properties
            StartTime = simenv.StartTime;
            StopTime = simenv.StopTime;
            Name = simenv.Name;
            // TODO: add info about the integrator

            // list of simobjs
            SimObj = new List<JSimObj>();
            for (int i = 0; i < simenv.Count; i++)
                SimObj.Add(new JSimObj(simenv[i]));

            // list of links between simobjs
            SimObjLink = new List<JSimObjLink>();
            for (int t = 0; t < simenv.Count; t++)
            {
                ISimObj objDest = simenv[t];
                for (int i = 0; i < objDest.Inputs.Count; i++)
                {
                    // If this input is the output of another SimObj, then we add a SimObjLink
                    IInput inp = objDest.Inputs[i];
                    bool v = inp.Data != null
                        && inp.Data.Owner != null
                        && inp.Data.Owner != objDest; // really should check if owner is in simenv
                    if (v)
                    // this input is linked to the output from another ISimObj in the simulation, therefore write info about this link
                    {
                        ISimObj objFrom = inp.Data.Owner;
                        int oo = -1;
                        for (int o = 0; o < objFrom.Outputs.Count; o++)
                        {
                            if (objFrom.Outputs[o].Data == inp.Data)
                            {
                                oo = o;
                                break;
                            }
                        }
                        if (oo >= 0)
                        {
                            // add link
                            Console.WriteLine("found link [{0}].[{1}] --> [{2}].[{3}]"
                                , objFrom.Name
                                , objFrom.Outputs[oo].Name
                                , objDest.Name
                                , inp.Name
                                );
                            SimObjLink.Add(new JSimObjLink(objFrom.Name, objFrom.Outputs[oo].Name, objDest.Name, inp.Name));
                        }
                    }
                    else
                    // .. this input is NOT the output of another SimObj, therefore we write info about the input
                    {
                        // add input
                        SimObj[t].Inputs.Add(new JInput(objDest.Inputs[i], objDest));
                    }
                }
            }
        }
    }
}
