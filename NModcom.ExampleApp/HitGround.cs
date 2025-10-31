using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModcom.ExampleApp
{
    public class HitGround : StateEvent
    {
        public HitGround(ISimObj source, ISimObj target, int priority, int message) : base(source, target, priority, message)
        {
        }

        public override bool CheckState(double time)
        {
            // Return "true" when the ball has hit the ground
            // This is the case when the height of the ball is less than delta
            // and when speed is negative (to prevent the event from firing when the ball right after a bounce,
            // when the ball is still close to the ground but moving up)
            const double delta = 0.05;
            return (Target.Outputs["height"].Data.AsFloat < delta) 
                && (Target.Outputs["velocity"].Data.AsFloat < 0.0);
        }
    }
}
