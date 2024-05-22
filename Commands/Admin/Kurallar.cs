using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Kurallar

    [ConsoleCommand("kurallar")]
    public void Kurallar(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player.PrintToChat(@$"{Prefix}{CC.W}  * Discord gelerek GÜNCEL kuralları öğrenebilirsiniz. !dc");
        player.PrintToChat(@$"{Prefix}{CC.W}  * Discord gelerek GÜNCEL kuralları öğrenebilirsiniz. !dc");
        player.PrintToChat(@$"{Prefix}{CC.W}  * Discord gelerek GÜNCEL kuralları öğrenebilirsiniz. !dc");
        player.PrintToChat(@$"{Prefix}{CC.W}  * Discord gelerek GÜNCEL kuralları öğrenebilirsiniz. !dc");
        player.PrintToChat(@$"{Prefix}{CC.W}  * Discord gelerek GÜNCEL kuralları öğrenebilirsiniz. !dc");
    }

    #endregion Kurallar
}