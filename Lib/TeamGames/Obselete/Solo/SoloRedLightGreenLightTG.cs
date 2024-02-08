using CounterStrikeSharp.API.Core;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SoloRedLightGreenLightTG : TeamGamesGameBase
    {
        public CounterStrikeSharp.API.Modules.Timers.Timer? LightActionTimer { get; set; } = null;
        public CounterStrikeSharp.API.Modules.Timers.Timer? ChangeLightTimer { get; set; } = null;
        public GLRLBase CurrentLight { get; set; } = null;
        public readonly List<GLRLBase> GLRLDatas = new()
        {
            new GLRLBase(true,"...",3),
            new GLRLBase(true,"...",1),
            new GLRLBase(true,"...",5),
            new GLRLBase(false,"...",5),
        };
        public Dictionary<ulong, GLRLCoords> Coords { get; set; }
        public List<ulong> PlayerCount { get; set; } = new();
        public const float Treshold = 25 * 25;
        public bool MusicPlaying { get; set; } = false;
        public SoloRedLightGreenLightTG() : base(TeamGamesSoloChoices.RedLightGreenLight)
        {
        }


        internal override void StartGame(Action callback)
        {
            Coords = new();
            CurrentLight = null;
            MusicPlaying = false;
            PlayerCount = RemoveAllWeapons(giveKnife: false);
            GameTimerStart();
            PrintToCenterAll("Green Light - Hareket et, Red Light - DUR!");
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            CurrentLight = null;
            PlayerCount = new();
            ChangeLightTimer?.Kill();
            ChangeLightTimer = null;
            LightActionTimer?.Kill();
            LightActionTimer = null;
            MusicPlaying = false;
            Coords = null;
            RemoveAllWeapons(true);
            base.Clear(printMsg);
        }

        internal override void EventPlayerDeath(EventPlayerDeath @event)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Attacker, false) == false) return;

            if (ValidateCallerPlayer(@event.Userid, false) == false) return;
            SoloCheckGameFinished(this, @event.Userid.SteamID, PlayerCount, @event.Attacker.PlayerName);

            base.EventPlayerDeath(@event);
        }

        internal override void EventPlayerDisconnect(ulong? tempSteamId)
        {
            if (tempSteamId == null) return;
            SoloCheckGameFinished(this, tempSteamId.Value, PlayerCount, null);

            base.EventPlayerDisconnect(tempSteamId);
        }
        private void GameTimerStart()
        {
            LightActionTimer?.Kill();
            int waiter = 0;
            Coords = new();
            MusicPlaying = false;

            LightActionTimer = Global?.AddTimer(1, () =>
            {
                if (CurrentLight == null)
                {
                    CurrentLight = GLRLDatas
                                .Where(x => x.IsRed == false)
                                .Skip(_random.Next(GLRLDatas
                                              .Where(x => x.IsRed == false)
                                              .Count()))
                                .FirstOrDefault()!;
                    MusicPlaying = false;
                }
                if (CurrentLight.Time <= waiter)
                {
                    var filtered = GLRLDatas.Where(x => x.IsRed != CurrentLight.IsRed);
                    CurrentLight = filtered
                                    .Skip(_random.Next(filtered.Count()))
                                    .FirstOrDefault()!;
                    MusicPlaying = false;
                    waiter = 0;
                }
                else
                {
                    waiter++;
                }
            });
            ChangeLightTimer?.Kill();

            ChangeLightTimer = Global?.AddTimer(1, () =>
            {
                if (CurrentLight == null)
                {
                    CurrentLight = GLRLDatas
                             .Where(x => x.IsRed == false)
                             .Skip(_random.Next(GLRLDatas
                                           .Where(x => x.IsRed == false)
                                           .Count()))
                             .FirstOrDefault()!;
                    MusicPlaying = false;
                }
                if (MusicPlaying == false)
                {
                    MusicPlaying = true;
                    PlayGivenMusic(CurrentLight.MusicName);
                }
                if (CurrentLight.IsRed)
                {
                    PrintToCenterHtmlAll("<font color='#FF0000'>Red Light - Kırmızı Işık</font>");
                    PrintToCenterAll("Red Light - Kırmızı Işık");
                    KillOnRedLightMove();
                }
                else
                {
                    PrintToCenterHtmlAll("<font color='#00FF00'>Green Light - Yeşil Işık</font>");
                    PrintToCenterAll("Green Light - Yeşil Işık");
                    GetPlayerXYZAndEyeCoords();
                }
            }, Full);
        }

        private void KillOnRedLightMove()
        {
            var players = GetPlayers().Where(x => x.PawnIsAlive && PlayerCount.Contains(x.SteamID));
            foreach (var x in players)
            {
                if (ValidateCallerPlayer(x, false) == false) continue;
                var abs = x.PlayerPawn.Value.AbsOrigin.LengthSqr();
                if (ValidateCallerPlayer(x, false) == false) continue;
                var eyes = x.PlayerPawn.Value.EyeAngles;
                var eyeSqr = eyes.X * eyes.X + eyes.Y * eyes.Y + eyes.Z * eyes.Z;
                if (Coords.TryGetValue(x.SteamID, out var value))
                {
                    float differenceAbs = Math.Abs(abs - value.Abs);
                    float differenceEyes = Math.Abs(eyeSqr - value.Eyes);

                    if (differenceAbs > Treshold || differenceEyes > Treshold)
                    {
                        x.CommitSuicide(false, false);
                    }
                }
            }
        }

        private void GetPlayerXYZAndEyeCoords()
        {
            Coords = new();
            var players = GetPlayers().Where(x => x.PawnIsAlive && PlayerCount.Contains(x.SteamID));
            foreach (var x in players)
            {
                if (ValidateCallerPlayer(x, false) == false) continue;
                var abs = x.PlayerPawn.Value.AbsOrigin.LengthSqr();
                if (ValidateCallerPlayer(x, false) == false) continue;
                var eyes = x.PlayerPawn.Value.EyeAngles;
                var eyeSqr = eyes.X * eyes.X + eyes.Y * eyes.Y + eyes.Z * eyes.Z;
                Coords.Add(x.SteamID, new(abs, eyeSqr));
            }
        }
        public class GLRLCoords
        {
            public GLRLCoords(float abs, float eyes)
            {
                Abs = abs;
                Eyes = eyes;
            }

            public float Abs { get; set; }
            public float Eyes { get; set; }
        }
        public class GLRLBase
        {
            public GLRLBase(bool isRed, string musicName, decimal time)
            {
                IsRed = isRed;
                MusicName = musicName;
                Time = time;
            }

            public bool IsRed { get; set; }
            public string MusicName { get; set; }
            public decimal Time { get; set; }
        }
    }
}
