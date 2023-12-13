using System.ComponentModel.DataAnnotations;

namespace JailbreakExtras;

public class PlayerMarketModel
{
    public PlayerMarketModel(ulong steamId)
    {
        SteamId = steamId;
    }

    public ulong SteamId { get; set; }

    [Range(0, int.MaxValue)]
    public int Credit { get; set; } = 0;

    public string? ModelIdCT { get; set; }

    public string? ModelIdT { get; set; }

    public int? DefaultIdCT { get; set; }

    public int? DefaultIdT { get; set; }
}