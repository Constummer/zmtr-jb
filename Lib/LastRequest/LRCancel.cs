using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LrCancel()
    {
        LrActive = false;
        ActivatedLr = null;
    }
}