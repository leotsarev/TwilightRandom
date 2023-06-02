using Twilight.Domain;
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

        //while (Players.Count < 8)
        //{
        //    Players.Add($"Запасной&nbsp;игрок&nbsp;{Players.Count + 1}");
        //}

        var randomizer = new Randomiser(gameModel, DefaultData.Factions, AllianceMode.None);
        var result = randomizer.Randomize();

        ConsolePrinter.PrintResult(result);

        ReadLine();
    }
}