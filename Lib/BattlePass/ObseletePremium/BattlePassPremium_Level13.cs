using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePassPremium_Level13 : BattlePassPremiumBase
    {
        [JsonIgnore]
        public const int Sut = 25;

        [JsonIgnore]
        public const int TWin = 100;

        public int CurrentSut { get; set; } = 0;
        public int CurrentTWin { get; set; } = 0;

        public BattlePassPremium_Level13() : base(13, 10, 0, 500)
        {
        }

        internal override void OnSutCommand()
        {
            CurrentSut++;
            base.OnSutCommand();
            CheckIfLevelUp(false);
        }

        internal override void OnRoundTWinCommand()
        {
            CurrentTWin++;
            base.OnRoundTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentSut >= Sut &&
                CurrentTime >= Time &&
                CurrentTWin >= TWin)
            {
                base.CheckIfLevelUp(true);
            }
            else
            {
                base.CheckIfLevelUp(false);
            }
        }

        internal override void BuildLevelMenu(CenterHtmlMenu menu)
        {
            base.BuildLevelMenu(menu);
            menu.AddMenuOption($"{CurrentSut}/{Sut} Süt Olma", null, true);
            menu.AddMenuOption($"{CurrentTWin}/{TWin} {T_LowerPositioning} kazanma", null, true);
        }
    }
}