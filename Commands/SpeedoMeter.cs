using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static List<ulong> SpeedoMeterActive = new List<ulong>();

    #region SpeedoMeter

    [ConsoleCommand("hizim")]
    [ConsoleCommand("speedim")]
    public void SpeedoMeter(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (SpeedoMeterActive.Any(x => x == player.SteamID))
        {
            SpeedoMeterActive = SpeedoMeterActive.Where(x => x != player.SteamID).ToList();
        }
        else
        {
            SpeedoMeterActive.Add(player.SteamID);
        }
    }

    private void SpeedoMeterOnTick(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (SpeedoMeterActive.Contains(player.SteamID))
        {
            if (!VoteInProgress && !KomAlVoteInProgress)
            {
                var buttons = player.Buttons;
                PrintToCenterHtml(player,
              $"<pre>HIZ: <font color='#00FF00'>{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())}</font><br>" +
                          $"{((buttons & PlayerButtons.Left) != 0 ? "←" : "_")} " +
                          $"{((buttons & PlayerButtons.Forward) != 0 ? "W" : "_")} " +
                          $"{((buttons & PlayerButtons.Right) != 0 ? "→" : "_")}<br>" +
                          $"{((buttons & PlayerButtons.Moveleft) != 0 ? "A" : "_")} " +
                          $"{((buttons & PlayerButtons.Back) != 0 ? "S" : "_")} " +
                          $"{((buttons & PlayerButtons.Moveright) != 0 ? "D" : "_")} </pre>");
                //string htmlString = $"<pre style='position: relative;'>" +
                //   $"<div style='position: absolute; top: 0; left: 0; width: 100%; height: 100%; " +
                //   "background-image: url(\"https://i.ibb.co/SXkHMj6/countdown.png\"); " +
                //   "background-size: cover; background-repeat: no-repeat;'></div>" +
                //   $"<span style=\"position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); " +
                //   "font-size: 20px; z-index: 1;\">➤</span>" +
                //   $"Speed: <font color='#00FF00'>{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())}</font><br>" +
                //   $"{((buttons & PlayerButtons.Left) != 0 ? "←" : "_")} " +
                //   $"{((buttons & PlayerButtons.Forward) != 0 ? "W" : "_")} " +
                //   $"{((buttons & PlayerButtons.Right) != 0 ? "→" : "_")}<br>" +
                //   $"{((buttons & PlayerButtons.Moveleft) != 0 ? "A" : "_")} " +
                //   $"{((buttons & PlayerButtons.Back) != 0 ? "S" : "_")} " +
                //   $"{((buttons & PlayerButtons.Moveright) != 0 ? "D" : "_")} </pre>";

                //string htmlString = $"<pre style='position: relative; max-width: 400px; min-width: 200px; max-height: 200px; min-height: 100px;'>" +
                //   $"<div style='position: absolute; top: 0; left: 0; width: 100%; height: 100%; " +
                //   "background-image: url(\"https://i.ibb.co/SXkHMj6/countdown.png\"); " +
                //   "background-size: cover; background-repeat: no-repeat;'>" +
                //   $"<span style=\"position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); " +
                //   "font-size: 20px; z-index: 1;\">➤</span>" +
                //   $"Speed: <font color='#00FF00'>{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())}</font><br>" +
                //   $"{((buttons & PlayerButtons.Left) != 0 ? "←" : "_")} " +
                //   $"{((buttons & PlayerButtons.Forward) != 0 ? "W" : "_")} " +
                //   $"{((buttons & PlayerButtons.Right) != 0 ? "→" : "_")}<br>" +
                //   $"{((buttons & PlayerButtons.Moveleft) != 0 ? "A" : "_")} " +
                //   $"{((buttons & PlayerButtons.Back) != 0 ? "S" : "_")} " +
                //   $"{((buttons & PlayerButtons.Moveright) != 0 ? "D" : "_")}</div> </pre>";

                ////string htmlString = $"<pre style='position: relative;'>" +
                //   $"<div style='position: absolute; top: 0; left: 0; width: 100%; height: 100%; " +
                //   "background-image: url(\"https://i.ibb.co/SXkHMj6/countdown.png\"); " +
                //   "background-size: cover; background-repeat: no-repeat;'></div>" +
                //   $"<span style=\"position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); " +
                //   "font-size: 20px;\">➤</span>" +
                //   $"Speed: <font color='#00FF00'>{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())}</font><br>" +
                //   $"{((buttons & PlayerButtons.Left) != 0 ? "←" : "_")} " +
                //   $"{((buttons & PlayerButtons.Forward) != 0 ? "W" : "_")} " +
                //   $"{((buttons & PlayerButtons.Right) != 0 ? "→" : "_")}<br>" +
                //   $"{((buttons & PlayerButtons.Moveleft) != 0 ? "A" : "_")} " +
                //   $"{((buttons & PlayerButtons.Back) != 0 ? "S" : "_")} " +
                //   $"{((buttons & PlayerButtons.Moveright) != 0 ? "D" : "_")} </pre>";

                //string htmlString = $"<pre " +
                //   //$"style='background-image: url(\"https://i.ibb.co/SXkHMj6/countdown.png\"); " +
                //   //"background-size: 40px 40px; background-repeat: no-repeat;'" +
                //   ">" +
                //   $"Speed: <font color='#00FF00'>{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())}</font><br>" +
                //   $"{((buttons & PlayerButtons.Left) != 0 ? "←" : "_")} " +
                //   $"{((buttons & PlayerButtons.Forward) != 0 ? "W" : "_")} " +
                //   $"{((buttons & PlayerButtons.Right) != 0 ? "→" : "_")}<br>" +
                //   $"{((buttons & PlayerButtons.Moveleft) != 0 ? "A" : "_")} " +
                //   $"{((buttons & PlayerButtons.Back) != 0 ? "S" : "_")} " +
                //   $"{((buttons & PlayerButtons.Moveright) != 0 ? "D" : "_")} " +
                //   "<img src=\"https://i.ibb.co/SXkHMj6/countdown.png\" alt=\"Icon\" style=\"width: 2px; height: 1px;\">" +
                //   $"</pre>";

                //  var btnPart = $"{((buttons & PlayerButtons.Left) != 0 ? "←" : "_")} " +
                //$"{((buttons & PlayerButtons.Forward) != 0 ? "W" : "_")} " +
                //$"{((buttons & PlayerButtons.Right) != 0 ? "→" : "_")}<br>" +
                //$"{((buttons & PlayerButtons.Moveleft) != 0 ? "A" : "_")} " +
                //$"{((buttons & PlayerButtons.Back) != 0 ? "S" : "_")} " +
                //$"{((buttons & PlayerButtons.Moveright) != 0 ? "D" : "_")}";
                //  var textPart = $"Speed: <font color='#00FF00'>{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())}</font><br>";
                //  SharpTimerPrintHtml(player, @$"{textPart}{btnPart}");

                //SharpTimerPrintHtml(player, @$"<div style=""width: 20px; height: 20px; position: relative;"">
                //                  <img src=""https://i.ibb.co/54bc5fZ/countdown2.png"" style=""width: 100%; height: 100%; position: absolute; top: 0; left: 0; z-index: 99991;""/>
                //                  <div style=""z-index: 1; position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index: 99992;"">
                //                      {textPart}{btnPart}
                //                  </div>
                //               </div>");========================================================

                //SharpTimerPrintHtml(player, @$"<pre><img src=""https://i.ibb.co/SXkHMj6/countdown.png"" width=""20"" height=""20""\>                                {textPart}{btnPart}</pre>");
                //SharpTimerPrintHtml(player, @$"<pre><img src=""https://i.ibb.co/SXkHMj6/countdown.png""\>{textPart}{btnPart}</pre>");
                //SharpTimerPrintHtml(player, @$"<pre><img src=""https://i.ibb.co/SXkHMj6/countdown.png"" width=""20"" height=""20"">{textPart}{btnPart}</pre>");
                //SharpTimerPrintHtml(player, @$"<pre><img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:20px; height:20px;"">{textPart}{btnPart}</pre>");
                //SharpTimerPrintHtml(player, @$"<pre><style>img {{ width: 25px; height: 50px; }}</style><img src=""https://i.ibb.co/SXkHMj6/countdown.png"">{textPart}{btnPart}</pre>");
                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:20px; height:20px;"">{textPart}{btnPart}");
                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:2rem; height:2rem;"">{textPart}{btnPart}");
                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:2em; height:2em;"">{textPart}{btnPart}");
                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:50%; height:50%;"">{textPart}{btnPart}");
                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:10vw; height:10vw;"">{textPart}{btnPart}");
                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:10vh; height:10vh;"">{textPart}{btnPart}");
                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:10vmin; height:10vmin;"">{textPart}{btnPart}");
                //SharpTimerPrintHtml(player, @$"<div style=""position: relative; display: inline-block; background: url('https://i.ibb.co/SXkHMj6/countdown.png') no-repeat center center; background-size: cover; width: 20px; height: 20px;"">{textPart}{btnPart}</div>");
                //SharpTimerPrintHtml(player, @$"<div style=""position: relative; width: 20px; height: 20px;""><img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width: 100%; height: 100%; position: absolute; top: 0; left: 0;"">{textPart}{btnPart}</div>");
                //SharpTimerPrintHtml(player, @$"<div style=""position: relative; width: 20px; height: 20px;""><img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width: 100%; height: 100%; position: absolute; top: 0; left: 0; z-index: -1;"">{textPart}{btnPart}</div>");
                //SharpTimerPrintHtml(player, @$"<div style=""position: relative; width: 20px; height: 20px; background: url('https://i.ibb.co/SXkHMj6/countdown.png') no-repeat center center; background-size: cover;"">{textPart}{btnPart}</div>");

                //SharpTimerPrintHtml(player, @$"<div style=""width:20px; height:20px;""><img src=""https://i.ibb.co/SXkHMj6/countdown.png"" style=""width:100%; height:100%;""></div>{textPart}{btnPart}");

                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/r29dSsp/countdown.png"" style=""width:20px; height:20px;"">{textPart}{btnPart}");
                //SharpTimerPrintHtml(player, @$"<img src=""https://i.ibb.co/0y5sSt9/countdown.png"" style=""width:20px; height:20px;"">{textPart}{btnPart}");=======================
                //SharpTimerPrintHtml(player, @$"<div style=""position: relative; width: 20px; height: 20px; background: url('https://i.ibb.co/0y5sSt9/countdown.png') no-repeat center center; background-size: cover;""><div style=""position: absolute; top: 0; left: 0; width: 100%; height: 100%;"">{textPart}{btnPart}</div></div>");
                //SharpTimerPrintHtml(player, @$"<div style=""display: inline-block; width: 20px; height: 20px; position: relative;""><img src=""https://i.ibb.co/0y5sSt9/countdown.png"" style=""width: 100%; height: 100%; z-index: -1;""/>{textPart}{btnPart}</div>");

                //SharpTimerPrintHtml(player, @$"<div style=""position: relative; width: 20px; height: 20px; background: url('https://i.ibb.co/54bc5fZ/countdown2.png') no-repeat center center; background-size: cover;"">
                //                  <div style=""position: absolute; top: 0; left: 0; width: 100%; height: 100%;"">
                //                      {textPart}{btnPart}
                //                  </div>
                //               </div>");
            }
        }
    }

    #endregion SpeedoMeter
}