using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Dur-Devam

    [ConsoleCommand("dur", "herkesi durdurma")]
    public void Dur(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.ExecuteCommand($"sv_maxspeed 0");
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Herkes durduruldu.");
    }

    [ConsoleCommand("devam", "herkesi durdurma")]
    public void Devam(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.ExecuteCommand($"sv_maxspeed 320");
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Yürüme düzeltildi.");
    }

    #endregion Dur-Devam
}