﻿using System;
using System.Linq;
using Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.SetPieces;

class TempleB : Temple
{
    public override int Size { get { return 60; } }

    
    public override void RenderSetPiece(World world, IntPoint pos)
    {
        var t = new int[Size, Size];
        var o = new int[Size, Size];

        for (var x = 0; x < 60; x++)                    //Flooring
            for (var y = 0; y < 60; y++)
            {
                if (Math.Abs(x - Size / 2) / (Size / 2.0) + MathUtils.NextFloat() * 0.3 < 0.9 &&
                    Math.Abs(y - Size / 2) / (Size / 2.0) + MathUtils.NextFloat() * 0.3 < 0.9)
                {
                    var dist = Math.Sqrt(((x - Size / 2) * (x - Size / 2) + (y - Size / 2) * (y - Size / 2)) / ((Size / 2.0) * (Size / 2.0)));
                    t[x, y] = MathUtils.NextFloat() < (1 - dist) * (1 - dist) ? 2 : 1;
                }
            }

        for (var x = 0; x < Size; x++)                  //Corruption
            for (var y = 0; y < Size; y++)
                if (MathUtils.Chance(.02f))
                    t[x, y] = 0;

        const int bas = 16;                             //Walls
        for (var x = 0; x < 23; x++)
        {
            if (x > 9 && x < 13) continue;

            o[bas + x, bas] = 2;
            o[bas + x, bas + 1] = 2;

            o[bas + x, bas + 21] = 2;
            o[bas + x, bas + 22] = 2;
        }
        for (var y = 0; y < 23; y++)
        {
            if (y > 9 && y < 13) continue;

            o[bas, bas + y] = 2;
            o[bas + 1, bas + y] = 2;

            o[bas + 21, bas + y] = 2;
            o[bas + 22, bas + y] = 2;
        }
        o[bas - 1, bas + 7] = o[bas - 1, bas + 8] = o[bas - 1, bas + 9] =
            o[bas - 1, bas + 13] = o[bas - 1, bas + 14] = o[bas - 1, bas + 15] = 1;
        o[bas + 23, bas + 7] = o[bas + 23, bas + 8] = o[bas + 23, bas + 9] =
            o[bas + 23, bas + 13] = o[bas + 23, bas + 14] = o[bas + 23, bas + 15] = 1;
        o[bas + 7, bas - 1] = o[bas + 8, bas - 1] = o[bas + 9, bas - 1] =
            o[bas + 13, bas - 1] = o[bas + 14, bas - 1] = o[bas + 15, bas - 1] = 1;
        o[bas + 7, bas + 23] = o[bas + 8, bas + 23] = o[bas + 9, bas + 23] =
            o[bas + 13, bas + 23] = o[bas + 14, bas + 23] = o[bas + 15, bas + 23] = 1;


        for (var y = 0; y < 4; y++)                     //Columns
            for (var x = 0; x < 4; x++)
                o[bas + 5 + x * 4, bas + 5 + y * 4] = 3;

        for (var x = 0; x < Size; x++)                  //Plants
            for (var y = 0; y < Size; y++)
            {
                if (((x > 5 && x < bas) || (x < Size - 5 && x > Size - bas) ||
                     (y > 5 && y < bas) || (y < Size - 5 && y > Size - bas)) &&
                    o[x, y] == 0 && t[x, y] == 1)
                {
                    double r = MathUtils.NextFloat();
                    if (r > 0.6)        //0.4
                        o[x, y] = 4;
                    else if (r > 0.35)  //0.25
                        o[x, y] = 5;
                    else if (r > 0.33)  //0.02
                        o[x, y] = 6;
                }
            }

        int rotation = MathUtils.Next(4);               //Rotation
        for (var i = 0; i < rotation; i++)
        {
            t = SetPieces.RotateCW(t);
            o = SetPieces.RotateCW(o);
        }

        Render(this, world, pos, t, o);

        //Boss & Chest
        
        var c = new Container(0x0501, -1, null);
        var loot = chest.GetLoots(3, 8).ToArray();
        for (var k = 0; k < loot.Length; k++)
        {
            var roll = Resources.Type2Item[loot[k]].Roll();
            c.Inventory[k] = loot[k];
            c.ItemDatas[k] = roll.Item1 ? (int) roll.Item2 : -1;
            c.UpdateInventorySlot(k);
        }

        world.AddEntity(c, new Vector2(pos.X + bas + 11.5f, pos.Y + bas + 11.5f));

        var snake = Entity.Resolve(0x0dc2);
        world.AddEntity(snake, new Vector2(pos.X + bas + 11.5f, pos.Y + bas + 11.5f));
    }
}
