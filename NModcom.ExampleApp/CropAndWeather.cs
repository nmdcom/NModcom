using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModcom.ExampleApp
{
    internal class CropAndWeather
    {
        public static void RunSimulation()
        {
            ISimEnv simenv = new SimEnv
            {
                StartTime = 0,
                StopTime = 4
            };

            ISimObj w = new Weather();
            simenv.Add(w);

            ISimObj crop = new Crop();
            simenv.Add(crop);

            crop.Inputs["I"].Data = w.Outputs["I"].Data;
            crop.Inputs["k"].Data = new ConstFloatSimData(0.7);
            crop.Inputs["e"].Data = new ConstFloatSimData(3.0);
            crop.Inputs["frleaves"].Data = new ConstFloatSimData(0.5);
            crop.Inputs["SLA"].Data = new ConstFloatSimData(0.002);

            simenv.OutputEvent += Simenv_OutputEvent; 

            simenv.Run();

            Console.WriteLine();

        }

        private static void Simenv_OutputEvent(object sender, EventArgs e)
        {
            ISimEnv simenv = sender as ISimEnv;
            Console.WriteLine("{0}\t{1}\t{2}\t{3}"
                , simenv.CurrentTime
                , simenv[0].Outputs[0].Data.AsFloat
                , simenv[1].Outputs[0].Data.AsFloat
                , simenv[1].Outputs[1].Data.AsFloat
                );
        }
    }
}
