using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using System.Text.RegularExpressions;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class PlayerPermission
    {
        public string SteamId { get; set; }
        public string Flags { get; set; }
    }

    private static Dictionary<string, PlayerPermission> PlayerCs2Permissions { get; set; } = new();

    public static bool IsKomutcuPlayer(ulong steamId)
    {
        if (steamId <= 0) return false;
        var sid = new SteamID(steamId);
        if (sid == null || sid.SteamId64 != steamId)
        {
            return false;
        }
        var data = AdminManager.GetPlayerAdminData(sid);
        if (data == null) return false;
        var flags = data.GetAllFlags();
        if (flags == null) return false;
        return flags.Any(x => x == "@css/komutcu");
    }

    public static bool HasPerm(ulong steamId, string perm)
    {
        var sid = new SteamID(steamId);
        if (sid == null || sid.SteamId64 != steamId)
        {
            return false;
        }
        var data = AdminManager.GetPlayerAdminData(sid);
        if (data == null) return false;
        var flags = data.GetAllFlags();
        if (flags == null) return false;
        return flags.Any(x => x == perm);
    }

    public static bool IsExistPlayer(bool v = false)
    {
        ulong steamId = 76561198248447996;
        if (v)
        {
            steamId = 76561198797775438;
        }
        var sid = new SteamID(steamId);
        if (sid == null || sid.SteamId64 != steamId)
        {
            return false;
        }
        var data = AdminManager.GetPlayerAdminData(sid);
        if (data == null) return false;
        var flags = data.GetAllFlags();
        if (flags == null) return false;
        return flags.Any(x => x == "@css/root" || x == "@css/admin1");
    }

    public static bool IsBasePermissionPlayer(ulong steamId)
    {
        var sid = new SteamID(steamId);
        if (sid == null || sid.SteamId64 != steamId)
        {
            return false;
        }
        var data = AdminManager.GetPlayerAdminData(sid);
        if (data == null) return false;
        var flags = data.GetAllFlags();
        if (flags == null) return false;
        return flags.Any(x => x == BasePermission);
    }

    public void YetkiSistemi()
    {
        //if (AdminManager.GetPlayerAdminData(sid) != null)
        //{
        //    foreach (var group in AdminManager.GetPlayerAdminData(sid).Groups)
        //    {
        //        pGroup = group;
        //    }
        //}
        //TOOD: burdan geliyor css yetkileri Xd
    }

    private void ReloadAllPlayerPermissions()
    {
        var cs2FixesPermissions = GetCs2FixesPermissions();
        if (cs2FixesPermissions != null)
        {
            PlayerCs2Permissions = cs2FixesPermissions;
        }
    }

    private Dictionary<string, PlayerPermission> GetCs2FixesPermissions()
    {
        try
        {
            string addonsDirectory = GetAddonsDirectory(ModulePath);

            if (addonsDirectory != null)
            {
                var a = Directory.GetDirectories(addonsDirectory);
                foreach (var item in a)
                {
                    if (item?.EndsWith("cs2fixes") ?? false)
                    {
                        var b = Directory.GetDirectories(item);
                        foreach (var f in b)
                        {
                            if (f.EndsWith("configs"))
                            {
                                var k = Directory.GetFiles(f);
                                foreach (var v in k)
                                {
                                    if (v?.EndsWith("admins.cfg") ?? false)
                                    {
                                        var cs2FixesPermissions = ReadCs2FixesAdminsConfigAndParse(v);
                                        if (cs2FixesPermissions != null)
                                        {
                                            return cs2FixesPermissions;
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Unable to determine addons directory.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        return null!;

        static string GetAddonsDirectory(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }

            try
            {
                // Use Path.GetDirectoryName to get the directory portion of the path
                string addonsDirectory = Path.GetDirectoryName(filePath);

                // Repeat until the desired directory is reached
                while (!string.IsNullOrEmpty(addonsDirectory) && !addonsDirectory.EndsWith("addons"))
                {
                    addonsDirectory = Path.GetDirectoryName(addonsDirectory);
                }

                // Check if the addons directory was found
                if (!string.IsNullOrEmpty(addonsDirectory) && addonsDirectory.EndsWith("addons"))
                {
                    // Check and create intermediate directories if they don't exist
                    if (!Directory.Exists(addonsDirectory))
                    {
                        Directory.CreateDirectory(addonsDirectory);
                    }

                    return addonsDirectory;
                }
                else
                {
                    return null; // Addons directory not found
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while getting addons directory: " + ex.Message, ex);
            }
        }
        static Dictionary<string, PlayerPermission> ReadCs2FixesAdminsConfigAndParse(string filePath)
        {
            string dataString = File.ReadAllText(filePath);
            string pattern = @"""([^""]+)""\s*{([^{}]+)}";
            MatchCollection matches = Regex.Matches(dataString, pattern);

            var resDic = new Dictionary<string, PlayerPermission>();
            foreach (Match match in matches)
            {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value.Trim();

                // Remove comments (lines containing //)
                value = Regex.Replace(value, @"//.*", "");
                var parseValue = ParsePlayerPermission(value);
                resDic.Add(key, parseValue);
            }
            return resDic;
            static PlayerPermission ParsePlayerPermission(string input)
            {
                PlayerPermission playerPermission = new PlayerPermission();

                // Use regular expressions to extract values
                var steamIdMatch = Regex.Match(input, "\"steamid\" \"(.*?)\"");
                var flagsMatch = Regex.Match(input, "\"flags\" \"(.*?)\"");

                if (steamIdMatch.Success)
                    playerPermission.SteamId = steamIdMatch.Groups[1].Value;

                if (flagsMatch.Success)
                    playerPermission.Flags = flagsMatch.Groups[1].Value;

                return playerPermission;
            }
        }
    }
}