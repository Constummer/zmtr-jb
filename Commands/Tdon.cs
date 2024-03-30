using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tdon", "Freeze a t.")]
    [ConsoleCommand("td", "Freeze a t.")]
    public void OnTDonCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.Terrorist)
                         .Where(x => x.PawnIsAlive)
                         .ToList()
                         .ForEach(x =>
                         {
                             if (TeamActive == false)
                             {
                                 SetColour(x, Config.Burry.BuryColor);
                             }
                             SetMoveType(x, MoveType_t.MOVETYPE_OBSOLETE);
                             //SetStateChanged(x, "CBaseEntity", "m_MoveType");
                             //Utilities.SetStateChanged(x.PlayerPawn.Value, "CBaseEntity", "m_MoveType");
                             //x.Pawn.Value!.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                             //x.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
                             RefreshPawn(x);
                         });
        FreezeOrUnfreezeSound();
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{T_PluralLowerObjective} {CC.B}dondurdu{CC.W}.");
    }

    [ConsoleCommand("tdonboz", "Unfreeze t.")]
    [ConsoleCommand("tdb", "Unfreeze t.")]
    public void TDonbozCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TdonbozAction();
        FreezeOrUnfreezeSound();
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{T_PluralCamelPossesive} {CC.B}donunu kaldırdı{CC.W}.");
    }

    private static void TdonbozAction()
    {
        GetPlayers(CsTeam.Terrorist)
                        .Where(x => x.PawnIsAlive)
                        .ToList()
                        .ForEach(x =>
                        {
                            if (TeamActive == false)
                            {
                                SetColour(x, DefaultColor);
                            }
                            SetMoveType(x, MoveType_t.MOVETYPE_WALK);
                            RefreshPawn(x);
                        });
    }

    private static void AllDonbozAction()
    {
        GetPlayers()
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               if (TeamActive == false)
               {
                   SetColour(x, DefaultColor);
               }
               SetMoveType(x, MoveType_t.MOVETYPE_WALK);

               RefreshPawn(x);
           });
    }
}