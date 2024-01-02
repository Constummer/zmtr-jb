using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventRoundStart()
    {
        RegisterEventHandler<EventRoundStart>((@event, _) =>
        {
            PrepareRoundDefaults();
            ClearAll();

            CoinAfterNewCommander();
            AddTimer(1.0f, () =>
            {
                CoinGo = true;
            });
            HookDisabled = true;
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}Hook 5 sn boyunca kapalý");

            AddTimer(5.0f, () =>
            {
                HookDisabled = false;
                Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}Hook açýk");
            });
            foreach (var x in GetPlayers())
            {
                if (x.SteamID == LatestWCommandUser)
                {
                    x.VoiceFlags &= ~VoiceFlags.Muted;
                }
                else
                {
                    if (Unmuteds.Contains(x.SteamID) == false)
                    {
                        x.VoiceFlags |= VoiceFlags.Muted;
                    }
                }
                if (HideFoots.TryGetValue(x.SteamID, out bool hideFoot))
                {
                    if (hideFoot)
                    {
                        x.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);
                        RefreshPawn(x);
                    }
                    else
                    {
                        x.PlayerPawn.Value.Render = DefaultPlayerColor;
                        RefreshPawn(x);
                    }
                }
            }
            return HookResult.Continue;
        });
    }
}