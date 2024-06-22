using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static void ConsMsg(string message, bool dc = true)
    {
        try
        {
            GetPlayers().Where(x => GrabAllowedSteamIds.Contains(x.SteamID))
                .ToList()
                .ForEach(x =>
                {
                    x.PrintToChat(message);
                    x.PrintToConsole(message);
                });
        }
        catch
        {
        }
        try
        {
            if (dc)
            {
                Task.Run(() =>
                {
                    Server.NextFrame(() =>
                    {
                        try
                        {
                            DiscordPost(_Config.Additional.PluginExceptionWebHook, message);
                        }
                        catch (Exception e)
                        {
                        }
                    });

                    return Task.CompletedTask;
                });
            }
        }
        catch
        {
        }
    }
}