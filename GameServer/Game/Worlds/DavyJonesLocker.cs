using Common;

namespace RotMG.Game.Worlds;

public class DavyJonesLocker : World
{
    public DavyJonesLocker(Map map, WorldDesc desc) : base(map, desc)
    {
        SLog.Info("[DJL Setup Running]");
        SpawnKeys();
    }

    private void SpawnKeys()
    {
        var greenSpawns = GetAllRegion(Region.Enemy1);
        var redSpawns = GetAllRegion(Region.Enemy2);
        var yellowSpawns = GetAllRegion(Region.Enemy3);

        AddEntity(new Entity(0x0e3f), greenSpawns[Manager.DungeonRNG.Next(greenSpawns.Count)].ToVector2()); //Green key
        AddEntity(new Entity(0x0e40), redSpawns[Manager.DungeonRNG.Next(redSpawns.Count)].ToVector2()); //Red Key
        AddEntity(new Entity(0x0e41), yellowSpawns[Manager.DungeonRNG.Next(yellowSpawns.Count)].ToVector2()); //Yellow key

    }

    //A Behavior will run this code when player within range
    public void OpenDoors(int i)
    {
        switch(i)
        {
            //Purple doors
            case 0:
                foreach(var pos in GetAllRegion(Region.Decoration1))
                {
                    RemoveStatic(pos.X, pos.Y);
                }
                break;
                //Green doors
            case 1:
                foreach (var pos in GetAllRegion(Region.Decoration2))
                {
                    RemoveStatic(pos.X, pos.Y);
                }
                break;
                //red doors
            case 2:
                foreach (var pos in GetAllRegion(Region.Decoration3))
                {
                    RemoveStatic(pos.X, pos.Y);
                }
                break;
                //yellow doors
            case 3:
                foreach (var pos in GetAllRegion(Region.Decoration4))
                {
                    RemoveStatic(pos.X, pos.Y);
                }
                break;
        }
    }
}