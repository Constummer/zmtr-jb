using CounterStrikeSharp.API;
using JailbreakExtras.Lib.Configs;
using System.Drawing;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static string _customConfigPath = "";

    public void ConfigReadPath()
    {
        var path = Server.GameDirectory;
        if (!Directory.Exists(path))
        {
            return;
        }
        path = Path.Combine(path, "csgo");
        if (!Directory.Exists(path))
        {
            return;
        }
        path = Path.Combine(path, "addons");
        if (!Directory.Exists(path))
        {
            return;
        }
        path = Path.Combine(path, "counterstrikesharp");
        if (!Directory.Exists(path))
        {
            return;
        }
        path = Path.Combine(path, "configs");
        if (!Directory.Exists(path))
        {
            return;
        }
        path = Path.Combine(path, "plugins");
        if (!Directory.Exists(path))
        {
            return;
        }
        path = Path.Combine(path, ModuleName);
        if (!Directory.Exists(path))
        {
            return;
        }
        _customConfigPath = path;
    }

    private void ReadInitConfig()
    {
        try
        {
            var temp1 = ReadCustomConfigFromPath<AdditionalConfig>("AdditionalConfig.json");
            if (temp1 != null)
            {
                _Config.Additional = Config.Additional = temp1;
                Cits = new(_Config.Additional.CitMaxCount);
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp2 = ReadCustomConfigFromPath<BlockedRadioConfig>("BlockedRadioConfig.json");
            if (temp2 != null)
            {
                _Config.BlockedRadio = Config.BlockedRadio = temp2;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp3 = ReadCustomConfigFromPath<BurryConfig>("BurryConfig.json");
            if (temp3 != null)
            {
                _Config.Burry = Config.Burry = temp3;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp4 = ReadCustomConfigFromPath<CreditConfig>("CreditConfig.json");
            if (temp4 != null)
            {
                _Config.Credit = Config.Credit = temp4;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp5 = ReadCustomConfigFromPath<DatabaseConfig>("DatabaseConfig.json");
            if (temp5 != null)
            {
                _Config.Database = Config.Database = temp5;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp6 = ReadCustomConfigFromPath<DontBlockOnGaggedConfig>("DontBlockOnGaggedConfig.json");
            if (temp6 != null)
            {
                _Config.DontBlockOnGagged = Config.DontBlockOnGagged = temp6;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp7 = ReadCustomConfigFromPath<LaserConfig>("LaserConfig.json");
            if (temp7 != null)
            {
                _Config.Laser = Config.Laser = temp7;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp10 = ReadCustomConfigFromPath<MarketConfig>("MarketConfig.json");
            if (temp10 != null)
            {
                _Config.Market = Config.Market = temp10;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp11 = ReadCustomConfigFromPath<ModelConfig>("ModelConfig.json");
            if (temp11 != null)
            {
                _Config.Model = Config.Model = temp11;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp12 = ReadCustomConfigFromPath<ParticleConfig>("ParticleConfig.json");
            if (temp12 != null)
            {
                _Config.Particle = Config.Particle = temp12;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp121 = ReadCustomConfigFromPath<ParachuteConfig>("ParachuteConfig.json");
            if (temp121 != null)
            {
                _Config.Parachute = Config.Parachute = temp121;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp13 = ReadCustomConfigFromPath<SoundsConfig>("SoundsConfig.json");
            if (temp13 != null)
            {
                _Config.Sounds = Config.Sounds = temp13;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp14 = ReadCustomConfigFromPath<SteamGroupConfig>("SteamGroupConfig.json");
            if (temp14 != null)
            {
                _Config.SteamGroup = Config.SteamGroup = temp14;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp16 = ReadCustomConfigFromPath<UnrestrictedFovConfig>("UnrestrictedFovConfig.json");
            if (temp16 != null)
            {
                _Config.UnrestrictedFov = Config.UnrestrictedFov = temp16;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp9 = ReadCustomConfigFromPath<MapConfig>("MapConfig.json");
            if (temp9 != null)
            {
                _Config.Map = Config.Map = temp9;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            var temp17 = PubgConfigReadFromFolder();
            if (temp17 != null)
            {
                _Config.Pubg = Config.Pubg = temp17;
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            ReadMapConfigs();
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        try
        {
            SetLevelPermissionDictionary();
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
        _Config.Burry.BuryColor
                    = Config.Burry.BuryColor
                    = Color.FromArgb(Config.Burry.BurryColorR, Config.Burry.BurryColorG, Config.Burry.BurryColorB);
    }

    private void ReadMapConfigs()
    {
        try
        {
            var files = Directory.GetFiles(_customConfigPath)
                                 .Where(x => x.Contains("map_")
                                        && x.EndsWith("map_jb_example.json") == false)
                                 .Select(x => new
                                 {
                                     All = x,
                                     Extensionless = Path.GetFileNameWithoutExtension(x),
                                 })
                                 .ToArray();
            if (files != null)
            {
                if (files.Length == 0)
                {
                    ReadCustomConfigFromPath<MapConfigDetailed>("map_jb_example.json");
                }
                else
                {
                    _Config.Map.MapConfigDatums = new();
                    Config.Map.MapConfigDatums = new();
                    foreach (var item in files)
                    {
                        var temp = ReadCustomConfigFromPath<MapConfigDetailed>(item.All);
                        if (temp != null)
                        {
                            Server.PrintToChatAll(item.Extensionless.Substring(4));
                            _Config.Map.MapConfigDatums.TryAdd(item.Extensionless.Substring(4), temp);
                            Config.Map.MapConfigDatums.TryAdd(item.Extensionless.Substring(4), temp);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Server.PrintToChatAll(e.Message);
        }
    }

    private T? ReadCustomConfigFromPath<T>(string configName)
    {
        var cfgPath = Path.Combine(_customConfigPath, configName);
        if (File.Exists(cfgPath))
        {
            var data = File.ReadAllText(cfgPath);
            if (string.IsNullOrWhiteSpace(data) == false)
            {
                var temp = JsonSerializer.Deserialize<T>(data);
                Server.PrintToConsole("Config reloaded");
                return temp;
            }
            else
            {
                CreateCustomConfigOnPath<T>(cfgPath);
            }
        }
        else
        {
            CreateCustomConfigOnPath<T>(cfgPath);
        }
        return (T)default;
    }

    private static void CreateCustomConfigOnPath<T>(string cfgPath)
    {
        var serialized = JsonSerializer.Serialize(Activator.CreateInstance(typeof(T)),
        new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        File.WriteAllText(cfgPath, serialized);
    }

    private static PubgConfig PubgConfigReadFromFolder()
    {
        var res = new PubgConfig()
        {
            PubgCoords = new()
        };
        var cfgsPath = Path.Combine(_customConfigPath, "Pubg");
        if (Directory.Exists(cfgsPath))
        {
            var files = Directory.GetFiles(cfgsPath);
            foreach (var item in files)
            {
                var file = File.ReadAllText(item);
                if (string.IsNullOrWhiteSpace(file) == false)
                {
                    try
                    {
                        var temp = JsonSerializer.Deserialize<PubgConfigConverted>(file);
                        if (temp != null
                            && string.IsNullOrWhiteSpace(temp.MapName) == false
                            && temp.Data != null
                            && temp.Data.Count > 0)
                        {
                            if (res.PubgCoords.ContainsKey(temp.MapName))
                            {
                                res.PubgCoords[temp.MapName] = temp.Data;
                            }
                            else
                            {
                                res.PubgCoords.Add(temp.MapName, temp.Data);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Server.PrintToChatAll(e.Message);
                    }
                }
            }
        }
        return res;
    }
}