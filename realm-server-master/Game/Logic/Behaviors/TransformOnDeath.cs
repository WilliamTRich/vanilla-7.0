using RotMG.Common;
using RotMG.Game.Worlds;
using RotMG.Utils;
using SimpleLog;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class TransformOnDeath : Behavior
    {
        public readonly ushort Target;
        public readonly int Min;
        public readonly int Max;
        public readonly float Probability;
        
        public TransformOnDeath(string target, int min = 1, int max = 1, float probability = 1)
        {
            Target = GetObjectType(target);
            Min = min;
            Max = max;
            Probability = probability;
        }
        public override void Death(Entity host)
        {
            if (!MathUtils.Chance(Probability))
                return;

            var count = MathUtils.NextInt(Min, Max + 1);
            for (var i = 0; i < count; i++)
            {
                var entity = Entity.Resolve(Target);
                host.Parent.AddEntity(entity, host.Position);

                if (entity.Desc.DisplayId == "Ocean Vent")//add vent to list of vents
                {
#if DEBUG
                    SLog.Debug( "Trying to add vent");
#endif
                    ((OceanTrench)host.Parent).addVentToWorld(entity);
                }

            }
        }
    }
}