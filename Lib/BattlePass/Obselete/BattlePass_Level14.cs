using CounterStrikeSharp.API.Modules.Menu;
using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level14 : BattlePassBase
    {
        [JsonIgnore]
        public const int NoScopeKill = 25;

        [JsonIgnore]
        public const int CTKill = 70;

        [JsonIgnore]
        public const int WKill = 30;

        public int CurrentNoScopeKill { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level14() : base(14, 10, 1000, 0)
        {
        }

        internal override void EventNoScopeKill()
        {
            CurrentNoScopeKill++;
            base.EventNoScopeKill();
            CheckIfLevelUp(false);
        }

        internal override void EventCTKilled()
        {
            CurrentCTKill++;
            base.EventCTKilled();
            CheckIfLevelUp(false);
        }

        internal override void EventWKilled()
        {
            CurrentWKill++;
            base.EventWKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentNoScopeKill >= NoScopeKill &&
                CurrentCTKill >= CTKill &&
                CurrentTime >= Time &&
                CurrentWKill >= WKill)
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
            menu.AddMenuOption($"{CurrentCTKill}/{CTKill} {CT_CamelCase} Kill", null, true);
            menu.AddMenuOption($"{CurrentNoScopeKill}/{NoScopeKill} No Scope Kill", null, true);
            menu.AddMenuOption($"{CurrentWKill}/{WKill} Komutçu Kill", null, true);
        }
    }
}