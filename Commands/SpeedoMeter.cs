using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private List<ulong> SpeedoMeterActive = new List<ulong>();

    #region SpeedoMeter

    [ConsoleCommand("hizim")]
    [ConsoleCommand("speedim")]
    public void SpeedoMeter(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (SpeedoMeterActive.Any(x => x == player.SteamID))
        {
            SpeedoMeterActive = SpeedoMeterActive.Where(x => x != player.SteamID).ToList();
        }
        else
        {
            SpeedoMeterActive.Add(player.SteamID);
        }
    }

    private void SpeedoMeterOnTick(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (SpeedoMeterActive.Contains(player.SteamID))
        {
            if (!VoteInProgress && !KomAlVoteInProgress)
            {
                var buttons = player.Buttons;
                SharpTimerPrintHtml(player,
                $"<pre>Speed: <font color='#00FF00'>{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())}</font><br>" +
                            $"{((buttons & PlayerButtons.Left) != 0 ? "←" : "_")} " +
                            $"{((buttons & PlayerButtons.Forward) != 0 ? "W" : "_")} " +
                            $"{((buttons & PlayerButtons.Right) != 0 ? "→" : "_")}<br>" +
                            $"{((buttons & PlayerButtons.Moveleft) != 0 ? "A" : "_")} " +
                            $"{((buttons & PlayerButtons.Back) != 0 ? "S" : "_")} " +
                            $"{((buttons & PlayerButtons.Moveright) != 0 ? "D" : "_")} </pre>");
            }
        }
    }

    #endregion SpeedoMeter
}