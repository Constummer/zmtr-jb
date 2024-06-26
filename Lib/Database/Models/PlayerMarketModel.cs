﻿using System.ComponentModel.DataAnnotations;

namespace JailbreakExtras.Lib.Database.Models;

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

    public string? DefaultIdCT { get; set; }

    public string? DefaultIdT { get; set; }
}