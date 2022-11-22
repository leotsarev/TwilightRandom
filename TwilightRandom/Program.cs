namespace TwilightRandom;

internal class Program
{
    static void Main(string[] args)
    {
        var randomizer = new Randomiser();
        var result = randomizer.Randomize();
        Console.WriteLine("|Имя игрока|Цвет|Выбранные фракции|Победные очки|");
        Console.WriteLine("|--|--|--|--|");
        foreach (var x in result)
        {
            Console.WriteLine($"|{x.PlayerName}|{x.Color}|{string.Join("<br />", x.Factions)}|- |");
        }

        Console.WriteLine("Невыбранные фракции");
        foreach (var f in randomizer.Factions)
        {
            Console.WriteLine(f);

        }
    }
}