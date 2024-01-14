using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiKnifeFightTG : TeamGamesGameBase
    {
        public MultiKnifeFightTG() : base(TeamGamesMultiChoices.KnifeFight)
        {
        }

        internal override void StartGame(Action callback)
        {
            GetPlayers(CsTeam.Terrorist)
             .Where(x => x.PawnIsAlive)
             .ToList()
             .ForEach(x =>
             {
                 if (ValidateCallerPlayer(x, false) == false) return;
                 RemoveWeapons(x, false);
                 if (ValidateCallerPlayer(x, false) == false) return;
                 x.GiveNamedItem($"weapon_knife");
             });
            base.StartGame(callback);
        }
    }
}