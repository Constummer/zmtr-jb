using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using System.Runtime.InteropServices;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ccoingun")]
    public void ccoingun(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        Server.NextWorldUpdate(() =>
        {
            CBasePlayerWeapon? activeweapon = player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value;

            if (activeweapon != null)
            {
                UpdateModel(player, activeweapon, "models/coop/challenge_coin.vmdl", true);
            }
        });
        static void UpdateModel(CCSPlayerController player, CBasePlayerWeapon weapon, string model, bool update)
        {
            weapon.Globalname = $"{GetViewModel(player)},{model}";
            weapon.SetModel(model);

            if (update)
            {
                SetViewModel(player, model);
            }
        }
        static unsafe string GetViewModel(CCSPlayerController player)
        {
            return ViewModel(player).VMName;
        }
        static unsafe CBaseViewModel ViewModel(CCSPlayerController player)
        {
            CCSPlayer_ViewModelServices viewModelServices = new(player.PlayerPawn.Value!.ViewModelServices!.Handle);

            nint ptr = viewModelServices.Handle + Schema.GetSchemaOffset("CCSPlayer_ViewModelServices", "m_hViewModel");
            Span<nint> viewModels = MemoryMarshal.CreateSpan(ref ptr, 3);

            CHandle<CBaseViewModel> viewModel = new(viewModels[0]);

            return viewModel.Value!;
        }
        static unsafe void SetViewModel(CCSPlayerController player, string model)
        {
            ViewModel(player).SetModel(model);
        }
    }
}