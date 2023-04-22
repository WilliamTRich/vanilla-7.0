using RotMG.Common;
using RotMG.Game.Worlds;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class DavyJonesDoorOpen : Behavior
    {
        private readonly int DoorId;
        public DavyJonesDoorOpen(int doorId)
        {
            DoorId = doorId;
        }

        public override void Enter(Entity host)
        {
            if(host.Parent is DavyJonesLocker locker)
            {
                locker.OpenDoors(DoorId);
            }

            base.Enter(host);
        }
    }
}