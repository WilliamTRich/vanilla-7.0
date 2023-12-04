using Common;

namespace RotMG.Game.SetPieces;

class KageKami : ISetPiece
{
    public int Size { get { return 65; } }

    public void RenderSetPiece(World world, IntPoint pos)
    {
        SetPieces.RenderFromMap(world, pos, GameResources.SetPieces["Kage Kami"]);
    }
}
