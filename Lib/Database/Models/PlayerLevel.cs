using System.ComponentModel.DataAnnotations;

namespace JailbreakExtras.Lib.Database.Models;

public class PlayerLevel
{
    public PlayerLevel(ulong steamId)
    {
        SteamId = steamId;
    }

    public ulong SteamId { get; set; }

    [Range(0, int.MaxValue)]
    public int Xp { get; set; } = 0;
}