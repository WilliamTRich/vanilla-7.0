using System.Linq;
using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class Order : Behavior
    {
        public readonly float Radius;
        public readonly ushort Children;
        public readonly string TargetStateName;
        public readonly int CoolDown;
        public readonly int CoolDownOffset;
        public State TargetState;

        public Order(float range, string children, string targetState, int coolDown = 1000, int coolDownOffset = 0)
        {
            Radius = range;
            Children = GetObjectType(children);
            TargetStateName = targetState.ToLower();
            CoolDown = coolDown;
            CoolDownOffset = coolDownOffset;
        }
        public override void Enter(Entity host)
        {
            host.StateCooldown.Add(Id, CoolDownOffset);

            TargetState = Manager.Behaviors.Models[Children].States.Values
                .Select(i => FindState(i, TargetStateName))
                .FirstOrDefault(s => s != null);
        }

        public override void Exit(Entity host)
        {
            host.StateCooldown.Remove(Id);
        }

        private static State FindState(State state, string name)
        {
            if (state.StringId == name)
                return state;
            
            return state.States.Values
                .Select(i => FindState(i, name))
                .FirstOrDefault(s => s != null);
        }

        public override bool Tick(Entity host)
        {
            host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
            if (host.StateCooldown[Id] <= 0)
            {
                foreach (var entity in host.GetNearestEntitiesByType(Radius, Children))
                        entity.OrderState(TargetState.Id);

                host.StateCooldown[Id] = CoolDown.NextCooldown(0);
            }

                //if (entity.CurrentStates.Contains(TargetState))
            
            return true;
        }
    }
}