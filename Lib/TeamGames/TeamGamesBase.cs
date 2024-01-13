using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool TgActive = false;
    public CounterStrikeSharp.API.Modules.Timers.Timer TgTimer { get; set; } = null;
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
        CoctailParty
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
        CoctailParty
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
        new ("Chicken Hunt",      true,  null, TeamGamesSoloChoices.ChickenHunt),
        new ("Hot Patato",        true,  null, TeamGamesSoloChoices.HotPatato),
        new ("Chicken Roulette",  false,  null, TeamGamesSoloChoices.ChickenRoulette),
        new ("Gun Fight",         true,  null, TeamGamesSoloChoices.GunFight),
        new ("HeadShot Only",     false,  null, TeamGamesSoloChoices.HeadShotOnly),
        new ("Knife Fight",       false,  null, TeamGamesSoloChoices.KnifeFight),
        new ("No Zoom",           true,  null, TeamGamesSoloChoices.NoZoom),
        new ("Pistol Zoom Battle",true,  null, TeamGamesSoloChoices.PistolZoomBattle),
        new ("Reload Battle",     true,  null, TeamGamesSoloChoices.ReloadBattle),
        new ("Taser Mania",       true,  null, TeamGamesSoloChoices.TaserMania),
        new ("Weild West",        true,  null, TeamGamesSoloChoices.WildWest),
        new ("He Grenades",       true,  null, TeamGamesSoloChoices.HeGrenades),
        new ("Machines + 500HP",  true,  null, TeamGamesSoloChoices.Machines500HP),
        new ("Coctail Part",      true,  null, TeamGamesSoloChoices.CoctailParty),
    };

    public List<TGBaseClass> MultiTGGamesMenu { get; set; } = new()
    {
        new ("Chiken Hucnt",      true,  TeamGamesMultiChoices.ChickenHunt),
        new ("Chicken Roulette",  false,  TeamGamesMultiChoices.ChickenRoulette),
        new ("Gun Fight",         true,  TeamGamesMultiChoices.GunFight),
        new ("HeadShot Only",     false,  TeamGamesMultiChoices.HeadShotOnly),
        new ("Knife Fight",       false,  TeamGamesMultiChoices.KnifeFight),
        new ("No Zoom",           true,  TeamGamesMultiChoices.NoZoom),
        new ("Pistol Zoom Battle",true,  TeamGamesMultiChoices.PistolZoomBattle),
        new ("Reload Battle",     true,  TeamGamesMultiChoices.ReloadBattle),
        new ("Taser Mania",       true,  TeamGamesMultiChoices.TaserMania),
        new ("Weild West",        true,  TeamGamesMultiChoices.WeildWest),
        new ("He Grenades",       true,  TeamGamesMultiChoices.HeGrenades),
        new ("Machines + 500HP",  true,  TeamGamesMultiChoices.Machines500HP),
        new ("Coctail Party",     true,  TeamGamesMultiChoices.CoctailParty)
    };
}