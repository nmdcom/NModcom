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
            return (Target.Outputs["height"].Data.AsFloat < 0.05) 
                && (Target.Outputs["velocity"].Data.AsFloat < 0.0);
        }
    }
}
