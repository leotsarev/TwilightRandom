using System.Diagnostics;
using Tomlyn;
using TwilightRandom;

namespace Twilight.Console;

internal class ConfigLoader
{
    public static GameRequest? Get()
    {
        var iniFileName = "twinlight_players.ini";
        var gameModel = LoadConfigFromIni(iniFileName);

        if (gameModel.Players is null)
        {
            var notepad = Process.Start("notepad.exe", iniFileName);
            notepad.WaitForExit();

            gameModel = LoadConfigFromIni(iniFileName);
            if (gameModel.Players is null)
            {
                return null;
            }
        }

        return gameModel;
    }

    private static GameRequest LoadConfigFromIni(string iniFileName)
    {
        string iniFile = File.ReadAllText(iniFileName);
        var gameModel = Toml.ToModel<GameRequest>(iniFile);
        return gameModel;
    }
}
