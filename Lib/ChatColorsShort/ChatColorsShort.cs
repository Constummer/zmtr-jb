using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class CC
    {
        public static char D => ChatColors.Default;
        public static char W => ChatColors.White;
        public static char DR => ChatColors.DarkRed;
        public static char G => ChatColors.Green;
        public static char LY => ChatColors.LightYellow;
        public static char LB => ChatColors.LightBlue;
        public static char Ol => ChatColors.Olive;
        public static char L => ChatColors.Lime;
        public static char R => ChatColors.Red;
        public static char LP => ChatColors.LightPurple;
        public static char P => ChatColors.Purple;
        public static char Gr => ChatColors.Grey;
        public static char Y => ChatColors.Yellow;
        public static char Go => ChatColors.Gold;
        public static char S => ChatColors.Silver;
        public static char B => ChatColors.Blue;
        public static char DB => ChatColors.DarkBlue;
        public static char BG => ChatColors.BlueGrey;
        public static char M => ChatColors.Magenta;
        public static char LR => ChatColors.LightRed;
        public static char Or => ChatColors.Orange;
    }

    public static readonly Dictionary<string, char> ChatColorsData = new(StringComparer.InvariantCultureIgnoreCase)
    {
        {"D"    ,CC.D },
        {"W"    ,CC.W},
        {"DR"   ,CC.DR},
        {"G"    ,CC.G},
        {"LY"   ,CC.LY},
        {"LB"   ,CC.LB},
        {"Ol"   ,CC.Ol},
        {"L"    ,CC.L},
        {"R"    ,CC.R},
        {"LP"   ,CC.LP},
        {"P"    ,CC.P},
        {"Gr"   ,CC.Gr},
        {"Y"    ,CC.Y},
        {"Go"   ,CC.Go},
        {"S"    ,CC.S},
        {"B"    ,CC.B},
        {"DB"   ,CC.DB},
        {"BG"   ,CC.BG},
        {"M"    ,CC.M},
        {"LR"   ,CC.LR},
        {"Or"   ,CC.Or},
    };

    private static char GetChatColor(string cName)
    {
        if (ChatColorsData.TryGetValue(cName, out var res) == false)
        {
            return CC.W;
        }
        return res;
    }
}