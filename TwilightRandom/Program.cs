using System.Diagnostics;
using Tomlyn;

namespace TwilightRandom;

internal class Program
{
    static void Main(string[] args)
    {
        GameRequest? gameModel = ConfigLoader.Get();

        if (gameModel is null)
        {
            Console.WriteLine("Укажите игроков!..");
            Console.ReadLine();
            return;
        }

        var randomizer = new Randomiser(gameModel);
        var result = randomizer.Randomize();

        ConsolePrinter.PrintResult(result);

        Console.ReadLine();
    }


}