using TwilightRandom;
using static System.Console;
namespace Twilight.Console;

internal class Program
{
    static void Main(string[] args)
    {
        GameRequest? gameModel = ConfigLoader.Get();

        if (gameModel is null)
        {
            WriteLine("Укажите игроков!..");
            ReadLine();
            return;
        }

        var randomizer = new Randomiser(gameModel, DefaultData.Factions);
        var result = randomizer.Randomize();

        ConsolePrinter.PrintResult(result);

        ReadLine();
    }
}