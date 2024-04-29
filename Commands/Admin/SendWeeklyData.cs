using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("SendWeeklyData")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void SendWeeklyData(CCSPlayerController? player, CommandInfo info)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                SendWeeklyAllData(con);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}