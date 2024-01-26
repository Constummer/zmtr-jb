namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool TgActive = false;
    public static CounterStrikeSharp.API.Modules.Timers.Timer TgTimer { get; set; } = null;
    public static TeamGamesGameBase? ActiveTeamGamesGameBase { get; set; } = null;

    public enum TeamGamesSoloChoices
    {
        None = 0,
        ChickenHunt,
        HotPatato,
        ChickenRoulette,
        GunFight,
        HeadShotOnly,
        KnifeFight,
        NoZoom,
        PistolZoomBattle,
        ReloadBattle,
        TaserMania,
        WildWest,
        HeGrenades,
        Machines500HP,
        CoctailParty,
        ChickenSlayer,
        HideFight,
        Corona,
        MORESOON,
    }

    public enum TeamGamesMultiChoices
    {
        None = 0,
        ChickenHunt,
        ChickenRoulette,
        GunFight,
        HeadShotOnly,
        KnifeFight,
        NoZoom,
        PistolZoomBattle,
        ReloadBattle,
        TaserMania,
        WeildWest,
        HeGrenades,
        Machines500HP,
        CoctailParty,
        ChickenSlayer,
        MORESOON
    }

    public class TGBaseClass
    {
        public TGBaseClass(string text
            , bool disabled
            , TeamGamesMultiChoices? multiChoice = TeamGamesMultiChoices.None
            , TeamGamesSoloChoices? soloChoice = TeamGamesSoloChoices.None)
        {
            Text = text;
            Disabled = disabled;
            MultiChoice = multiChoice ?? TeamGamesMultiChoices.None;
            SoloChoice = soloChoice ?? TeamGamesSoloChoices.None;
        }

        public string Text { get; set; }
        public bool Disabled { get; set; }
        public TeamGamesSoloChoices SoloChoice { get; set; } = TeamGamesSoloChoices.None;
        public TeamGamesMultiChoices MultiChoice { get; set; } = TeamGamesMultiChoices.None;
    }

    public List<TGBaseClass> SoloTGGamesMenu { get; set; } = new()
    {
        new ("Chicken Hunt",      false,  null, TeamGamesSoloChoices.ChickenHunt),
        new ("Hot Patato",        false,  null, TeamGamesSoloChoices.HotPatato),
        new ("Chicken Roulette",  false,  null, TeamGamesSoloChoices.ChickenRoulette),
        new ("Gun Fight",         false,  null, TeamGamesSoloChoices.GunFight),
        new ("HeadShot Only",     false,  null, TeamGamesSoloChoices.HeadShotOnly),
        new ("Knife Fight",       false,  null, TeamGamesSoloChoices.KnifeFight),
        new ("No Scope",          false,  null, TeamGamesSoloChoices.NoZoom),
        new ("Pistol Zoom Battle",false,  null, TeamGamesSoloChoices.PistolZoomBattle),
        new ("Reload Battle",     false,  null, TeamGamesSoloChoices.ReloadBattle),
        new ("Taser Mania",       false,  null, TeamGamesSoloChoices.TaserMania),
        new ("Weild West",        false,  null, TeamGamesSoloChoices.WildWest),
        new ("He Grenades",       false,  null, TeamGamesSoloChoices.HeGrenades),
        new ("Machines + 500HP",  false,  null, TeamGamesSoloChoices.Machines500HP),
        new ("Coctail Part",      false,  null, TeamGamesSoloChoices.CoctailParty),
        new ("Chicken Slayer",    false,  null, TeamGamesSoloChoices.ChickenSlayer),
        new ("Hide Fight",        false,  null, TeamGamesSoloChoices.HideFight),
        new ("Corona",            false,  null, TeamGamesSoloChoices.Corona),
        new ("VE ÇOK DAHA FAZLASI... YAKINDA",    true,  null, TeamGamesSoloChoices.MORESOON),
    };

    public List<TGBaseClass> MultiTGGamesMenu { get; set; } = new()
    {
        new ("Chiken Hunt",       false,  TeamGamesMultiChoices.ChickenHunt),
        new ("Chicken Roulette",  false,  TeamGamesMultiChoices.ChickenRoulette),
        new ("Gun Fight",         false,  TeamGamesMultiChoices.GunFight),
        new ("HeadShot Only",     false,  TeamGamesMultiChoices.HeadShotOnly),
        new ("Knife Fight",       false,  TeamGamesMultiChoices.KnifeFight),
        new ("No Scope",          false,  TeamGamesMultiChoices.NoZoom),
        new ("Pistol Zoom Battle",false,  TeamGamesMultiChoices.PistolZoomBattle),
        new ("Reload Battle",     false,  TeamGamesMultiChoices.ReloadBattle),
        new ("Taser Mania",       false,  TeamGamesMultiChoices.TaserMania),
        new ("Weild West",        false,  TeamGamesMultiChoices.WeildWest),
        new ("He Grenades",       false,  TeamGamesMultiChoices.HeGrenades),
        new ("Machines + 500HP",  false,  TeamGamesMultiChoices.Machines500HP),
        new ("Coctail Party",     false,  TeamGamesMultiChoices.CoctailParty),
        new ("Chicken Slayer",    false,  TeamGamesMultiChoices.ChickenSlayer),
        new ("VE ÇOK DAHA FAZLASI... YAKINDA",    true,  TeamGamesMultiChoices.MORESOON),
    };

    public class ChickenKiller
    {
        public ChickenKiller(string pname, int count)
        {
            Pname = pname;
            Count = count;
        }

        public string Pname { get; set; } = "";
        public int Count { get; set; } = 0;
    }
}