namespace TwilightRandom
{
    internal static class ConsolePrinter
    {
        public static void PrintResult(RandomizeResult result)
        {
            Console.WriteLine("|Имя игрока|Цвет|Выбранные фракции|Победные очки|");
            Console.WriteLine("|--|--|--|--|");
            foreach (var x in result.Players)
            {
                Console.WriteLine($"|{x.PlayerName}|{x.Color}|{string.Join("<br />", x.Factions)}|- |");
            }

            Console.WriteLine("Невыбранные фракции");
            foreach (var f in result.UnselectedFactions)
            {
                Console.WriteLine(f);
            }
        }
    }
}
