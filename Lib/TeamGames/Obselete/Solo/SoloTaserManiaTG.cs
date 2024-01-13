using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloTaserManiaTG : TeamGamesGameBase
    {
        public SoloTaserManiaTG() : base(TeamGamesSoloChoices.TaserMania)
        {
        }

        internal override void StartGame(Action callback)
        {
            Global?.SinirsizXAction(null, "@t", "taser");
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