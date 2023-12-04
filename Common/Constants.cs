namespace Common;
public static class Constants {
    public const int MaxLatencyMS = 10000;
    public const int MaxPotions = 6;

    public const int Star1 = 20;
    public const int Star2 = 150;
    public const int Star3 = 400;
    public const int Star4 = 800;
    public const int Star5 = 2000;

    private static int[] _stars = [ Star1, Star2, Star3, Star4, Star5];
    public static int GetStar(int idx) => _stars[idx];
    public static int GetStarsLength() => _stars.Length;
}