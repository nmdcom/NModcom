using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModcom.ExampleApp
{
    internal class SimEnvOnlyCalendarTime
    {
        public static void RunSimulation()
        {
            Console.WriteLine("Simulation with SimEnv and calendar dates");

            ISimEnv simenv = new SimEnv()
            {
                StartTime = CalendarTime.ToDouble(new DateTime(2024, 3, 11)),
                StopTime = CalendarTime.ToDouble(new DateTime(2024, 3, 25))
            };
            simenv.OutputEvent += Simenv_OutputEvent;
            simenv.Run();
        }
        private static void Simenv_OutputEvent(object sender, EventArgs e)
        {
            DateTime dt = CalendarTime.ToDateTime((sender as ISimEnv).CurrentTime);
            Console.WriteLine(dt.ToString("yyyy-MM-dd"));
        }
    }
}
