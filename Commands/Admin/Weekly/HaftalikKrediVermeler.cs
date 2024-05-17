using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class HaftalikKredi
    {
        public List<HaftalikKrediLer> Data { get; set; } = new();
    }

    public class HaftalikKrediLer
    {
        [JsonPropertyName("steamId")]
        public ulong SteamId { get; set; }

        [JsonPropertyName("c")]
        public int C { get; set; }
    }

    [ConsoleCommand("HaftalikKrediVermeler")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void HaftalikKrediVermeler3(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        Server.NextWorldUpdate(() =>
        {
            HaftalikKrediVermelerAction();
        });
    }

    private void HaftalikKrediVermelerAction()
    {
        ///home/container/game
        var path = Server.GameDirectory;
        if (Directory.Exists(path))
        {
            path = Path.Combine(path, "csgo");
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, "addons");
                if (Directory.Exists(path))
                {
                    path = Path.Combine(path, "counterstrikesharp");
                    if (Directory.Exists(path))
                    {
                        path = Path.Combine(path, "configs");
                        if (Directory.Exists(path))
                        {
                            path = Path.Combine(path, "plugins");
                            if (Directory.Exists(path))
                            {
                                path = Path.Combine(path, ModuleName);
                                if (Directory.Exists(path))
                                {
                                    //GTG
                                    ReadHaftalikKredilerFromPath(path, "HaftalikKredi.json");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void ReadHaftalikKredilerFromPath(string path, string configName)
    {
        try
        {
            var cfgPath = Path.Combine(path, configName);
            if (File.Exists(cfgPath))
            {
                var data = File.ReadAllText(cfgPath);
                if (string.IsNullOrWhiteSpace(data) == false)
                {
                    var temp = JsonSerializer.Deserialize<HaftalikKredi>(data);
                    if (temp != null && temp.Data != null)
                    {
                        Server.PrintToConsole($"Dosya okundu = {temp.Data.Count} adet entry var");
                        var dblist = new List<HaftalikKrediLer>();
                        foreach (var item in temp.Data)
                        {
                            var p = GetPlayers().Where(x => x.SteamID == item.SteamId).FirstOrDefault();
                            if (p == null)
                            {
                                dblist.Add(item);
                                continue;
                            }
                            if (PlayerMarketModels.TryGetValue(item.SteamId, out var m))
                            {
                                m.Credit += item.C;
                                PlayerMarketModels[item.SteamId] = m;
                            }
                            else
                            {
                                dblist.Add(item);
                                continue;
                            }
                        }
                        AddCreditBulk(dblist);
                    }
                    else
                    {
                        Server.PrintToConsole($"Dosya okunamadi");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Server.PrintToConsole(e.Message);
            Console.WriteLine(e);
        }
    }

    private void AddCreditBulk(List<HaftalikKrediLer> dblist)
    {
        if (dblist == null || dblist.Count == 0)
        {
            return;
        }

        try
        {
            using (var con = Connection())
            {
                if (con == null)
                {
                    return;
                }

                List<MySqlParameter> parameters = new List<MySqlParameter>();

                var sql = "";
                var i = 0;

                foreach (var item in dblist)
                {
                    sql += @$"UPDATE `PlayerMarketModel` SET `Credit` = `Credit` + @Credit_{i} WHERE `SteamId` = @SteamId_{i};";
                    parameters.Add(new MySqlParameter($"@SteamId_{i}", item.SteamId));
                    parameters.Add(new MySqlParameter($"@Credit_{i}", item.C));

                    i++;
                }
                if (string.IsNullOrWhiteSpace(sql))
                {
                    return;
                }

                var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "hata");
        }
    }
}