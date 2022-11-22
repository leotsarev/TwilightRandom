using MoreLinq;

namespace TwilightRandom;

internal class Randomiser
{
    public HashSet<string> Factions = new(new[]
{
        "The Arborec",
        "The Barony of Letnev",
        "The Clan of Saar",
        "The Embers of Muaat (Тлеющие с Муаата)",
        "The Emirates of Hacan",
        "The Federation of Sol",
        "The Ghosts of Creuss (Призраки Креусса)",
        "The L1Z1X Mindnet",
        "The Mentak Coalition",
        "The Naalu Collective",
        "The Nekro Virus (Некровирус)",
        "Sardakk N’orr",
        "The Universities of Jol-Nar (Университеты Жол-Нара)",
        "The Winnu",
        "The Xxcha Kingdom",
        "The Yin Brotherhood",
        "The Yssaril Tribes",
        "The Argent Flight",
        "The Empyrean",
        "The Mahact Gene-Sorcerers",
        "The Naaz-Rokha Alliance",
        "The Nomad (Кочевник)",
        "The Titans of Ul (Титаны Ула)",
        "The Vuil'Raith Cabal (Кабал вуил’райт)",
    });

    public HashSet<string> Colors = new(new[]
    {
        "Черный",
        "Желтый",
        "Фиолетовый",
        "Зеленый",
        "Синий",
        "Оранжевый",
        "Красный",
        "Розовый",
    });

    public HashSet<string> Players = new(new[]
    {
        "@leotsarev",
        "@GrayFox23b",
        "@Germesina",
        //"@Warpo",
        "@Korran_Horn",
        "@blakotya",
        "@Werrus",
        "Руслан",
    });

    public class PlayerRandomizeResult
    {
        public string PlayerName { get; set; }
        public string Color { get; set; }
        public string[] Factions { get; set; }

        public override string ToString()
        {
            return $"{PlayerName} - {Color} - {string.Join(",", Factions)}";
        }
    }

    public PlayerRandomizeResult[] Randomize()
    {
        while (Players.Count < 8)
        {
            Players.Add($"Запасной игрок {Players.Count + 1}");
        }

        var playersDict = Players.ToDictionary(player => player, player => new PlayerRandomizeResult { PlayerName = player });

        if (playersDict.TryGetValue("@Germesina", out var germesina))
        {
            germesina.Color = "Черный";
            Colors.Remove(germesina.Color);
        }

        foreach (var result in playersDict.Values.Where(r => r.Color is null))
        {
            result.Color = SelectAndRemoveRandom(Colors);
        }

        foreach (var result in playersDict.Values.Where(r => r.Factions is null))
        {
            result.Factions = new[] { SelectAndRemoveRandom(Factions), SelectAndRemoveRandom(Factions) };
        }

        return playersDict.Values.Shuffle().ToArray();
    }

    private static string SelectAndRemoveRandom(HashSet<string> set)
    {
        var idx = Random.Shared.Next(0, set.Count);
        var selected = set.ElementAt(idx);
        set.Remove(selected);
        return selected;
    }
}
