using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiWeildWestTG : TeamGamesGameBase
    {
        public MultiWeildWestTG() : base(TeamGamesMultiChoices.WeildWest)
        {
        }

        internal override void StartGame(Action callback)
        {
            GiveAction("", "@t", "revolver", TargetForArgument.None, false);
            base.StartGame(callback);
        }

        internal override void Clear()
        {
            GetPlayers(CsTeam.Terrorist)
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                RemoveWeapons(x, true);
            });
            base.Clear();
        }
    }
}