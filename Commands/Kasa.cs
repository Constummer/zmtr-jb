using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using MySqlConnector;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class KasaData
    {
        public KasaData(string text, int creditCost, KasaOptions option, List<string> menuDisabledTexts, List<KasaResultData> results)
        {
            Text = text;
            CreditCost = creditCost;
            Option = option;
            MenuDisabledTexts = menuDisabledTexts;
            Results = results;
        }

        public string Text { get; set; }
        public int CreditCost { get; set; }
        public KasaOptions Option { get; set; }
        public List<string> MenuDisabledTexts { get; set; }
        public List<KasaResultData> Results { get; set; }
    }

    public enum KasaResultOptions
    {
        None = 0,
        Credit,
        Admin,
        Tp
    }

    public class KasaResultData
    {
        public KasaResultData(string text, int possibility, KasaResultOptions option, int? reward = null)
        {
            Text = text;
            Possibility = possibility;
            Option = option;
            Reward = reward;
        }

        public string Text { get; set; }
        public int Possibility { get; set; }
        public int? Reward { get; set; }
        public KasaResultOptions Option { get; set; }
    }

    public enum KasaOptions
    {
        None = 0,
        Elmas,
        Zumrut
    }

    private List<KasaData> KasaDatas { get; set; } = new()
    {
        {new ($"Elmas Kasa | 5000 Kredi",5000, KasaOptions.Elmas,
            new()
            {
                "Boş %25 | Kredi %25 (10K)",
                "Adminlik %25 | Tp %25 (2K)",
            },
            new()
            {
                new ("Boş", 25, KasaResultOptions.None),
                new ("Kredi", 25, KasaResultOptions.Credit, 10000),
                new ("Adminlik", 25, KasaResultOptions.Admin),
                new ("Tp", 25, KasaResultOptions.Tp, 2000),
            })
        },
        {new ($"Zümrüt Kasa | 10000 Kredi",10000, KasaOptions.Zumrut,
             new()
            {
                "Boş %10 | Kredi %30 (15K)",
                "Adminlik %30 | Tp %30 (3K)",
            },
            new()
            {
                new ("Boş", 10, KasaResultOptions.None),
                new ("Kredi", 30, KasaResultOptions.Credit,15000),
                new ("Adminlik", 30, KasaResultOptions.Admin),
                new ("Tp", 30, KasaResultOptions.Tp, 3000),
            })
         },
    };

    [ConsoleCommand("kasa")]
    public void Kasa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player?.SteamID == null || player!.SteamID == 0)
        {
            return;
        }
        var data = GetPlayerMarketModel(player.SteamID);
        if (data.Model == null) return;
        var menu = new CenterHtmlMenu($"KASA - Kredin {data.Model.Credit}", this);
        foreach (var item in KasaDatas.Where(x => x.Results?.Any() ?? false))
        {
            if (ValidateCallerPlayer(player, false) == false) return;
            menu.AddMenuOption(item.Text, (p, info) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (player?.SteamID == null || player!.SteamID == 0) return;

                if (data.Model == null
                    || data.Model.Credit < item.CreditCost
                    || data.Model.Credit - item.CreditCost < 0)
                {
                    player.PrintToChat($"{Prefix} {CC.W}Yetersiz Bakiye!");
                    return;
                }
                ConfirmMenuHtml(player, data.Model.Credit, item.Text, () =>
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    player.PrintToChat("kasa kapali guzel kardesim");
                    return;
                    data.Model.Credit -= item.CreditCost;
                    PlayerMarketModels[player.SteamID] = data.Model;

                    double totalProbability = item.Results.Sum(item => item.Possibility) / 100.0;
                    double cumulativeProbability = 0;
                    double[] cumulativeProbabilities = new double[item.Results.Count];

                    for (int i = 0; i < item.Results.Count; i++)
                    {
                        cumulativeProbability += (double)item.Results[i].Possibility / (totalProbability * 100);
                        cumulativeProbabilities[i] = cumulativeProbability;
                    }

                    double randomNumber = _random.NextDouble();

                    // Find the index based on the generated random number and cumulative probabilities
                    int index = Array.FindIndex(cumulativeProbabilities, p => p >= randomNumber);

                    // Get the randomly selected result
                    KasaResultData randomResult = item.Results[index];
                    if (ValidateCallerPlayer(player, false) == false) return;

                    switch (randomResult.Option)
                    {
                        case KasaResultOptions.None:
                            {
                                player.PrintToChat($"{Prefix} {CC.B}{item.CreditCost} {CC.W}Kredi Karşılığında {CC.Go}KASADAN {CC.B}MALESEF {CC.W} hiç bir şey kazanamadın malesef.");
                                LogKasaWin(player.SteamID, (int)item.Option, (int)randomResult.Option, true);
                            }
                            break;

                        case KasaResultOptions.Credit:
                            {
                                data.Model.Credit += randomResult.Reward.Value;
                                PlayerMarketModels[player.SteamID] = data.Model;
                                player.PrintToChat($"{Prefix} {CC.B}{item.CreditCost} {CC.W}Kredi Karşılığında {CC.Go}KASADAN {CC.B}{randomResult.Reward.Value} {CC.W}KREDI KAZANDIN!");
                                player.PrintToChat($"{Prefix} {CC.W}Mevcut Kredin = {CC.B}{data.Model.Credit}{CC.R}");
                                LogKasaWin(player.SteamID, (int)item.Option, (int)randomResult.Option, true);
                            }
                            break;

                        case KasaResultOptions.Admin:
                            {
                                player.PrintToChat($"{Prefix} {CC.B}{item.CreditCost} {CC.W}Kredi Karşılığında {CC.Go}KASADAN {CC.B}ADMİNLİK {CC.W} KAZANDIN.");
                                player.PrintToChat($"{Prefix} {CC.W} ADMINLIK ILE ILGILI DISCORD'DAN TICKET ACIP BELIRTEBILIRSIN.");
                                LogKasaWin(player.SteamID, (int)item.Option, (int)randomResult.Option, true);
                            }
                            break;

                        case KasaResultOptions.Tp:
                            {
                                if (PlayerLevels.TryGetValue(player.SteamID, out var level))
                                {
                                    level.Xp += randomResult.Reward.Value;
                                    PlayerLevels[player.SteamID] = level;
                                    player.PrintToChat($"{Prefix} {CC.B}{item.CreditCost} {CC.W}Kredi Karşılığında {CC.Go}KASADAN {CC.B}{randomResult.Reward.Value} {CC.W}TP KAZANDIN!");
                                    player.PrintToChat($"{Prefix} {CC.W}Mevcut Kredin = {CC.B}{data.Model.Credit}{CC.R} |{CC.W} Mevcut TP ={CC.B} {level.Xp}");
                                    LogKasaWin(player.SteamID, (int)item.Option, (int)randomResult.Option, true);
                                }
                                else
                                {
                                    data.Model.Credit += item.CreditCost;
                                    PlayerMarketModels[player.SteamID] = data.Model;

                                    player.PrintToChat($"{Prefix} {CC.W}Seviyen yok, seviye alabilmek için  {CC.DR}!slotol {CC.W},{CC.DR} !seviyeol {CC.W}yazabilirsin!");
                                    LogKasaWin(player.SteamID, (int)item.Option, (int)randomResult.Option, false);
                                    return;
                                }
                            }
                            break;

                        default:
                            break;
                    }
                },
                item.MenuDisabledTexts);
            });
            foreach (var disabledText in item.MenuDisabledTexts)
            {
                menu.AddMenuOption(disabledText, null, true);
            }
        }
        MenuManager.OpenCenterHtmlMenu(this, player, menu);
    }

    /*
      CREATE TABLE IF NOT EXISTS `PlayerKasa` (
                          `Id` bigint(20) PRIMARY KEY AUTO_INCREMENT,
                          `StartTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                          `SteamId` bigint(20) DEFAULT NULL,
                          `Opened` mediumint(9) DEFAULT 0,
                          `Won` mediumint(9) DEFAULT 0,
                          `GotTheReward` bit DEFAULT 0
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    */

    private void LogKasaWin(ulong steamId, int opened, int won, bool gotTheReward)
    {
        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }
                var cmd = new MySqlCommand(@$"INSERT INTO `PlayerKasa`
                                     (SteamId,Opened,Won,GotTheReward)
                                     VALUES (@SteamId,@Opened,@Won,@GotTheReward);", con);

                cmd.Parameters.AddWithValue("@SteamId", steamId);
                cmd.Parameters.AddWithValue("@Opened", opened);
                cmd.Parameters.AddWithValue("@Won", won);
                cmd.Parameters.AddWithValue("@GotTheReward", gotTheReward);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            ConsMsg(e.Message);
        }
    }
}