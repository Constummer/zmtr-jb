using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

internal static class JailbreakExtrasDatabaseExtension
{
    internal static object GetDbValue(this string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return DBNull.Value;
        return data;
    }

    internal static object GetDbValue(this int data) => GetDbValue((int?)data);

    internal static object GetDbValue(this int? data)
    {
        if (data.HasValue == false)
            return DBNull.Value;
        return data;
    }

    internal static object GetDbValue(this long data) => GetDbValue((long?)data);

    internal static object GetDbValue(this long? data)
    {
        if (data.HasValue == false)
            return DBNull.Value;
        return data;
    }

    internal static object GetDbValue(this CsTeam? data)
    {
        if (data.HasValue == false)
            return DBNull.Value;
        switch (data.Value)
        {
            case CsTeam.Terrorist:
                return (int)CsTeam.Terrorist;

            case CsTeam.CounterTerrorist:
                return (int)CsTeam.CounterTerrorist;

            default:
                return (int)CsTeam.None;
        }
    }
}