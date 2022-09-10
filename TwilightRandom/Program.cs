namespace TwilightRandom
{
    internal class Program
    {


        static void Main(string[] args)
        {
            var randomizer = new Randomiser();
            var result = randomizer.Randomize();
            foreach(var x in result)
            {
                Console.WriteLine(x);
            }

            Console.WriteLine("Невыбранные фракции");
            foreach (var f in randomizer.Factions)
            {
                Console.WriteLine(f);

            }
        }
    }
}