using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModcom.ExampleApp
{
    internal class SimEnvOnly
    {
        public static void RunSimulation()
        {
            Console.WriteLine("Simulation with SimEnv");

            ISimEnv simenv = new SimEnv()
            {
                StartTime = 0,
                StopTime = 5
            };
            simenv.OutputEvent += Simenv_OutputEvent;
            simenv.Run();
        }

        public static void RunSimulationCalendar()
        {
            Console.WriteLine("Simulation with SimEnv and calendar dates");

            ISimEnv simenv = new SimEnv()
            {
                StartTime = CalendarTime.ToDouble(new DateTime(2024, 3, 11)),
                StopTime = CalendarTime.ToDouble(new DateTime(2024, 3, 25))
            };
            simenv.OutputEvent += Simenv_OutputEventCalender;
            simenv.Run();
        }
        private static void Simenv_OutputEvent(object sender, EventArgs e)
        {
            Console.WriteLine((sender as ISimEnv).CurrentTime);
        }
        private static void Simenv_OutputEventCalender(object sender, EventArgs e)
        {
            DateTime dt = CalendarTime.ToDateTime((sender as ISimEnv).CurrentTime);
            Console.WriteLine(dt.ToString("yyyy-MM-dd"));
        }
    }
}
