using Common;

namespace RotMG.Game.SetPieces;

class GhostShip : ISetPiece
{
    public int Size { get { return 40; } }

    public void RenderSetPiece(World world, IntPoint pos)
    {
        SetPieces.RenderFromMap(world, pos, GameResources.SetPieces["Ghost Ship"]);
    }
}
