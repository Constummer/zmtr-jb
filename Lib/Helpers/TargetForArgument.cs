namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [Flags]
    internal enum TargetForArgument
    {
        None = 0,
        All = 1 << 0,
        T = 1 << 1,
        Ct = 1 << 2,
        Alive = 1 << 3,
        Random = 1 << 4,
        RandomT = 1 << 5,
        RandomCt = 1 << 6,
        Me = 1 << 7,
        Dead = 1 << 8,
        UserIdIndex = 1 << 9,
        Aim = 1 << 10,
        Sut = 1 << 11,
        SingleUser = Me | UserIdIndex | Aim | None
    }
}