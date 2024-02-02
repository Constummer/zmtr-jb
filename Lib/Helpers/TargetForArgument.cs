namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal enum TargetForArgument
    {
        None = 0,
        All,
        T,
        Ct,
        Alive,
        Random,
        RandomT,
        RandomCt,
        Me,
        Dead,
        UserIdIndex,
        Aim,
        SingleUser = Me | Dead | UserIdIndex | Aim | None
    }
}