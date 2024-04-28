using CounterStrikeSharp.API.Modules.Menu;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level09 : BattlePassBase
    {
        [JsonIgnore]
        public const int AK47Kill = 300;

        [JsonIgnore]
        public const int CtKill = 60;

        public int CurrentAK47Kill { get; set; } = 0;
        public int CurrentCtKill { get; set; } = 0;

        public BattlePass_Level09() : base(9, 10, 1750, 0)
        {
        }

        internal override void EventAK47Kill()
        {
            CurrentAK47Kill++;
            base.EventAK47Kill();
            CheckIfLevelUp(false);
        }

        internal override void EventCTKilled()
        {
            CurrentCtKill++;
            base.EventCTKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentAK47Kill >= AK47Kill &&
                CurrentCtKill >= CtKill &&
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
            menu.AddMenuOption($"{CurrentCtKill}/{CtKill} {CT_CamelCase} Kill", null, true);
            menu.AddMenuOption($"{CurrentAK47Kill}/{AK47Kill} AK47 Kill", null, true);
        }
    }
}