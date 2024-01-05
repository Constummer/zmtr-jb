using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SeviyeOl

    [ConsoleCommand("slotol")]
    [ConsoleCommand("seviyeol")]
    public void SeviyeOl(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (PlayerLevels.ContainsKey(player.SteamID) == false)
        {
            InsertAndGetPlayerLevelData(player.SteamID);
            player.PrintToCenter($" {CC.LR}[ZMTR] {CC.G} Seviye sistemine Hoşgeldin.");
            player.PrintToCenter($" {CC.LR}[ZMTR] {CC.B} !seviye{CC.W}, {CC.B}!seviyem{CC.W}, {CC.B}!tp {CC.W} yazarak TP puanini öğrenebilirsin");
            player.PrintToCenter($" {CC.LR}[ZMTR] {CC.G} Sunucuda bulunduğun süre boyunca dakikada {CC.R}1{CC.G} TP Kazanacaksin");
            player.PrintToCenter($" {CC.LR}[ZMTR] {CC.G} Seviye sisteminden ayrılmak istersen {CC.R}!seviyeayril {CC.G} ile ayrılabilirsin");
            player.PrintToCenter($" {CC.LR}[ZMTR] {CC.G} Seviye sisteminin ayrıcalıkları hakkında bilgi almak için discorda katılmalısın ({CC.B}!dc{CC.G})");
            player.PrintToCenter($" {CC.LR}[ZMTR] {CC.Gr} TEKRAR HOŞ GELDİN");
        }
        else
        {
            player.PrintToCenter($" {CC.LR}[ZMTR] {CC.G} Halihazirda slotsun.");
        }
    }

    #endregion SeviyeOl
}