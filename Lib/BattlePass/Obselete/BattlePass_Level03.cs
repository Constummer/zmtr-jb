using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level03 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 10;

        [JsonIgnore]
        public const int TWin = 50;

        public int CurrentSut { get; set; } = 0;
        public int CurrentTWin { get; set; } = 0;

        public BattlePass_Level03() : base(3, 10, 750, 0)
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