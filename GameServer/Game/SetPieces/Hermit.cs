using Common;

namespace RotMG.Game.SetPieces;

class Hermit : ISetPiece
{
    public int Size { get { return 32; } }

    public void RenderSetPiece(World world, IntPoint pos)
    {
        SetPieces.RenderFromMap(world, pos, GameResources.SetPieces["Hermit God"]);
    }
}
