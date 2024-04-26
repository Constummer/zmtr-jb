using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private readonly WIN_LINUX<int> OnCollisionRulesChangedOffset = new WIN_LINUX<int>(173, 172);

    private HookResult NoBlockOnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        if (Config.Additional.NoBlockActive == false) return HookResult.Continue;
        var player = @event?.Userid;
        if (ValidateCallerPlayer(player, false) == false) return HookResult.Continue;

        if (player.Connected != PlayerConnectedState.PlayerConnected)
        {
            return HookResult.Continue;
        }

        if (!player.PlayerPawn.IsValid)
        {
            return HookResult.Continue;
        }

        CHandle<CCSPlayerPawn> pawn = player.PlayerPawn;
        if (ValidateCallerPlayer(player, false) == false) return HookResult.Continue;

        Server.NextFrame(() => PlayerSpawnNextFrame(player, pawn));
        if (ValidateCallerPlayer(player, false) == false) return HookResult.Continue;

        void PlayerSpawnNextFrame(CCSPlayerController player, CHandle<CCSPlayerPawn> pawn)
        {
            if (ValidateCallerPlayer(player, false) == false) return;
            pawn.Value.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DISSOLVING;

            if (ValidateCallerPlayer(player, false) == false) return;
            pawn.Value.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DISSOLVING;
            if (ValidateCallerPlayer(player, false) == false) return;

            VirtualFunctionVoid<nint> collisionRulesChanged = new VirtualFunctionVoid<nint>(pawn.Value.Handle, OnCollisionRulesChangedOffset.Get());

            if (ValidateCallerPlayer(player, false) == false) return;
            collisionRulesChanged.Invoke(pawn.Value.Handle);
        }
        return HookResult.Continue;
    }

    public class WIN_LINUX<T>
    {
        [JsonPropertyName("Windows")]
        public T Windows { get; private set; }

        [JsonPropertyName("Linux")]
        public T Linux { get; private set; }

        public WIN_LINUX(T windows, T linux)
        {
            this.Windows = windows;
            this.Linux = linux;
        }

        public T Get()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return this.Windows;
            }
            else
            {
                return this.Linux;
            }
        }
    }
}