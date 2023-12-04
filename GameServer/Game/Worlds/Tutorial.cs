using Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Worlds;
public class Tutorial(Map map, WorldDesc desc) : World(map, desc) {
    private void CheckChicken() {
        const ushort chickenGodType = 0x6b1;

        var pos = GetRegion(Region.Enemy1).ToVector2();
        var entities = EntityChunks.HitTest(pos, 15f);
        bool exists = false;

        foreach(var entity in entities) {
            if(entity.Type == chickenGodType) {
                exists = true;
            }
        }

        if(!exists) {
            AddEntity(new Entity(chickenGodType, null), pos);
        }
    }

    public override int AddEntity(Entity en, Vector2 at, bool noIdChange = false) {
        if(en is Player plr && plr.Client != null && plr.Client.Account != null) {
            CheckChicken();
            plr.Client.Account.VisitedTutorial = true;
            plr.Client.Account.Save();
        }

        return base.AddEntity(en, at);
    }
}