using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Piyango

    [ConsoleCommand("jackpot")]
    [ConsoleCommand("piyango")]
    [CommandHelper(0, "<kredi>")]
    public void Piyango(CCSPlayerController? player, CommandInfo info)
    {
        if (KumarKapatDisable)
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W} - piyango kapalı -.");
            return;
        }
        if (IsGameBannedToday())
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Bu mübarek günde, yakışıyor mu müslüman din kardeşim - piyango kapalı -.");
            return;
        }
        var total = PiyangoPlayers.ToList().Select(x => x.Value).Sum();

        var creditStr = info.ArgString.GetArg(0);
        if (string.IsNullOrWhiteSpace(creditStr) || new String(creditStr.Where(Char.IsDigit).ToArray()).Length == 0)
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kullanım = {CC.G}!piyango <kredi>");
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
            return;
        }

        if (!int.TryParse(creditStr, out int credit))
        {
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.R}GEÇERSİZ MİKTAR");
            player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
            return;
        }
        else
        {
            if (credit < 10 || credit > 100)
            {
                player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.R}Min 10, Max 100 kredi girebilirsin");
                player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
                return;
            }
            else
            {
                if (PiyangoPlayers.TryGetValue(player.SteamID, out var enteredCredit))
                {
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.B}{enteredCredit} {CC.W}kredi bastın, değiştiremezsin!");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
                    return;
                }
                else
                {
                    var data = GetPlayerMarketModel(player.SteamID);

                    if (data.Model == null || data.Model.Credit < credit || data.Model.Credit - credit < 0)
                    {
                        player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Yetersiz Bakiye!");
                        player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total}");
                        return;
                    }

                    PiyangoPlayers.Add(player.SteamID, credit);
                    data.Model!.Credit -= credit;
                    PlayerMarketModels[player.SteamID] = data.Model;
                    Server.PrintToChatAll($" {CC.Ol}[PİYANGO] {CC.Ol}{player.PlayerName} {CC.W}Piyango'ya{CC.G} {credit} {CC.W}kredi ile katıldı!");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.G} {credit} {CC.W}kredi bastın!");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Güncel Kredin: {CC.G}{data.Model!.Credit}");
                    player.PrintToChat($" {CC.Ol}[PİYANGO] {CC.W}Kasa = {total + credit}");
                }
            }
        }
    }

    #endregion Piyango
}