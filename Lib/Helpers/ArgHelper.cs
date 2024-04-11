namespace JailbreakExtras;

public static class JailbreakExtrasHelper
{
    public static string GetArg(this string? arg, int argIndex)
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
        var res = argCount.Skip(argIndex).FirstOrDefault();
        return res;
    }

    public static string GetArgSkip(this string? arg, int argIndex)
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
        var res = string.Join(" ", argCount.Skip(argIndex));
        return res;
    }
}