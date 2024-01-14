using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class MultiCoctailPartyTG : TeamGamesGameBase
    {
        public MultiCoctailPartyTG() : base(TeamGamesMultiChoices.CoctailParty)
        {
        }

        internal override void StartGame(Action callback)
        {
            GetPlayers(CsTeam.Terrorist)
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                RemoveWeapons(x, false);
            });
            Server.PrintToChatAll("1");
            Global?.SinirsizXAction(null, "@t", "incgrenade");
            Server.PrintToChatAll("2");
            base.StartGame(callback);
        }

        internal override void Clear()
        {
            Global?.SinirsizXKapaAction("@t", "");
            GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               if (ValidateCallerPlayer(x, false) == false) return;
               RemoveWeapons(x, true);
           });
            base.Clear();
        }
    }
}