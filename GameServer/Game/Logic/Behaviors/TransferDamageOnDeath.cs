using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class TransferDamageOnDeath : Behavior
{
    public readonly ushort Target;
    public readonly float Radius;
    public readonly bool TransferHp;

    public TransferDamageOnDeath(string target, float radius = 50, bool transferHp = false)
    {
        Target = GetObjectType(target);
        Radius = radius;
        TransferHp = transferHp;
    }
    public TransferDamageOnDeath(string target, float radius = 50)
    {
        Target = GetObjectType(target);
        Radius = radius;
        TransferHp = false;
    }
    public TransferDamageOnDeath(string target)
    {
        Target = GetObjectType(target);
        Radius = 50f;
        TransferHp = false;
    }
    public override void Death(Entity host)
    {
        if (!(host is Enemy enemy))
            return;

        if (!(host.GetNearestEntity(Radius, Target) is Enemy targetEntity))
            return;
        
        foreach (var (player, damage) in enemy.DamageStorage)
        {
            if (targetEntity.DamageStorage.ContainsKey(player))
                targetEntity.DamageStorage[player] += damage;
            else
                targetEntity.DamageStorage[player] = damage;
        }

        if (TransferHp)
        {
            enemy.MaxHp += host.Hp;
            enemy.Hp += host.Hp;
        }
    }
}