namespace JailbreakExtras;

public static class JailbreakExtrasHelper
{
    public static string GetArg(this string? arg, int argIndex)
    {
        var argCount = GetArgCountWithFormatted(arg);
        if (argCount == null) return null;
        var res = argCount.Skip(argIndex).FirstOrDefault();
        return res;
    }

    private static string[]? GetArgCountWithFormatted(string? arg)
    {
        if (string.IsNullOrWhiteSpace(arg))
        {
            return null;
        }
        var argCount = arg.Split(" ");
        argCount = argCount.Select(x => x.Trim()).Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray();
        if (argCount.Length == 0)
        {
            return null;
        }
        return argCount;
    }

    public static string GetArgSkip(this string? arg, int argIndex)
    {
        var argCount = GetArgCountWithFormatted(arg);
        if (argCount == null) return null;
        var res = string.Join(" ", argCount.Skip(argIndex));
        return res;
    }

    public static string GetArgLast(this string? arg)
    {
        var argCount = GetArgCountWithFormatted(arg);
        if (argCount == null) return null;
        var res = argCount.Last();
        return res;
    }

    public static string GetArgSkipFromLast(this string? arg, int argIndex)
    {
        var argCount = GetArgCountWithFormatted(arg);
        if (argCount == null) return null;
        var res = string.Join(" ", argCount.Reverse<string>().Skip(argIndex).Reverse<string>());
        return res;
    }
}