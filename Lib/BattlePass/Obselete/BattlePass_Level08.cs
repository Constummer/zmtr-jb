using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level08 : BattlePassBase
    {
        [JsonIgnore]
        public const int P90Kill = 100;

        [JsonIgnore]
        public const int WKill = 20;

        public int CurrentP90Kill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level08() : base(8, 10, 1500, 0)
        {
        }

        internal override void EventP90Kill()
        {
            CurrentP90Kill++;
            base.EventP90Kill();
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
            if (CurrentP90Kill >= P90Kill &&
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
            menu.AddMenuOption($"{CurrentP90Kill}/{P90Kill} P90 Kill", null, true);
            menu.AddMenuOption($"{CurrentWKill}/{WKill} Komutçu Kill", null, true);
        }
    }
}