using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("PatronuKoruMapDeis")]
    [ConsoleCommand("PatronuKoruMapDegis")]
    [ConsoleCommand("EtkinlikSonuMapDegis")]
    [ConsoleCommand("EtkinlikMapDegis")]
    [ConsoleCommand("EtkinlikBitisMapDegis")]
    [ConsoleCommand("EtkinlikBitir")]
    public void PatronuKoruMapDeis(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        MapDKLastVoterSteamId = player.SteamID;
        Answers = new()
            {
                {"Değiş",1 },
                {"Kal",0 },
            };
        MapDkFinished(Answers);
    }
}