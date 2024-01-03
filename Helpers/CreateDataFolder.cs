namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static string ContentRootPath = "";

    internal void CreateDataFolder()
    {
        var contentRootPath = ModulePath.Replace(
            ModulePath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }).Last(), "datas");

        if (!Directory.Exists(contentRootPath))
        {
            Directory.CreateDirectory(contentRootPath);
        }
        ContentRootPath = contentRootPath;
    }
}