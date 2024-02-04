using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, int> FovActivePlayers = new();

    private void FovKapaAction(CCSPlayerController? player, bool extraMsg = false)
    {
        GetPlayers()
            .Where(x => x.PawnIsAlive == true
                       && FovActivePlayers.ContainsKey(x.SteamID))
            .ToList()
            .ForEach(x =>
            {
                if (player == null)
                {
                    x.PrintToChat($"{Prefix} {CC.W} FOV'un {(extraMsg ? "oyun için" : "")} kapandı.");
                }
                else
                {
                    x.PrintToChat($"{AdliAdmin(player.PlayerName)} {CC.W} FOV'unu {(extraMsg ? "oyun için" : "")} kapadı.");
                }
                FovAction(x, null, true);
            });
        Config.UnrestrictedFov.Enabled = false;
    }

    private void FovReopenAction(bool extraMsg = false, CsTeam? team = CsTeam.Terrorist)
    {
        Config.UnrestrictedFov.Enabled = true;

        GetPlayers(team)
            .ToList()
            .ForEach(x =>
            {
                if (FovActivePlayers.TryGetValue(x.SteamID, out int fov))
                {
                    x.PrintToChat($"{Prefix} {CC.W}{(extraMsg ? "Oyun için" : "")} kapalı FOV'un tekrar açıldı. !fov yazarak değiştirebilirsin.");
                    FovAction(x, fov.ToString());
                }
                else
                {
                    x.PrintToChat($"{Prefix} {CC.W}{(extraMsg ? "Oyun için" : "")} kapalı FOV'un sıfırlandı. !fov yazarak değiştirebilirsin.");
                    FovAction(x, null);
                }
            });
    }

    private void FovAction(CCSPlayerController? player, string fov, bool force = false, bool setForce = false, bool addDic = true)

    {
        if (ValidateCallerPlayer(player, false) == false) return;

        if (setForce == false && Config.UnrestrictedFov.Enabled == false)
        {
            player.PrintToChat($"{Prefix} {CC.DR}FOV devre dışı!");
            return;
        }

        if (force)
        {
            player.DesiredFOV = 90;
            SetStateChanged(player, "CBasePlayerController", "m_iDesiredFOV");
            player.PrintToChat($"{Prefix} {CC.W}FOV ayarın sıfırlandı..");
            return;
        }

        if (fov == "" || fov == null)
        {
            player.DesiredFOV = 90;
            if (addDic)
            {
                if (FovActivePlayers.ContainsKey(player.SteamID) == false)
                {
                    FovActivePlayers.Add(player.SteamID, 90);
                }
                else
                {
                    FovActivePlayers[player.SteamID] = 90;
                }
            }

            SetStateChanged(player, "CBasePlayerController", "m_iDesiredFOV");

            player.PrintToChat($"{Prefix} {CC.W}FOV ayarın sıfırlandı..");

            return;
        }

        if (!IsInt(fov))
        {
            player.PrintToChat($"{Prefix} {CC.W}FOV bulunamadı. Sayıyı doğru gir {CC.DR}{Config.UnrestrictedFov.FOVMin}{CC.W} - {CC.DR}{Config.UnrestrictedFov.FOVMax}");
            return;
        }
        if (Convert.ToInt32(fov) < Config.UnrestrictedFov.FOVMin)
        {
            player.PrintToChat($"{Prefix} {CC.W}Minimum FOV {CC.DR}{Config.UnrestrictedFov.FOVMin}{CC.W} olarak ayarlanabilir");
            return;
        }
        if (Convert.ToInt32(fov) > Config.UnrestrictedFov.FOVMax)
        {
            player.PrintToChat($"{Prefix} {CC.W}Maximum FOV {CC.DR}{Config.UnrestrictedFov.FOVMax}{CC.W} olarak ayarlanabilir");
            return;
        }
        if (addDic)
        {
            if (FovActivePlayers.ContainsKey(player.SteamID) == false)
            {
                FovActivePlayers.Add(player.SteamID, Convert.ToInt32(fov));
            }
            else
            {
                FovActivePlayers[player.SteamID] = Convert.ToInt32(fov);
            }
        }

        player.DesiredFOV = Convert.ToUInt32(fov);
        SetStateChanged(player, "CBasePlayerController", "m_iDesiredFOV");
        player.PrintToChat($"{Prefix} {CC.W}FOV ayarın {CC.L}{fov} {CC.W}olarak ayarlandı.");
    }

    private bool IsInt(string sVal)
    {
        foreach (char c in sVal)
        {
            int iN = (int)c;
            if ((iN > 57) || (iN < 48))
                return false;
        }
        return true;
    }

    // Thanks "yarukon (59 61 72 75 6B 6F 6E)" Discord member
    private static MemoryFunctionVoid<nint, nint, int, short, short> _StateChanged = new(@"\x55\x48\x89\xE5\x41\x57\x41\x56\x41\x55\x41\x54\x53\x89\xD3");

    private static MemoryFunctionVoid<nint, int, long> _NetworkStateChanged = new(@"\x4C\x8B\x07\x4D\x85\xC0\x74\x2A\x49\x8B\x40\x10");

    public int FindSchemaChain(string classname) => Schema.GetSchemaOffset(classname, "__m_pChainEntity");

    public void SetStateChanged(CCSPlayerController entity, string classname, string fieldname, int extraOffset = 0)
    {
        if (ValidateCallerPlayer(entity, false) == false) return;

        int offset = Schema.GetSchemaOffset(classname, fieldname);
        int chainOffset = FindSchemaChain(classname);
        if (ValidateCallerPlayer(entity, false) == false) return;

        if (chainOffset != 0)
        {
            _NetworkStateChanged.Invoke(entity.Handle + chainOffset, offset, 0xFFFFFFFF);
            return; // No need to execute the rest of the things
        }
        if (ValidateCallerPlayer(entity, false) == false) return;

        _StateChanged.Invoke(entity.NetworkTransmitComponent.Handle, entity.Handle, offset + extraOffset, -1, -1);
        if (ValidateCallerPlayer(entity, false) == false) return;

        entity.LastNetworkChange = Server.CurrentTime;
        entity.IsSteadyState.Clear();
    }
}