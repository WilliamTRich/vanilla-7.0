using RotMG.Common;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class GroundTransformOnDeath : Behavior
    {
        public readonly ushort TileType;
        public readonly int Radius;
        public readonly int RelativeX;
        public readonly int RelativeY;
        public readonly ushort? From;
        
        public GroundTransformOnDeath(string tileId, int radius = 0, int relativeX = 0, int relativeY = 0, string from = null)
        {
            TileType = Resources.Id2Tile[tileId].Type;
            Radius = radius;
            RelativeX = relativeX;
            RelativeY = relativeY;
            From = from != null ? (ushort?) Resources.Id2Tile[from].Type : null;
        }
        public override void Death(Entity host)
        {
            var posX = (int) host.Position.X + RelativeX;
            var poxY = (int) host.Position.Y + RelativeY;
            for (var x = posX - Radius; x <= posX + Radius; x++)
            for (var y = poxY - Radius; y <= poxY + Radius; y++)
            {
                var tile = host.Parent.GetTile(x, y);
                if (tile == null)
                    continue;
                    
                if (tile.Type == TileType)
                    continue;
                
                if (From != null && tile.Type != From)
                    continue;

                host.Parent.UpdateTile(x, y, TileType);
            }
        }
    }
}