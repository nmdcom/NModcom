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

        public static void RunSimulation(string str_integrator = "Euler")
        {
            Console.WriteLine("Simulation of bouncing ball");
            string fn;
            Integrator integrator;
            if (str_integrator.Equals("RK"))
            {
                integrator = new RKCKIntegrator();
                fn = "bouncingball_rk.csv";
            }
            else
            {
                integrator = new EulerIntegrator();
                integrator.IntegrationTimeStep = 0.01;
                fn = "bouncingball_euler.csv";
            }

            ISimEnv simenv = new SimEnv()
            {
                StartTime = 0,
                StopTime = 15,
                Integrator = integrator
            };

            simenv.OutputEvent += Simenv_OutputEvent;

            ISimObj ball = new BouncingBall();
            simenv.Add(ball);

            sw = new StreamWriter(fn);
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
