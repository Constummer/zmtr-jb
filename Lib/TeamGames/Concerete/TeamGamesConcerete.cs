namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class TeamGamesGameBase
    {
        public TeamGamesGameBase(TeamGamesMultiChoices multiChoice)
        {
            MultiChoice = multiChoice;
        }

        public TeamGamesGameBase(TeamGamesSoloChoices soloChoice)
        {
            SoloChoice = soloChoice;
        }

        public TeamGamesMultiChoices MultiChoice { get; set; } = TeamGamesMultiChoices.None;
        public TeamGamesSoloChoices SoloChoice { get; set; } = TeamGamesSoloChoices.None;

        internal virtual void StartGame(Action callback)
        {
            callback();
        }
    }
}