﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace AAAAA;

public class AAAAA : BasePlugin
{
    public override string ModuleName => "AAAAA";

    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "AAAAA";

    public override void Load(bool hotReload)
    {
        RegisterEventHandler((GameEventHandler<EventPlayerSpawn>)((@event, _) =>
        {
            if (@event == null)
                return HookResult.Continue;
            var player = @event.Userid;
            if (player != null && player.IsValid && player.PlayerPawn.IsValid)
            {
                if ((CsTeam)player.TeamNum == CsTeam.CounterTerrorist)
                {
                    player.GiveNamedItem("item_assaultsuit");
                    player.GiveNamedItem("weapon_deagle");
                    player.GiveNamedItem("weapon_m4a1");
                }
            }
            return HookResult.Continue;
        }));
    }
}