using JoinRpg.Common.PrimitiveTypes;

namespace Twilight.Web.GamePlayers;

/// <summary>
/// Shared between initial game creation (<see cref="Pages.GameCreateModel"/>) and adding a single
/// player to an existing game (<see cref="Pages.GamePlayerAddModel"/>): parses a player name, optionally
/// followed by a joinrpg id after "#", e.g. "Вася #12345".
/// </summary>
public static class PlayerLineParser
{
    public static PlayerInput Parse(string line)
    {
        var parts = line.Split('#', 2, StringSplitOptions.TrimEntries);
        if (parts.Length == 2 && int.TryParse(parts[1], out var joinrpgUserId))
        {
            return new PlayerInput(parts[0], new UserIdentification(joinrpgUserId));
        }

        return new PlayerInput(line, null);
    }
}

public record PlayerInput(string Name, UserIdentification? JoinrpgUserId);
