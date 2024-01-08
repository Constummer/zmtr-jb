namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool IsGameBannedToday() => DateTime.UtcNow.AddHours(3).DayOfWeek == DayOfWeek.Friday;
}