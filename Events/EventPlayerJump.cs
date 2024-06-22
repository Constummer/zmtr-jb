using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void EventPlayerJump()
    {
        RegisterEventHandler((GameEventHandler<EventPlayerJump>)((@event, _) =>
        {
            try
            {
                if (@event != null)
                {
                    if (ValidateCallerPlayer(@event.Userid, false))
                    {
                        if ((@event.Userid.Buttons & PlayerButtons.Jump) != 0)
                        {
                            SaveBattlePasssesJumpDataToCache(@event);
                            if (JumpCountActive)
                            {
                                if (JumpCount.TryGetValue(@event.Userid.SteamID, out var val))
                                {
                                    JumpCount[@event.Userid.SteamID] = new(val.Item1 + 1, val.Item2);
                                }
                                else
                                {
                                    JumpCount.Add(@event.Userid.SteamID, new(1, @event.Userid.PlayerName));
                                }
                            }
                        }
                    }
                }
                return HookResult.Continue;
            }
            catch (Exception e)
            {
                ConsMsg(e.Message);
                return HookResult.Continue;
            }
        }));
    }
}