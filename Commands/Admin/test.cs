//using CounterStrikeSharp.API;
//using CounterStrikeSharp.API.Core;
//using CounterStrikeSharp.API.Core.Attributes.Registration;
//using CounterStrikeSharp.API.Modules.Commands;
//using CounterStrikeSharp.API.Modules.Utils;
//using Microsoft.Extensions.Logging;

//namespace JailbreakExtras;

//public partial class JailbreakExtras
//{
//    private readonly List<ulong> _connectedPlayers = new();
//    private readonly Dictionary<ulong, bool> _playerUsingThirdPerson = new();
//    private readonly Dictionary<ulong, bool> _playerIsCrouching = new();
//    private readonly Dictionary<ulong, bool> _playerCanToggle = new();
//    private readonly Dictionary<ulong, CPointCamera> _playerThirdPerson = new();

//    public override void Load(bool hotReload)
//    {
//        Logger.LogDebug($"{ModuleName} v{ModuleVersion} yukleniyor...");

//        RegisterListener<Listeners.OnTick>(() =>
//        {
//            foreach (var playerSteamId in _connectedPlayers)
//            {
//                var player = Utilities.GetPlayers().Where(x => x.SteamID == playerSteamId).FirstOrDefault();
//                if (player is { IsValid: true, IsBot: false, PawnIsAlive: true } == false)
//                {
//                    continue;
//                }
//                ToggleThirdPerson(player);

//                if (_playerCanToggle[playerSteamId] == false) continue;

//                if ((player.Buttons & PlayerButtons.Use) != 0)
//                {
//                    _playerCanToggle[playerSteamId] = false;

//                    if (_playerUsingThirdPerson[playerSteamId] == false)
//                    {
//                        _playerUsingThirdPerson[playerSteamId] = true;

//                        AddTimer(0.1f, () =>
//                        {
//                            _playerCanToggle[playerSteamId] = true;
//                        });
//                    }
//                    else
//                    {
//                        _playerUsingThirdPerson[playerSteamId] = false;

//                        AddTimer(0.1f, () =>
//                        {
//                            _playerCanToggle[playerSteamId] = true;
//                        });
//                    }
//                }

//                if ((player.Buttons & PlayerButtons.Duck) != 0)
//                {
//                    _playerIsCrouching[playerSteamId] = true;
//                }
//                else
//                {
//                    _playerIsCrouching[playerSteamId] = false;
//                }
//            }
//        });

//        //Logger.LogDebug($"{ModuleDisplayName/*ambro bu nedir bilmiorum*/} v{ModuleVersion} yuklendi!");
//    }

//    [GameEventHandler]
//    public HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
//    {
//        var player = @event.Userid;

//        if (!player.IsValid || player.IsBot) return HookResult.Continue;

//        _connectedPlayers.Add(player.SteamID);
//        _playerUsingThirdPerson[player.SteamID] = false;
//        _playerIsCrouching[player.SteamID] = false;
//        _playerCanToggle[player.SteamID] = true;

//        return HookResult.Continue;
//    }

//    [GameEventHandler]
//    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
//    {
//        var player = @event.Userid;

//        if (!player.IsValid || player.IsBot) return HookResult.Continue;

//        _connectedPlayers.Remove(player.SteamID);
//        _playerUsingThirdPerson.Remove(player.SteamID);
//        _playerIsCrouching.Remove(player.SteamID);
//        _playerCanToggle.Remove(player.SteamID);

//        _playerThirdPerson.TryGetValue(player.SteamID, out var ThirdPerson);
//        ThirdPerson?.Remove();
//        _playerThirdPerson.Remove(player.SteamID);

//        return HookResult.Continue;
//    }

//    [GameEventHandler]
//    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
//    {
//        var player = @event.Userid;

//        if (!player.IsValid || player.IsBot) return HookResult.Continue;

//        if (_connectedPlayers.Contains(player.SteamID) == false)
//        {
//            _connectedPlayers.Add(player.SteamID);
//            _playerUsingThirdPerson[player.SteamID] = false;
//            _playerIsCrouching[player.SteamID] = false;
//            _playerCanToggle[player.SteamID] = true;
//        }

//        _playerUsingThirdPerson[player.SteamID] = false;

//        return HookResult.Continue;
//    }

//    [GameEventHandler]
//    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
//    {
//        var player = @event.Userid;

//        if (!player.IsValid || player.IsBot) return HookResult.Continue;

//        _playerUsingThirdPerson[player.SteamID] = false;

//        return HookResult.Continue;
//    }

//    public void ToggleThirdPerson(CCSPlayerController player)
//    {
//        if (_playerUsingThirdPerson[player.SteamID] == false)
//        {
//            if (_playerThirdPerson.TryGetValue(player.SteamID, out var value))
//            {
//                value.Remove();
//                _playerThirdPerson.Remove(player.SteamID);
//            }

//            return;
//        }

//        var entity = _playerThirdPerson.TryGetValue(player.SteamID, out var ThirdPerson) ? ThirdPerson : Utilities.CreateEntityByName<CPointCamera>("point_camera");

//        if (entity == null || !entity.IsValid)
//        {
//            Logger.LogError("Entity olusturalamadi.");
//            return;
//        }

//        entity.Teleport(
//            new Vector(
//                player.PlayerPawn.Value!.AbsOrigin!.X, -4.02f +
//                player.PlayerPawn.Value!.AbsOrigin!.Y,
//                player.PlayerPawn.Value!.AbsOrigin!.Z + (_playerIsCrouching[player.SteamID] ? 46.03f : 64.03f)
//            ),
//            player.PlayerPawn.Value!.EyeAngles,
//            player.PlayerPawn.Value!.AbsVelocity
//        );

//        entity.Active = true;
//        entity.DispatchSpawn();
//        _playerThirdPerson[player.SteamID] = entity;
//    }

//    [ConsoleCommand("css_tp", "Toggles the ThirdPerson")]
//    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
//    public void ToggleThirdPerson(CCSPlayerController player, CommandInfo? info)
//    {
//        if (!player.IsValid || !player.PawnIsAlive) return;

//        if (_playerCanToggle[player.SteamID] == false) return;

//        _playerUsingThirdPerson[player.SteamID] = !_playerUsingThirdPerson[player.SteamID];
//        _playerCanToggle[player.SteamID] = false;

//        AddTimer(0.1f, () =>
//        {
//            _playerCanToggle[player.SteamID] = true;
//        });
//    }
//}