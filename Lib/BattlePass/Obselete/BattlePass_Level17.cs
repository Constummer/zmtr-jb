using CounterStrikeSharp.API.Modules.Menu;
using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level17 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 30;

        [JsonIgnore]
        public const int CTWin = 45;

        public int CurrentSut { get; set; } = 0;
        public int CurrentCTWin { get; set; } = 0;

        public BattlePass_Level17() : base(17, 10, 2500, 0)
        {
        }

        internal override void OnSutCommand()
        {
            CurrentSut++;
            base.OnSutCommand();
            CheckIfLevelUp(false);
        }

        internal override void OnRoundCTWinCommand()
        {
            CurrentCTWin++;
            base.OnRoundCTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentSut >= Sut &&
                CurrentTime >= Time &&
                CurrentCTWin >= CTWin)
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
            menu.AddMenuOption($"{CurrentCTWin}/{CTWin} {CT_LowerPositioning} kazanma", null, true);
        }
    }
}