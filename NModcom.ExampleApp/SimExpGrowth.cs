using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModcom.ExampleApp
{
    internal class SimExpGrowth
    {
        public static void RunSimulation()
        {
            Console.WriteLine("Simulation of exponential growth");

            ISimEnv simenv = new SimEnv()
            {
                StartTime = 0,
                StopTime = 5
            };

            ISimObj simobj = new ExpGrowth();
            simenv.Add(simobj);
            simobj.Inputs["rgr"].Data = new ConstFloatSimData(0.05);

            simenv.OutputEvent += Simenv_OutputEvent;
            simenv.Run();
        }

        private static void Simenv_OutputEvent(object sender, EventArgs e)
        {
            ISimEnv? simenv = sender as ISimEnv;
            Console.WriteLine($"{simenv?.CurrentTime},{simenv?[0].Outputs["S"].Data.AsFloat}");
        }
    }
}
