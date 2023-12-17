using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class Block
    {
        [RequiresPermissions("@jail/debug")]
        public void is_blocked(CCSPlayerController? invoke, CommandInfo command)
        {
            invoke.announce(Debug.DEBUG_PREFIX, $"Block state {block_state} : {Lib.block_enabled()}");
        }

        public void block_all()
        {
            if (!Lib.block_enabled())
            {
                Lib.announce(Warden.WARDEN_PREFIX, "Block enabled");
                Lib.block_all();
                block_state = true;
            }
        }

        public void unblock_all()
        {
            if (Lib.block_enabled())
            {
                Lib.announce(Warden.WARDEN_PREFIX, "No block enabled");
                Lib.unblock_all();
                block_state = false;
            }
        }

        public void round_start()
        {
            // TODO: for now we just assume no block
            // we wont have a cvar
            unblock_all();
        }

        private bool block_state = false;
    }
}