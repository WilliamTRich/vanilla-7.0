using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Worlds;

public class CandyLand : World
{
    private const int RESPAWN_DELAY_IN_MS = 15000;
    private const int INITIAL_SPAWN_DELAY_IN_MS = 1000;
    private const int STARTING_MINIMUM_AMOUNT = 0;
    private const int POSSIBLE_ADDITIONAL_AMOUNT = 10;

    private int _cooldown = INITIAL_SPAWN_DELAY_IN_MS;
    private int _killsRequired = STARTING_MINIMUM_AMOUNT;
    private int _stackedBosses = 0;

    //string[] _importantNames = new string[] {"Beefy Fairy"};
    string[] _importantNames = new string[] {"Beefy Fairy" ,"Big Creampuff","Wishing Troll","Rototo", "Unicorn"};
    string[] _bossNames = new string[] {"Swoll Fairy" ,"Spoiled Creampuff","Desire Troll","MegaRototo", "Gigacorn"};
    
    private Vector2 _bossPosition;

    private List<IntPoint> _spawn1;
    private List<IntPoint> _spawn2;
    private List<IntPoint> _spawn3;
    private List<IntPoint> _spawn4;
    private List<IntPoint> _spawn5;
    public CandyLand(Map map, WorldDesc desc) : base(map, desc)
    {
        _bossPosition = GetRegion(Region.Enemy1).ToVector2();

        _spawn1 = GetAllRegion(Region.Enemy2);
        _spawn2 = GetAllRegion(Region.Enemy3);
        _spawn3 = GetAllRegion(Region.Enemy4);
        _spawn4 = GetAllRegion(Region.Enemy5);
        _spawn5 = GetAllRegion(Region.Enemy6);

        _killsRequired += Manager.DungeonRNG.Next(0, POSSIBLE_ADDITIONAL_AMOUNT);
    }

    public override void RemoveEntity(Entity en)
    {
        var enName = en.Desc.Id;

        base.RemoveEntity(en);
        
        //When an important entity dies lower the kills required
        if(_importantNames.Contains(enName))
        {
            _killsRequired--;
        }

        //When a boss dies try to spawn another from the stacked count
        if(_bossNames.Contains(enName) && _stackedBosses > 0)
        {
            _stackedBosses--;

            SpawnBoss();
        }

        if(_killsRequired <= 0)
        {
            _killsRequired += Manager.DungeonRNG.Next(1, 10);

            var entitiesByBoss = EntityChunks.HitTest(_bossPosition, 20f);
            bool isQuestNearby = false;
            foreach(var entity in entitiesByBoss)
            {
                if(entity.Desc.Quest)
                {
                    isQuestNearby = true;
                    break;
                }
            }

            //Only quest entities are bosses in cland
            if(!isQuestNearby)
            {
                SpawnBoss();
            }
            else
            {
                _stackedBosses++;
            }

        }
        
    }

    public override void Tick()
    {
        _cooldown -= Settings.MillisecondsPerTick;

        if(_cooldown <= 0)
        {
            _cooldown = RESPAWN_DELAY_IN_MS;

            CheckSpawnLocations(_spawn1);
            CheckSpawnLocations(_spawn2);
            CheckSpawnLocations(_spawn3);
            CheckSpawnLocations(_spawn4);
            CheckSpawnLocations(_spawn5);

        }


        base.Tick();
    }

    private void CheckSpawnLocations(List<IntPoint> points)
    {
        foreach(var location in points)
        {
            Vector2 pos = location.ToVector2();
            bool occupied = false;
            var entities = EntityChunks.HitTest(pos, 10f);
            foreach(var entity in entities)
            {
                if(_importantNames.Contains(entity.Desc.Id))
                {
                    occupied = true;
                    break;
                }
            }

            if(!occupied)
            {
                SpawnImportantEntity(pos);
            }
        }
    }

    private void SpawnImportantEntity(Vector2 vector2)
    {
        var entity = Entity.Resolve(Resources.Id2Object[_importantNames[Manager.DungeonRNG.Next(0, _importantNames.Length)]].Type);

        AddEntity(entity, vector2);
    }

    private void SpawnBoss()
    {
        var bossPos = GetRegion(Region.Enemy1).ToVector2();
        var entity = Entity.Resolve(Resources.Id2Object[_bossNames[Manager.DungeonRNG.Next(0, _bossNames.Length)]].Type);
        AddEntity(entity, bossPos);
    }
}