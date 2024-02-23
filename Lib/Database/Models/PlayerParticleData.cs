namespace JailbreakExtras.Lib.Database.Models;

public class PlayerParticleData
{
    public PlayerParticleData(ulong steamId)
    {
        SteamId = steamId;
    }

    public ulong SteamId { get; set; }

    public string? BoughtModelIds { get; set; }

    public string? SelectedModelId { get; set; }

    public List<int> GetBoughtModelIds()
    {
        if (string.IsNullOrWhiteSpace(BoughtModelIds))
        {
            return new();
        }
        return BoughtModelIds.Split(',').Select(x => int.Parse(x)).ToList();
    }
}