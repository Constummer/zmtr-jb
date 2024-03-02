using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("configreload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void ConfigLoad(CCSPlayerController? player, CommandInfo info)
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
                                    ReadConfigFromPath(path, "JailbreakExtras.json");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void ReadConfigFromPath(string path, string configName)
    {
        var cfgPath = Path.Combine(path, configName);
        if (File.Exists(cfgPath))
        {
            var data = File.ReadAllText(cfgPath);
            if (string.IsNullOrWhiteSpace(data) == false)
            {
                var temp = JsonSerializer.Deserialize<JailbreakExtrasConfig>(data);
                if (temp != null)
                {
                    Server.PrintToConsole("Config reload");
                    Config = _Config = temp;
                }
            }
        }
    }
}