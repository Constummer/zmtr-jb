using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Delay

    [ConsoleCommand("delay")]
    [CommandHelper(0, "<saniye>")]
    public void Delay(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.GetArg(1) : "5";
        if (int.TryParse(target, out var value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
                return;
            }
            else if (value < 5)
            {
                player.PrintToChat("Min 5 sn girebilirsin");
                return;
            }
            else
            {
                DelayCt(value); return;
            }
        }
        else
        {
            DelayCt(value); return;
        }
    }

    private void DelayCt(int value)
    {
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Gardiyanlar, {value} saniye mutelendi.");

        GetPlayers(CsTeam.CounterTerrorist)
            .ToList()
            .ForEach(x =>
        {
            x.VoiceFlags |= VoiceFlags.Muted;
        });
        BasicCountdown.CommandStartTextCountDown(this, $"Gardiyanlar, {value} saniye mutelendi");

        _ = AddTimer(value, () =>
        {
            GetPlayers(CsTeam.CounterTerrorist)
            .ToList()
            .ForEach(x =>
            {
                Unmuteds.Add(x.SteamID);
                x.VoiceFlags &= ~VoiceFlags.Muted;
            });
            FreezeOrUnfreezeSound();
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Gardiyanların, mutesi kaldırıldı.");
        });
    }

    #endregion Delay
}