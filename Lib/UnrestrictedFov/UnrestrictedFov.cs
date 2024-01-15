using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void FovAction(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        if (player == null) // if player is server then return
        {
            info.ReplyToCommand("[FOV] Cannot use command from RCON");
            return;
        }
        if (Config.UnrestrictedFov.Enabled == false)
        {
            player.PrintToChat($"{Prefix} {CC.DR}FOV devre dışı!");
            return;
        }
        var fov = info.ArgByIndex(1);
        if (fov == "")
        {
            player.DesiredFOV = 60;
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