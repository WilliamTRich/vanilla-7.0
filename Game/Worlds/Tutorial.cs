using System;
using System.Collections.Generic;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Worlds
{
    public class Tutorial : World
    {
        private int _cooldown = 15000;
        public Tutorial(Map map, WorldDesc desc) : base(map, desc)
        {
           
        }
        public override void Tick()
        {
            _cooldown -= Settings.MillisecondsPerTick;
            if(_cooldown < 0)
            {
                _cooldown = 15000;

                CheckChicken();

            }
            base.Tick();
        }

        private void CheckChicken()
        {
            var pos = GetRegion(Region.Enemy1).ToVector2();
            var entities = EntityChunks.HitTest(pos, 15f);
            bool exists = false;
            foreach(var entity in entities)
            {
                if(entity.Type == 0x6b1)//Evil Chicken God
                {
                    exists = true;
                }
            }

            if(!exists)
            {
                AddEntity(new Entity(0x6b1, null), pos);
            }
        }

        public override int AddEntity(Entity en, Vector2 at)
        {
            if(en is Player plr && plr.Client != null && plr.Client.Account != null)
            {
                plr.Client.Account.VisitedTutorial = true;
                plr.Client.Account.Save();
            }

            return base.AddEntity(en, at);
        }
    }
}