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
    public class ToplantiTP
    {
        public List<ToplantiTPLer> Data { get; set; } = new();
    }

    public class ToplantiTPLer
    {
        [JsonPropertyName("steamId")]
        public ulong SteamId { get; set; }

        [JsonPropertyName("xp")]
        public int Xp { get; set; }
    }

    [ConsoleCommand("ToplantiTPVermeler")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void ToplantiTPVermeler3(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        Server.NextWorldUpdate(() =>
        {
            ToplantiTpVermelerAction();
        });
    }

    private void ToplantiTpVermelerAction()
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
                                    ReadTPlerFromPath(path, "ToplantiTP.json");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void ReadTPlerFromPath(string path, string configName)
    {
        try
        {
            var cfgPath = Path.Combine(path, configName);
            if (File.Exists(cfgPath))
            {
                var data = File.ReadAllText(cfgPath);
                if (string.IsNullOrWhiteSpace(data) == false)
                {
                    var temp = JsonSerializer.Deserialize<ToplantiTP>(data);
                    if (temp != null && temp.Data != null)
                    {
                        Server.PrintToConsole($"Dosya okundu = {temp.Data.Count} adet entry var");
                        var dblist = new List<ToplantiTPLer>();
                        foreach (var item in temp.Data)
                        {
                            var p = GetPlayers().Where(x => x.SteamID == item.SteamId).FirstOrDefault();
                            if (p == null)
                            {
                                dblist.Add(item);
                                continue;
                            }
                            if (PlayerLevels.TryGetValue(item.SteamId, out var level) == false)
                            {
                                dblist.Add(item);
                                continue;
                            }
                            level.Xp += item.Xp;
                            PlayerLevels[item.SteamId] = level;
                        }
                        AddXpBulk(dblist);
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
            ConsMsg(e.Message);
        }
    }

    private void AddXpBulk(List<ToplantiTPLer> dblist)
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
                    sql += @$"UPDATE `PlayerLevel` SET `Xp` = `Xp` + @Xp_{i} WHERE `SteamId` = @SteamId_{i};";
                    parameters.Add(new MySqlParameter($"@SteamId_{i}", item.SteamId));
                    parameters.Add(new MySqlParameter($"@Xp_{i}", item.Xp));

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
            ConsMsg(e.Message);
        }
    }
}