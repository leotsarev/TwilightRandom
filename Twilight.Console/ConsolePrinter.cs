using TwilightRandom;
using static System.Console;

namespace Twilight.Console
{
    internal static class ConsolePrinter
    {
        public static void PrintResult(RandomizeResult result)
        {
            WriteLine("|Имя игрока|Цвет|Выбранные фракции|Победные очки|");
            WriteLine("|--|--|--|--|");
            foreach (var x in result.Players)
            {
                WriteLine($"|{x.PlayerName}|{x.Color}|{string.Join("<br />", x.Factions.Select(f => f.ToString()))}|- |");
            }

            WriteLine("Невыбранные фракции");
            foreach (var f in result.UnselectedFactions)
            {
                WriteLine(f);
            }
        }
    }
}
