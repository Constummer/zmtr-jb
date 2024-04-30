using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Memory;
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

        if (ValidateCallerPlayer(player, false) == false) return HookResult.Continue;

        PlayerSpawnNextFrame(player);
        if (ValidateCallerPlayer(player, false) == false) return HookResult.Continue;

        return HookResult.Continue;
    }

    private void PlayerSpawnNextFrame(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        player.PlayerPawn.Value.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DISSOLVING;
        if (player?.Collision?.CollisionGroup != null)
        {
            player.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DISSOLVING;
        }

        if (ValidateCallerPlayer(player, false) == false) return;
        if (player?.Collision?.CollisionAttribute?.CollisionGroup != null)
        {
            player.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DISSOLVING;
        }
        player.PlayerPawn.Value.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DISSOLVING;
        if (ValidateCallerPlayer(player, false) == false) return;

        VirtualFunctionVoid<nint> collisionRulesChanged = new VirtualFunctionVoid<nint>(player.PlayerPawn.Value.Handle, OnCollisionRulesChangedOffset.Get());

        if (ValidateCallerPlayer(player, false) == false) return;
        collisionRulesChanged.Invoke(player.PlayerPawn.Value.Handle);
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