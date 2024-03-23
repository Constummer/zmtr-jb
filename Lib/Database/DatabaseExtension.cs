using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras.Lib.Database;

internal static class JailbreakExtrasDatabaseExtension
{
    internal static object GetDbValue(this string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return DBNull.Value;
        return data;
    }

    internal static object GetDbValue(this int data) => ((int?)data).GetDbValue();

    internal static object GetDbValue(this int? data)
    {
        if (data.HasValue == false)
            return DBNull.Value;
        return data;
    }

    internal static int GetDbValue<T>(this T enumValue) where T : Enum
    {
        try
        {
            return Convert.ToInt32(enumValue);
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    internal static object GetDbValue(this long data) => ((long?)data).GetDbValue();

    internal static object GetDbValue(this long? data)
    {
        if (data.HasValue == false)
            return DBNull.Value;
        return data;
    }

    internal static object GetDbValue(this ulong data) => ((ulong?)data).GetDbValue();

    internal static object GetDbValue(this ulong? data)
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