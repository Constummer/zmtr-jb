using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static void HideV2(bool hideEnable = true)
    {
        if (hideEnable)
        {
            Server.ExecuteCommand($"c_hidea @t 1;c_rcon cs2f_hide_enable 1;");
        }
        else
        {
            Server.ExecuteCommand($"c_rcon cs2f_hide_enable 0; c_hidea @t 0;");
        }
    }
}