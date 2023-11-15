using System.Linq;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class OrderOnce : Behavior
    {
        public readonly float Radius;
        public readonly ushort Children;
        public readonly string TargetStateName;
        public State TargetState;

        public OrderOnce(float range, string children, string targetState)
        {
            Radius = range;
            Children = GetObjectType(children);
            TargetStateName = targetState.ToLower();
        }
        public override void Enter(Entity host)
        {
            TargetState = Manager.Behaviors.Models[Children].States.Values
                .Select(i => FindState(i, TargetStateName))
                .FirstOrDefault(s => s != null);
            
            foreach (var entity in host.GetNearestEntitiesByType(Radius, Children))
                entity.OrderState(TargetState.Id);
        }

        private static State FindState(State state, string name)
        {
            if (state.StringId == name)
                return state;
            
            return state.States.Values
                .Select(i => FindState(i, name))
                .FirstOrDefault(s => s != null);
        }
    }
}