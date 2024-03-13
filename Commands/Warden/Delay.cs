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
        if (LatestWCommandUser != player.SteamID)
        {
            if (ValidateCallerPlayer(player) == false)
            {
                return;
            }
        }
        else
        {
            if (ValidateCallerPlayer(player, false) == false)
            {
                return;
            }
        }
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : "5";
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
        Server.PrintToChatAll($"{Prefix} {CC.W}{CT_PluralCamel}, {value} saniye mutelendi.");

        GetPlayers(CsTeam.CounterTerrorist)
            .ToList()
            .ForEach(x =>
        {
            if (ValidateCallerPlayer(x, false) == false) return;
            x.VoiceFlags |= VoiceFlags.Muted;
        });
        BasicCountdown.CommandStartTextCountDown(this, $"{CT_PluralCamel}, {value} saniye mutelendi");

        _ = AddTimer(value, () =>
        {
            GetPlayers(CsTeam.CounterTerrorist)
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                Unmuteds.Add(x.SteamID);
                x.VoiceFlags &= ~VoiceFlags.Muted;
            });
            FreezeOrUnfreezeSound();
            Server.PrintToChatAll($"{Prefix} {CC.W}{CT_PluralCamelPossesive}, mutesi kaldırıldı.");
        }, SOM);
    }

    #endregion Delay
}