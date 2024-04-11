namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool LrActive = false;

    public ActiveLr? ActivatedLr { get; set; } = null;

    private CounterStrikeSharp.API.Modules.Timers.Timer LRTimer { get; set; } = null;

    public List<LrData> LrDatas = new List<LrData>()
    {
        new("Deagle", LrChoices.Deagle,"weapon_deagle"),
        //new("No Scope Scout", LrChoices.NoScopeScout,"weapon_ssg08", true),
        new("No Scope Awp", LrChoices.NoScopeAwp,"weapon_awp", true)
    };

    public enum LrChoices
    {
        None = 0,
        Deagle,
        NoScopeScout,
        NoScopeAwp
    }

    public class LrData
    {
        public LrData()
        {
        }

        public LrData(string text, LrChoices choice, string weaponName, bool scopeDisable = false)
        {
            Text = text;
            Choice = choice;
            WeaponName = weaponName;
            ScopeDisable = scopeDisable;
        }

        public string Text { get; set; }
        public LrChoices Choice { get; set; }
        public string WeaponName { get; }
        public bool ScopeDisable { get; set; }
    }

    public class ActiveLr : LrData
    {
        public ActiveLr(ulong mahkumSteamId, ulong gardSteamId, LrData item)
        {
            MahkumSteamId = mahkumSteamId;
            GardSteamId = gardSteamId;
            Text = item.Text;
            Choice = item.Choice;
            ScopeDisable = item.ScopeDisable;
        }

        public uint GardWeaponIndex { get; set; }
        public ulong MahkumSteamId { get; set; }
        public ulong GardSteamId { get; set; }
    }
}