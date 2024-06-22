using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static string ContentRootPath = "";

    internal void CreateDataFolder()
    {
        var contentRootPath = ModulePath.Replace(
            ModulePath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }).Last(), "datas");

        if (!Directory.Exists(contentRootPath))
        {
            Directory.CreateDirectory(contentRootPath);
        }
        ContentRootPath = contentRootPath;
    }

    private static List<ulong> WelcomeMsgDatas = new();

    private void WelcomeMsgSpam(CCSPlayerController? player)
    {
        if (string.IsNullOrWhiteSpace(Config.Additional.WelcomeImgUrl))
        {
            return;
        }

        if (WelcomeMsgDatas.Any(x => x == player?.SteamID) == false)
        {
            WelcomeMsgDatas.Add(player?.SteamID ?? 0);
            var t = AddTimer(0.1f, () =>
            {
                if (is_valid(player))
                {
                    player.PrintToCenterHtml($"<img src='{Config.Additional.WelcomeImgUrl}'></img>");
                }
            }, Full);
            AddTimer(10f, () =>
            {
                t.Kill();
            }, SOM);
        }
    }
}