namespace RotMG.Game.Logic.Behaviors
{
    public sealed class Transform : Behavior
    {
        public readonly ushort Target;

        public Transform(string target)
        {
            Target = GetObjectType(target);
        }

        public override bool Tick(Entity host)
        {
            var entity = Entity.Resolve(Target);

            host.Parent.AddEntity(entity, host.Position);
            host.Parent.RemoveEntity(host);
            return false;
        }
    }
}