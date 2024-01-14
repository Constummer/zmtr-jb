using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RR

    [ConsoleCommand("kafakapat")]
    [ConsoleCommand("kafakapa")]
    [ConsoleCommand("hskapa")]
    [ConsoleCommand("hskapat")]
    public void KafaKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.ExecuteCommand($"mp_damage_headshot_only 0");
        Server.PrintToChatAll($"{Prefix} {CC.W} Headshot-only kapandı.");
    }

    [ConsoleCommand("kafaac")]
    [ConsoleCommand("hsac")]
    public void KafaAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.ExecuteCommand($"mp_damage_headshot_only 1");
        Server.PrintToChatAll($"{Prefix} {CC.W} Headshot-only açıldı.");
    }

    #endregion RR
}