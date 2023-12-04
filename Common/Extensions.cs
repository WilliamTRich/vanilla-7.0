using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common;

public static class IntPointExt
{
    public static Vector2 ToVector2(this IntPoint point) => new(point.X, point.Y);
}

public static class Vector2Ext
{
    public static IntPoint ToIntPoint(this Vector2 vec) => new((int)vec.X, (int)vec.Y);
}
