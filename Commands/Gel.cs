using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gel

    [ConsoleCommand("gel")]
    public void Gel(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        GetPlayers()
                   .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player.PlayerName))
                   .ToList()
                   .ForEach(x =>
                   {
                       if (x.SteamID != player.SteamID)
                       {
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} yanına ışınladı{CC.W}.");
                           }
                           var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                           x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), ANGLE_ZERO, VEC_ZERO);
                       }
                   });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}yanına ışınladı");
        }
    }

    [ConsoleCommand("gelt")]
    public void GelT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@t", player.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player.SteamID)
                  {
                      var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.DR}Mahkûmları {CC.W}yanına ışınladı.");
    }

    [ConsoleCommand("gelct")]
    public void GelCt(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }

        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@ct", player!.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player!.SteamID)
                  {
                      var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.B}Gardiyanları {CC.W}yanına ışınladı.");
    }

    [ConsoleCommand("gelall")]
    public void GelAll(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@all", player!.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player!.SteamID)
                  {
                      var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.G}herkesi {CC.W}yanına ışınladı.");
    }

    [ConsoleCommand("geltakim")]
    [CommandHelper(1, "<takimno>")]
    public void geltakim(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.GetArg(1) : "0";

        if (int.TryParse(target, out var value))
        {
            if (value > 18)
            {
                player.PrintToChat("Max 18 girebilirsin");
                return;
            }
            else if (value < 0)
            {
                player.PrintToChat("Min 0 girebilirsin");
                return;
            }
            else
            {
                GelTeam(player, value);
            }
        }
        else
        {
            GelTeam(player, value);
        }
    }

    [ConsoleCommand("gelkirmizi")]
    public void gelkirmizi(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        GelTeam(player, CC.R);
    }

    [ConsoleCommand("gelmavi")]
    public void gelmavi(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        GelTeam(player, CC.B);
    }

    [ConsoleCommand("gelyesil")]
    public void gelyesil(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        GelTeam(player, CC.G);
    }

    [ConsoleCommand("gelgri")]
    public void gelgri(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        GelTeam(player, CC.Gr);
    }

    //[ConsoleCommand("gelkoyukirmizi")]
    //[ConsoleCommand("gelkirmizi2")]
    //public void gelkoyukirmizi(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.DR);
    //}

    //[ConsoleCommand("gelaciksari")]
    //[ConsoleCommand("gelsari2")]
    //public void gelaciksari(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.LY);
    //}

    //[ConsoleCommand("gelacikmavi")]
    //[ConsoleCommand("gelmavi2")]
    //public void gelacikmavi(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.LB);
    //}

    //[ConsoleCommand("gelkoyuyesil")]
    //[ConsoleCommand("gelyesil3")]
    //public void gelkoyuyesil(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.Ol);
    //}

    //[ConsoleCommand("gelacikyesil")]
    //[ConsoleCommand("gelyesil2")]
    //public void gelacikyesil(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.L);
    //}

    //[ConsoleCommand("gelacikmor")]
    //[ConsoleCommand("gelmor2")]
    //public void gelacikmor(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.LP);
    //}

    //[ConsoleCommand("gelmor")]
    //public void gelmor(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.P);
    //}

    //[ConsoleCommand("gelsari")]
    //public void gelsari(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.Y);
    //}

    //[ConsoleCommand("gelaltin")]
    //[ConsoleCommand("gelsari3")]
    //public void gelaltin(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.Go);
    //}

    //[ConsoleCommand("gelgumus")]
    //public void gelgumus(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.S);
    //}

    //[ConsoleCommand("gelkoyumavi")]
    //[ConsoleCommand("gelmavi3")]
    //public void gelkoyumavi(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.DB);
    //}

    //[ConsoleCommand("gelmavigri")]
    //[ConsoleCommand("gelmavi4")]
    //public void gelmavi4(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.BG);
    //}

    //[ConsoleCommand("gelbordo")]
    //public void gelbordo(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.M);
    //}

    //[ConsoleCommand("gelacikkirmizi")]
    //[ConsoleCommand("gelkirmizi2")]
    //public void gelacikkirmizi(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.LR);
    //}

    //[ConsoleCommand("gelturuncu")]
    //public void gelturuncu(CCSPlayerController? player, CommandInfo info)
    //{
    //    if (ValidateCallerPlayer(player) == false)
    //    {
    //        return;
    //    }
    //    GelTeam(player, CC.Or);
    //}

    private static void GelTeam(CCSPlayerController? player, char color)
    {
        if (player.PawnIsAlive == false)
        {
            return;
        }

        var index = GetTeamIndexByColor(color);

        if (index == -1)
        {
            return;
        }

        if (TeamSteamIds.TryGetValue(index, out var plist))
        {
            var res = GetTeamColorAndTextByIndex(index);
            GetPlayers(CsTeam.Terrorist)
                  .Where(x => x.PawnIsAlive)
                  .ToList()
                  .ForEach(x =>
                  {
                      if (plist != null)
                      {
                          if (plist.Contains(x.SteamID))
                          {
                              var playerAbs = player.PlayerPawn.Value.AbsOrigin;
                              x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                          }
                      }
                  });
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {res.Msg} Takımındaki Mahkûmları {CC.W}yanına ışınladı.");
        }
    }

    private void GelTeam(CCSPlayerController? player, int index)
    {
        if (TeamSteamIds.TryGetValue(index, out var plist))
        {
            var res = GetTeamColorAndTextByIndex(index);
            GetPlayers(CsTeam.Terrorist)
                  .Where(x => x.PawnIsAlive)
                  .ToList()
                  .ForEach(x =>
                  {
                      if (plist != null)
                      {
                          if (plist.Contains(x.SteamID))
                          {
                              var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                              x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                          }
                      }
                  });
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {res.Msg} Takımındaki Mahkûmları {CC.W}yanına ışınladı.");
        }
    }

    #endregion Gel
}