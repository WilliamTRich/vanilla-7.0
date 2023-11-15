using RotMG.Common;
using RotMG.Game;
using RotMG.Game.Entities;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Utils
{
    public static class GameUtils
    {
        static Random _R = new Random();
        public static int GetEnumLength<T>()
        {
            return Enum.GetValues(typeof(T)).Length;
        }
        public static T RandomEnumValue<T>() //wont sync with client random
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_R.Next(v.Length));
        }
        public static int GetDefenseDamage(this Entity entity, int damage, int defense, bool pierces)
        {
#if DEBUG
            if (entity == null || entity.Parent == null)
                return 0;//throw new Exception("Undefined entity");
#endif
            if (entity.HasConditionEffect(ConditionEffectIndex.ArmorBroken))
                pierces = true;

            if (pierces)
                defense = 0;

            if (entity.HasConditionEffect(ConditionEffectIndex.Armored))
                defense *= 2;

            var min = damage * 3 / 20;
            var d = Math.Max(min, damage - defense);

            if (entity.HasConditionEffect(ConditionEffectIndex.Invulnerable))
                d = 0;

            return d;
        }

        public static Entity GetNearestEntity(this Entity entity, float radius, ushort? objType = null)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif
            if (objType == null)
                return GetNearestPlayer(entity, radius);

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                float d;
                if (objType != null && en.Type != objType)
                    continue;

                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static IEnumerable<Entity> GetEntitiesInLine(this Entity entity, Vector2 line)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || line.Length() < 0)
                throw new Exception();
#endif

            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, line.Length()))
            {
                var w = en.Position - entity.Position;
                var cosA = line.Dot(w) / (line.Length() * w.Length());
                var sinA = MathF.Sqrt(1 - cosA * cosA);
                var d = w.Length() * sinA;

                if (d <= Player.EnemyHitRangeAllowance)
                    yield return en;
            }
        }

        public static Entity GetNearestEntity(this Entity entity, float radius, float angle, float cone)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (Math.Abs(angle - MathF.Atan2(en.Position.Y - entity.Position.Y, en.Position.X - entity.Position.X)) > cone)
                    continue;

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEntity(this World world, Vector2 position, float radius)
        {
#if DEBUG
            if (world == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in world.EntityChunks.HitTest(position, radius))
            {
                float d;
                if (en.Desc.Player)
                    continue;

                if ((d = position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static IEnumerable<Entity> GetNearestEntitiesByType(this Entity entity, float radius, ushort objType)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (en.Type == objType)
                    yield return en;
            }
        }

        public static IEnumerable<Entity> GetNearestEntitiesByGroup(this Entity entity, float radius, string group)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (en.Desc.Group == group)
                    yield return en;
            }
        }

        //always check for null returns
        public static Entity GetNearestEnemy(this Entity entity, float radius)
        {
            if (entity == null || entity.Parent == null || radius <= 0)
                return null;

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (!(en is Enemy))
                    continue;

                if (en.HasConditionEffect(ConditionEffectIndex.Invincible))
                    continue;

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEntityByName(this Entity entity, float radius, string entityName)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            var targetEn = Resources.Id2Object[entityName].Type;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (en.Type != targetEn)
                    continue;

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEnemy(this Entity entity, float radius, float angle, float cone, Vector2 target)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (!(en is Enemy))
                    continue;

                if (en.HasConditionEffect(ConditionEffectIndex.Invincible))
                    continue;

                if (Math.Abs(angle - MathF.Atan2(en.Position.Y - entity.Position.Y, en.Position.X - entity.Position.X)) > cone)
                    continue;

                float d;
                if ((d = target.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEnemy(this Entity entity, float radius, float angle, float cone, Vector2 target, HashSet<Entity> exclude)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (!(en is Enemy))
                    continue;

                if (en.HasConditionEffect(ConditionEffectIndex.Invincible))
                    continue;

                if (Math.Abs(angle - MathF.Atan2(en.Position.Y - entity.Position.Y, en.Position.X - entity.Position.X)) > cone)
                    continue;

                if (exclude.Contains(en))
                    continue;

                float d;
                if ((d = target.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEnemy(this Entity entity, float radius, HashSet<Entity> exclude)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (!(en is Enemy))
                    continue;

                if (en.HasConditionEffect(ConditionEffectIndex.Invincible))
                    continue;

                if (exclude.Contains(en))
                    continue;

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestPlayer(this Entity entity, float radius, bool seeInvis = false)
        {
#if DEBUG
            if (entity == null || entity.Parent == null)
                throw new Exception();
#endif
            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.PlayerChunks.HitTest(entity.Position, radius))
            {

                if (!seeInvis && en.HasConditionEffect(ConditionEffectIndex.Invisible))
                    continue;

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetLowestHpPlayer(this Entity entity, float radius)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif
            Entity lowest = null;
            var low = float.MaxValue;
            foreach (var en in entity.Parent.PlayerChunks.HitTest(entity.Position, radius))
            {
                if (en.HasConditionEffect(ConditionEffectIndex.Invisible))
                    continue;

                float d;
                if ((d = entity.Hp) < low)
                {
                    lowest = en;
                    low = d;
                }
            }
            return lowest;
        }

        public static IEnumerable<Entity> GetNearestPlayers(this Entity entity, float radius, bool seeInvis = false)
        {
#if DEBUG
            if (entity == null || entity.Parent == null)
                throw new Exception();
#endif
            foreach (var en in entity.Parent.PlayerChunks.HitTest(entity.Position, radius))
            {
                if (!seeInvis && en.HasConditionEffect(ConditionEffectIndex.Invisible))
                    continue;

                yield return en;
            }
        }

        public static int CountPlayerEntities(this Entity entity, float radius, bool seeInvis = false)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif
            var ret = 0;
            foreach (var i in entity.Parent.PlayerChunks.HitTest(entity.Position, radius))
            {
                if (!i.HasConditionEffect(ConditionEffectIndex.Invisible) && !seeInvis)
                    continue;

                var d = i.Position.Distance(entity);
                if (d < radius) ret++;
            }

            return ret;
        }

        public static int CountEntities(this Entity entity, float radius, ushort objType)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif
            var ret = 0;
            foreach (var i in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (i.Type != objType) continue;
                var d = i.Position.Distance(entity);
                if (d < radius)
                    ret++;
            }

            return ret;
        }

        public static int CountEntities(this Entity entity, float radius, string group)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif
            var ret = 0;
            foreach (var i in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (i.Desc.Group != group) continue;
                var d = i.Position.Distance(entity);
                if (d < radius)
                    ret++;
            }
            return ret;
        }
    }
}
