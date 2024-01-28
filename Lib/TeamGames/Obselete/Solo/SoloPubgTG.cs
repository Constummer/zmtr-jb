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
            RemoveAllWeapons(giveKnife: true);
            var points = GetTpPoints();
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
            if (_Config.TgGame.PubgCoords.TryGetValue(Server.MapName, out var coordsList))
            {
                // Make a copy of list1 to avoid modifying the original list
                List<ulong> remainingElements = new List<ulong>(PlayerCount);

                // Shuffle the remainingElements list to randomize the order
                remainingElements = ShuffleList(remainingElements);

                // Pair each element from list2 with a random element from remainingElements
                Dictionary<ulong, VectorTemp>? pairings = PairLists(coordsList, remainingElements);
                if (pairings == null) return null;
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

        private static Dictionary<ulong, VectorTemp>? PairLists(List<CoordinateTemplate>? list2, List<ulong> list1)
        {
            if (list2 == null) return null;
            Dictionary<ulong, VectorTemp> pairings = new Dictionary<ulong, VectorTemp>();

            for (int i = 0; i < list2.Count; i++)
            {
                if (i < list1.Count)
                {
                    pairings.Add(list1[i], list2[i]?.Coords ?? new(0, 0, 0));
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