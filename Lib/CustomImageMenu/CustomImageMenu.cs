using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    //not completed

    /*
     * TİTLE
     * RESIM
     * F ILE SEC
     * SONRAKI SAYFA -> shift
     * ONCEKI SAYFA -> ctrl
     * KAPAT -> tab
     */

    private static Dictionary<ulong, CustomImageMenu> PlayerCustomImageMenus { get; set; } = new();

    internal class CustomImageMenu
    {
        public class CustomImageMenuItem
        {
            public string ImagePath { get; set; } = "";
            public Action? Action { get; set; } = null;
        }

        internal CustomImageMenuItem Current { get; set; } = new();
        internal Dictionary<string, CustomImageMenuItem> Pages { get; set; } = new();

        internal void Add(string title, string imagePath, Action action)
        {
            if (Pages == null)
            {
                Pages = new();
            }
            var item = new CustomImageMenuItem()
            {
                ImagePath = imagePath,
                Action = action
            };
            if (Pages.Count == 0)
            {
                Current = item;
            }
            Pages.Add(title, item);
        }

        internal string Build()
        {
            return "";
        }

        internal void Choose(CCSPlayerController player, CustomImageMenu menu)
        {
            if (Current?.Action != null)
            {
                Current.Action();
            }
        }

        internal void NextPage()
        {
        }

        internal void PrevPage()
        {
        }
    }

    private static void CustomImageMenuOnTick(CCSPlayerController player)
    {
        if (PlayerCustomImageMenus.TryGetValue(player.SteamID, out var menu))
        {
            bool isFButtonPressed = (player.Pawn.Value.MovementServices!.Buttons.ButtonStates[0] & FButtonIndex) != 0;
            if (isFButtonPressed)
            {
                menu.Choose(player, menu);
            }
            else if ((player.Buttons & PlayerButtons.Walk) != 0)
            {
                menu.NextPage();
            }
            else if ((player.Buttons & PlayerButtons.Duck) != 0)
            {
                menu.PrevPage();
            }
            //else if ((player.Buttons & PlayerButtons.Use) != 0)
            //{
            //    player.Choose();
            //}
            else if (((long)player.Buttons & 8589934592) == 8589934592)
            {
                PlayerCustomImageMenus.Remove(player.SteamID);
            }
            else
            {
                PrintToCenterHtml(player, menu.Build());
            }
        }
    }
}