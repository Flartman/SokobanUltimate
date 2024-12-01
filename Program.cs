
internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new SokobanUltimate.Game1();
        game.Run();
    }
}