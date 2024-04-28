using CounterStrikeSharp.API.Modules.Menu;
using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level30 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 50;

        [JsonIgnore]
        public const int NoScopeKill = 50;

        [JsonIgnore]
        public const int Jump = 500_000;

        public int CurrentSut { get; set; } = 0;
        public int CurrentNoScopeKill { get; set; } = 0;
        public int CurrentJump { get; set; } = 0;

        public BattlePass_Level30() : base(30, 100, 10000, 5000)
        {
        }

        internal override void OnEventPlayerJump()
        {
            CurrentJump++;
            base.OnEventPlayerJump();
            CheckIfLevelUp(false);
        }

        internal override void EventNoScopeKill()
        {
            CurrentNoScopeKill++;
            base.EventNoScopeKill();
            CheckIfLevelUp(false);
        }

        internal override void OnSutCommand()
        {
            CurrentSut++;
            base.OnSutCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentJump >= Jump &&
                CurrentNoScopeKill >= NoScopeKill &&
                CurrentSut >= Sut &&
                CurrentTime >= Time)
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
            menu.AddMenuOption($"{CurrentJump}/{Jump} Zıplama", null, true);
            menu.AddMenuOption($"{CurrentSut}/{Sut} Süt Olma", null, true);
            menu.AddMenuOption($"{CurrentNoScopeKill}/{NoScopeKill} No Scope Kill", null, true);
        }
    }
}