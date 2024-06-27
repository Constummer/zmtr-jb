﻿using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public void AllEvents()
    {
        RegisterEventHandler<EventAchievementEarned>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAchievementEarnedLocal>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAchievementEvent>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAchievementInfoLoaded>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAchievementWriteFailed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAddBulletHitMarker>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAddPlayerSonarIcon>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAmmoPickup>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAmmoRefill>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventAnnouncePhaseEnd>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBeginNewMatch>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombAbortdefuse>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombAbortplant>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombBeep>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombBegindefuse>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombBeginplant>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombDefused>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombDropped>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombExploded>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombPickup>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBombPlanted>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBonusUpdated>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBotTakeover>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBreakBreakable>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBreakProp>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBrokenBreakable>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBulletFlightResolution>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBulletImpact>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBuymenuClose>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBuymenuOpen>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventBuytimeEnded>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCartUpdated>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventChoppersIncomingWarning>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventClientDisconnect>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventClientLoadoutChanged>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventClientsideLessonClosed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventClientsideReloadCustomEcon>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsGameDisconnected>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsIntermission>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsMatchEndRestart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsPreRestart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsPrevNextSpectator>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsRoundFinalBeep>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsRoundStartBeep>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsWinPanelMatch>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventCsWinPanelRound>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDecoyDetonate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDecoyFiring>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDecoyStarted>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDefuserDropped>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDefuserPickup>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDemoSkip>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDemoStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDemoStop>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDifficultyChanged>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDmBonusWeaponStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDoorBreak>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDoorClose>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDoorClosed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDoorMoving>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDoorOpen>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDroneAboveRoof>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDroneCargoDetached>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDroneDispatched>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDronegunAttack>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDropRateModified>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDynamicShadowLightChanged>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventDzItemInteraction>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEnableRestartVoting>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEndmatchCmmStartRevealItems>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEndmatchMapvoteSelectingMap>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEnterBombzone>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEnterBuyzone>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEnterRescueZone>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEntityKilled>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEntityVisible>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventEventTicketModified>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventExitBombzone>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventExitBuyzone>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventExitRescueZone>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventFinaleStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventFirstbombsIncomingWarning>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventFlareIgniteNpc>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventFlashbangDetonate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGameEnd>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGameInit>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGameinstructorDraw>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGameinstructorNodraw>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGameMessage>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGameNewmap>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGamePhaseChanged>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGameStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGameuiHidden>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGcConnected>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGgKilledEnemy>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGrenadeBounce>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGrenadeThrown>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventGuardianWaveRestart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHegrenadeDetonate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHelicopterGrenadePuntMiss>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHideDeathpanel>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvCameraman>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvChangedMode>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvChase>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvChat>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvFixed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvMessage>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvRankCamera>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvRankEntity>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvReplay>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvReplayStatus>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvStatus>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvTitle>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHltvVersioninfo>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHostageCallForHelp>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHostageFollows>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHostageHurt>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHostageKilled>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHostageRescued>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHostageRescuedAll>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHostageStopsFollowing>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventHostnameChanged>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInfernoExpire>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInfernoExtinguish>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInfernoStartburn>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInspectWeapon>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInstructorCloseLesson>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInstructorServerHintCreate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInstructorServerHintStop>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInstructorStartLesson>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventInventoryUpdated>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventItemEquip>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventItemPickup>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventItemPickupFailed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventItemPickupSlerp>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventItemPurchase>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventItemRemove>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventItemSchemaInitialized>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventJointeamFailed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventLocalPlayerControllerTeam>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventLocalPlayerPawnChanged>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventLocalPlayerTeam>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventLootCrateOpened>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventLootCrateVisible>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventMapShutdown>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventMapTransition>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventMatchEndConditions>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventMaterialDefaultComplete>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventMbInputLockCancel>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventMbInputLockSuccess>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventMolotovDetonate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventNavBlocked>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventNavGenerate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventNextlevelChanged>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventOpenCrateInstr>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventOtherDeath>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventParachuteDeploy>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventParachutePickup>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPhysgunPickup>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerActivate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerAvengedTeammate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerBlind>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerChangename>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerChat>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerConnect>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerConnectFull>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerDeath>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerDecal>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerDisconnect>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerFalldamage>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerFootstep>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerFullUpdate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerGivenC4>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerHintmessage>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerHurt>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerInfo>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerJump>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerPing>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerPingStop>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerRadio>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerResetVote>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerScore>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerShoot>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerSound>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerSpawn>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerSpawned>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerStatsUpdated>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventPlayerTeam>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRagdollDissolved>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventReadGameTitledata>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRepostXboxAchievements>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventResetGameTitledata>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundAnnounceFinal>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundAnnounceLastRoundHalf>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundAnnounceMatchPoint>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundAnnounceMatchStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundAnnounceWarmup>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundEnd>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundEndUploadStats>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundFreezeEnd>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundMvp>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundOfficiallyEnded>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundPoststart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundPrestart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundStartPostNav>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundStartPreEntity>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventRoundTimeWarning>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSeasoncoinLevelup>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventServerCvar>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventServerMessage>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventServerPreShutdown>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventServerShutdown>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventServerSpawn>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSetInstructorGroupEnabled>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSfuievent>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventShowDeathpanel>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventShowSurvivalRespawnStatus>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSilencerDetach>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSilencerOff>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSilencerOn>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSmokeBeaconParadrop>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSmokegrenadeDetonate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSmokegrenadeExpired>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSpecModeUpdated>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSpecTargetUpdated>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventStartHalftime>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventStartVote>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventStorePricesheetUpdated>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSurvivalAnnouncePhase>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSurvivalNoRespawnsFinal>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSurvivalNoRespawnsWarning>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSurvivalParadropBreak>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSurvivalParadropSpawn>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSurvivalTeammateRespawn>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventSwitchTeam>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTagrenadeDetonate>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTeamchangePending>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTeamInfo>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTeamIntroEnd>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTeamIntroStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTeamplayBroadcastAudio>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTeamplayRoundStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTeamScore>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTournamentReward>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventTrialTimeExpired>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventUgcFileDownloadFinished>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventUgcFileDownloadStart>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventUgcMapDownloadError>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventUgcMapInfoReceived>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventUgcMapUnsubscribed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventUpdateMatchmakingStats>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventUserDataDownloaded>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVipEscaped>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVipKilled>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVoteCast>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVoteCastNo>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVoteCastYes>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVoteChanged>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVoteEnded>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVoteFailed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVoteOptions>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVotePassed>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventVoteStarted>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWarmupEnd>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWeaponFire>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWeaponFireOnEmpty>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWeaponhudSelection>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWeaponReload>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWeaponZoom>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWeaponZoomRifle>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWriteGameTitledata>((@event, _) => HookResult.Continue);
        RegisterEventHandler<EventWriteProfileData>((@event, _) => HookResult.Continue);
    }
}