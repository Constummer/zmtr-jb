using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloWildWestTG : TeamGamesGameBase
    {
        public SoloWildWestTG() : base(TeamGamesSoloChoices.WildWest)
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
                if (ValidateCallerPlayer(x, false) == false) return;
                RemoveWeapons(x, true);
            });
            base.Clear();
        }
    }
}