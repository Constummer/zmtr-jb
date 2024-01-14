using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloCoctailPartyTG : TeamGamesGameBase
    {
        public SoloCoctailPartyTG() : base(TeamGamesSoloChoices.CoctailParty)
        {
        }

        internal override void StartGame(Action callback)
        {
            GetPlayers(CsTeam.Terrorist)
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                RemoveWeapons(x, false);
            });
            Global?.SinirsizXAction(null, "@t", "incgrenade");
            base.StartGame(callback);
        }

        internal override void Clear()
        {
            Global?.SinirsizXKapaAction("@t", "");
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