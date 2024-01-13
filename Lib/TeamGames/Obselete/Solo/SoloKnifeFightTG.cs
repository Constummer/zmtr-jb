namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloKnifeFightTG : TeamGamesGameBase
    {
        public SoloKnifeFightTG() : base(TeamGamesSoloChoices.KnifeFight)
        {
        }

        internal override void StartGame(Action callback)
        {
            GetPlayers()
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                x.GiveNamedItem($"weapon_knife");
            });
            base.StartGame(callback);
        }
    }
}