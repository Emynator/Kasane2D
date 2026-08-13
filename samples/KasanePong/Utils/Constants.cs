namespace KasanePong.Utils;

public static class Constants
{
    public const int ScreenWidth = 320;
    public const int ScreenHeight = 240;
    public const int SpriteSize = 16;

    public const float PlayAreaTop = 0.0f + SpriteSize + 11;
    public const float PlayAreaBottom = ScreenHeight - SpriteSize;
    
    public const float MaxPaddleSpeed = 150.0f;
}