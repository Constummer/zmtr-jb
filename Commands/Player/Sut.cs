using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Sut

    private static List<ulong> SutolCommandCalls = new();
    private static List<ulong> SutolCommandCallForBPs = new();

    [ConsoleCommand("sut")]
    [ConsoleCommand("sutol")]
    public void Sut(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (GetTeam(player) != CsTeam.Terrorist)
        {
            player.PrintToChat($"{Prefix}{CC.W} Sadece T ler bu komutu kullanabilir.");
            return;
        }

        GetPlayers()
        .Where(x => x.PawnIsAlive && x.SteamID == player!.SteamID)
        .ToList()
        .ForEach(x =>
        {
            //SetColour(x, Color.FromArgb(128, 0, 128));
            SetModelNextServerFrame(x, "characters/models/ambrosian/zmtr/sut/sut.vmdl");
            SutolCommandCalls.Add(x.SteamID);

            RefreshPawnTP(x);
            RemoveWeapons(x, false);
            if (GetPlayerCount() > 10 && LatestWCommandUser != null)
            {
                if (SutolCommandCallForBPs.Any(y => y == x.SteamID) == false)
                {
                    SutolCommandCallForBPs.Add(x.SteamID);
                    if (BattlePassDatas.TryGetValue(x.SteamID, out var battlePassData))
                    {
                        battlePassData?.OnSutCommand();
                    }
                    if (BattlePassPremiumDatas.TryGetValue(x.SteamID, out var battlePassPremiumData))
                    {
                        battlePassPremiumData?.OnSutCommand();
                    }
                }
            }

            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı oyuncu, {CC.LP}süt oldu.");
        });
    }

    [ConsoleCommand("suttemizle")]
    public void SutTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        SutolCommandCalls.Clear();

        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.LP}süt isimlerini temizledi.");
    }

    #endregion Sut
}