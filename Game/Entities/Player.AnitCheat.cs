using System;
using System.Collections.Generic;
using System.IO;
using RotMG.Common;
using RotMG.Networking;
using System.Linq;
using System.Text.RegularExpressions;
using RotMG.Game.SetPieces;
using RotMG.Game.Worlds;
using RotMG.Utils;
using RotMG.Game.Logic.Commands;
using SimpleLog;
using RotMG.Game.Logic.Behaviors;
using System.Runtime.InteropServices.JavaScript;

namespace RotMG.Game.Entities;

public partial class Player
{
    private const float MaxTimeDiff = 1.08f;
    private const float MinTimeDiff = 0.92f;

    private long LastAttackTime = -1;
    private int Shots;
    public PlayerShootStatus ValidatePlayerShoot(ItemDesc item, long time)
    {
        if (item.Type != Inventory[0])
            return PlayerShootStatus.ITEM_MISMATCH;

        //start

        if (time == LastAttackTime)
        {
            if (++Shots > item.NumProjectiles)
                return PlayerShootStatus.NUM_PROJECTILE_MISMATCH;
        }
        else
        {
            var attackPeriod = (int)(1.0 / GetAttackFrequency() * 1.0 / item.RateOfFire);
            if (time < LastAttackTime + attackPeriod)
                return PlayerShootStatus.COOLDOWN_STILL_ACTIVE;
            LastAttackTime = time;
            Shots = 1;
        }

        //end

        return PlayerShootStatus.OK;
    }
    public bool IsNoClipping()
    {
        if (Parent == null || !TileOccupied(Position.X, Position.Y) && !TileFullOccupied(Position.X, Position.Y))
            return false;

        SLog.Info($"{Name} is walking on an occupied tile.");
        return true;
    }

    public enum PlayerShootStatus
    {
        OK,
        ITEM_MISMATCH,
        COOLDOWN_STILL_ACTIVE,
        NUM_PROJECTILE_MISMATCH,
        CLIENT_TOO_SLOW,
        CLIENT_TOO_FAST
    }

}
