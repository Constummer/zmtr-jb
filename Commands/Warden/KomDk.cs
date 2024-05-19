using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region KomDk

    public DateTime? KomStartTime { get; set; } = null;

    private void KomKalanAction(CCSPlayerController? player)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        if (KomStartTime == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}Mevcutta kom yok");
            return;
        }
        if (ValidateCallerPlayer(player, false) == false) return;

        var substract = (DateTime.UtcNow - KomStartTime.Value).TotalSeconds;
        TimeSpan remainingTime = TimeSpan.FromMinutes(45) - TimeSpan.FromSeconds(substract);
        if (remainingTime.TotalSeconds > 0)
        {
            var dk = $"{CC.B}{remainingTime.ToString("mm")}{CC.W}";
            var sn = $"{CC.B}{remainingTime.ToString("ss")}{CC.W}";
            player.PrintToChat($"{Prefix} {CC.W} Komdk'ya daha {dk} dakika {sn} saniye var.");
        }
        else
        {
            player.PrintToChat($"{Prefix} {CC.W} Komdk atılabilir.");
        }
    }

    [ConsoleCommand("komdkyap")]
    [ConsoleCommand("komdkbaslat")]
    [ConsoleCommand("komdkbasla")]
    [ConsoleCommand("komdkac")]
    public void KomDkBaslat(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false) return;
        if (KomStartTime == null)
        {
            player.PrintToChat($"{Prefix} {CC.W}Mevcutta kom yok");
            return;
        }
        var substract = (DateTime.UtcNow - KomStartTime.Value).TotalSeconds;
        TimeSpan remainingTime = TimeSpan.FromMinutes(45) - TimeSpan.FromSeconds(substract);
        LogManagerCommand(player.SteamID, info.GetCommandString);
        if (remainingTime.TotalSeconds > 0)
        {
            var dk = $"{CC.B}{remainingTime.ToString("mm")}{CC.W}";
            var sn = $"{CC.B}{remainingTime.ToString("ss")}{CC.W}";
            player.PrintToChat($"{Prefix} {CC.W} Komdk'ya daha {dk} dakika {sn} saniye var.");
        }
        else
        {
            KomStartTime = DateTime.UtcNow;
            VoteAction(player, "Komutçu Değiş Kal");
        }
    }

    //command
    private static List<string> komKalanWords = new()
    {
        "komkalan",
        "komkaln",
        "komkln",
        "komklan",
        "komdk",
        "komutcudk",
        "k0mdk",
        "k00mdk",
        "koomdk",
        "komdeis",
        "komdegis",
        "kom",
        "komutdk",
    };

    //say
    private static List<string> KomInterceptWords = new();

    /// <summary>
    /// CREATE TABLE IF NOT EXISTS `KomKalanInterceptor` (
    ///                     `SelectedModelId` TEXT DEFAULT NULL
    ///                 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    /// </summary>
    /// <param name="con"></param>

    private void GetAllKomKalanInterceptorDatas(MySqlConnection con)
    {
        try
        {
            if (con == null)
            {
                return;
            }
            MySqlCommand? cmd = new MySqlCommand(@$"SELECT `SelectedModelId` FROM `KomKalanInterceptor`;", con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var text = reader.IsDBNull(0) ? "" : reader.GetString(0);

                    if (string.IsNullOrWhiteSpace(text) == false &&
                        KomInterceptWords.Contains(text.ToLower()) == false)
                    {
                        KomInterceptWords.Add(text.ToLower());
                    }
                }
            }
        }
        catch (Exception e)
        {
           ConsMsg(e.Message);
        }
    }

    private bool KomdkIntercepter(CCSPlayerController player, bool isSayTeam, string arg)
    {
        if (isSayTeam)
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(arg))
        {
            return false;
        }
        if (KomInterceptWords.Any(x => arg.ToLower().Contains(x)))
        {
            return true;
        }
        return false;
    }

    private bool KomKalanIntercepter(CCSPlayerController player, string arg)
    {
        var komkalan = arg.Substring(1);
        if (string.IsNullOrWhiteSpace(arg))
        {
            return false;
        }
        if (komKalanWords.Select(x => x.ToLower()).Contains(komkalan.ToLower()))
        {
            KomKalanAction(player);
            return true;
        }
        return false;
    }

    #endregion KomDk
}