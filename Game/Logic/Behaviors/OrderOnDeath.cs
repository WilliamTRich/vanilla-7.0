using System.Linq;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class OrderOnDeath : Behavior
    {
        public readonly float Radius;
        public readonly ushort Children;
        public readonly string TargetStateName;
        public readonly float Probability;
        public State TargetState;

        public OrderOnDeath(float range, string children, string targetState, float probability = 1)
        {
            Radius = range;
            Children = GetObjectType(children);
            TargetStateName = targetState.ToLower();
            Probability = probability;
        }
        public override void Enter(Entity host)
        {
            TargetState = Manager.Behaviors.Models[Children].States.Values
                .Select(i => FindState(i, TargetStateName))
                .FirstOrDefault(s => s != null);
        }

        private static State FindState(State state, string name)
        {
            if (state.StringId == name)
                return state;
            
            return state.States.Values
                .Select(i => FindState(i, name))
                .FirstOrDefault(s => s != null);
        }

        public override void Death(Entity host)
        {
            if (!MathUtils.Chance(Probability))
                return;
            
            foreach (var entity in host.GetNearestEntitiesByType(Radius, Children))
                entity.OrderState(TargetState.Id);
        }
    }
}