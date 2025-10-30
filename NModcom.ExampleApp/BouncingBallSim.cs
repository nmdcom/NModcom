using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModcom.ExampleApp
{
    public class BouncingBallSim
    {

        static StreamWriter? sw;

        public static void RunSimulation()
        {
            Console.WriteLine("Simulation of bouncing ball");

            ISimEnv simenv = new SimEnv()
            {
                StartTime = 0,
                StopTime = 15
            };
            simenv.Integrator.IntegrationTimeStep = 0.01;
            simenv.OutputEvent += Simenv_OutputEvent;

            ISimObj ball = new BouncingBall();
            simenv.Add(ball);

            sw = new StreamWriter("bouncingball.csv"); 
            sw.WriteLine("time,velocity,height");

            simenv.Run();

            sw.Close();
            Console.WriteLine("Simulation of bouncing ball finished");

        }

        private static void Simenv_OutputEvent(object sender, EventArgs e)
        {
            ISimEnv? simenv = sender as ISimEnv;
            sw?.WriteLine(
                $" {simenv?.CurrentTime:F3}" +
                $",{simenv?[0].Outputs["velocity"].Data.AsFloat:F3}" +
                $",{simenv?[0].Outputs["height"].Data.AsFloat:F3}"
            );
        }
    }
}
