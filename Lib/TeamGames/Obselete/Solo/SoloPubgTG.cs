using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class SoloPubgTG : TeamGamesGameBase
    {
        public static List<ulong> PlayerCount { get; set; } = new();

        public SoloPubgTG() : base(TeamGamesSoloChoices.Pubg)
        {
        }

        internal override void StartGame(Action callback)
        {
            RemoveAllWeapons(true);
            var points = GetTpPoints();
            if (points == null)
            {
                Server.PrintToChatAll($"{Prefix} Pubg oyunu başlatılamıyor");
                return;
            }
            GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive)
                .ToList()
                .ForEach(x =>
                {
                    if (points.TryGetValue(x.SteamID, out var tpValue))
                    {
                        if (ValidateCallerPlayer(x, false) == false) return;
                        Global?.AddTimer(0.5f, () =>
                        {
                            if (ValidateCallerPlayer(x, false) == false) return;
                            x.PlayerPawn.Value.Teleport(new Vector(tpValue.X, tpValue.Y, tpValue.Z), x.PlayerPawn.Value.EyeAngles, VEC_ZERO);
                        }, SOM);
                    }
                });
            base.StartGame(callback);
        }

        internal override void Clear(bool printMsg)
        {
            PlayerCount = new();
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

        private static Dictionary<ulong, VectorTemp>? GetTpPoints()
        {
            PlayerCount = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive).Select(x => x.SteamID).ToList();
            if (_Config.Pubg.PubgCoords.TryGetValue(Server.MapName, out var coordsList))
            {
                if (coordsList == null) return null;
                if (coordsList.Count == 0) return null;
                if (coordsList.Count < PlayerCount.Count) return null;

                // Make a copy of list1 to avoid modifying the original list
                List<VectorTemp> remainingElements = new List<VectorTemp>(coordsList);

                // Shuffle the remainingElements list to randomize the order
                remainingElements = ShuffleList(remainingElements);

                // Pair each element from list2 with a random element from remainingElements
                Dictionary<ulong, VectorTemp>? pairings = PairLists(remainingElements, PlayerCount);
                return pairings;
            }
            return null;
        }

        private static List<T> ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        private static Dictionary<ulong, VectorTemp>? PairLists(List<VectorTemp>? coords, List<ulong> players)
        {
            if (coords == null) return null;
            Dictionary<ulong, VectorTemp> pairings = new Dictionary<ulong, VectorTemp>();

            for (int i = 0; i < players.Count; i++)
            {
                if (i < coords.Count)
                {
                    pairings.Add(players[i], coords[i] ?? new(0, 0, 0));
                }
                else
                {
                    return null;
                }
            }

            return pairings;
        }
    }
}