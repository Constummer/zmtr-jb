﻿namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LrCancel()
    {
        LrActive = false;
        ActivatedLr = null;
        LRTimer?.Kill();
        LRTimer = null;
    }
}