
internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new Sokoban.App();
        game.Run();
    }
}