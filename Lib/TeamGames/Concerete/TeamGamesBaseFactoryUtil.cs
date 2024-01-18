namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static TeamGamesGameBase? GetTeamGameBase(TeamGamesSoloChoices soloChoice)
    {
        return soloChoice switch
        {
            TeamGamesSoloChoices.ChickenHunt => new SoloChickenHuntTG(),
            TeamGamesSoloChoices.HotPatato => new SoloHotPatatoTG(),
            TeamGamesSoloChoices.ChickenRoulette => new SoloChickenRouletteTG(),
            TeamGamesSoloChoices.GunFight => new SoloGunFightTG(),
            TeamGamesSoloChoices.HeadShotOnly => new SoloHeadShotOnlyTG(),
            TeamGamesSoloChoices.KnifeFight => new SoloKnifeFightTG(),
            TeamGamesSoloChoices.NoZoom => new SoloNoZoomTG(),
            TeamGamesSoloChoices.PistolZoomBattle => new SoloPistolZoomBattleTG(),
            TeamGamesSoloChoices.ReloadBattle => new SoloReloadBattleTG(),
            TeamGamesSoloChoices.TaserMania => new SoloTaserManiaTG(),
            TeamGamesSoloChoices.WildWest => new SoloWildWestTG(),
            TeamGamesSoloChoices.HeGrenades => new SoloHeGrenadesTG(),
            TeamGamesSoloChoices.Machines500HP => new SoloMachines500HPTG(),
            TeamGamesSoloChoices.CoctailParty => new SoloCoctailPartyTG(),
            TeamGamesSoloChoices.ChickenSlayer => new SoloChickenSlayerTG(),
            TeamGamesSoloChoices.HideFight => new SoloHideFightTG(),
            _ => null
        };
    }

    public static TeamGamesGameBase? GetTeamGameBase(TeamGamesMultiChoices multiChoice)
    {
        return multiChoice switch
        {
            TeamGamesMultiChoices.ChickenHunt => new MultiChickenHuntTG(),
            TeamGamesMultiChoices.ChickenRoulette => new MultiChickenRouletteTG(),
            TeamGamesMultiChoices.GunFight => new MultiGunFightTG(),
            TeamGamesMultiChoices.HeadShotOnly => new MultiHeadShotOnlyTG(),
            TeamGamesMultiChoices.KnifeFight => new MultiKnifeFightTG(),
            TeamGamesMultiChoices.NoZoom => new MultiNoZoomTG(),
            TeamGamesMultiChoices.PistolZoomBattle => new MultiPistolZoomBattleTG(),
            TeamGamesMultiChoices.ReloadBattle => new MultiReloadBattleTG(),
            TeamGamesMultiChoices.TaserMania => new MultiTaserManiaTG(),
            TeamGamesMultiChoices.WeildWest => new MultiWeildWestTG(),
            TeamGamesMultiChoices.HeGrenades => new MultiHeGrenadesTG(),
            TeamGamesMultiChoices.Machines500HP => new MultiMachines500HPTG(),
            TeamGamesMultiChoices.CoctailParty => new MultiCoctailPartyTG(),
            TeamGamesMultiChoices.ChickenSlayer => new MultiChickenSlayerTG(),
            _ => null
        };
    }
}