using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Kurallar

    [ConsoleCommand("kurallar")]
    public void Kurallar(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        var prefix = $"{CC.G}[ZMTR]{CC.W}";
        player.PrintToConsole(@$"{prefix}             --------------------------------------------");
        player.PrintToConsole(@$"{prefix}");
        player.PrintToConsole(@$"{prefix}               ________  __       __  ________  _______  ");
        player.PrintToConsole(@$"{prefix}              /        |/  \     /  |/        |/       \ ");
        player.PrintToConsole(@$"{prefix}              $$$$$$$$/ $$  \   /$$ |$$$$$$$$/ $$$$$$$  |");
        player.PrintToConsole(@$"{prefix}                  /$$/  $$$  \ /$$$ |   $$ |   $$ |__$$ |");
        player.PrintToConsole(@$"{prefix}                 /$$/   $$$$  /$$$$ |   $$ |   $$    $$< ");
        player.PrintToConsole(@$"{prefix}                /$$/    $$ $$ $$/$$ |   $$ |   $$$$$$$  |");
        player.PrintToConsole(@$"{prefix}               /$$/____ $$ |$$$/ $$ |   $$ |   $$ |  $$ |");
        player.PrintToConsole(@$"{prefix}              /$$      |$$ | $/  $$ |   $$ |   $$ |  $$ |");
        player.PrintToConsole(@$"{prefix}              $$$$$$$$/ $$/      $$/    $$/    $$/   $$/ ");
        player.PrintToConsole(@$"{prefix}");
        player.PrintToConsole(@$"{prefix}");
        player.PrintToConsole(@$"{prefix}      ---------------------------------------------------------");
        player.PrintToConsole(@$"{prefix}                                 KURALLAR");
        player.PrintToConsole(@$"{prefix}      ---------------------------------------------------------");
        player.PrintToConsole(@$"{prefix}");
        player.PrintToConsole(@$"{prefix}  * Oyuncu Kuralları");
        player.PrintToConsole(@$"{prefix}  - Sunucumuzda gerek saygı gerek dostluk ortamı açısından küfür, argo, ırkçılık, din veya siyaset yasaktır.");
        player.PrintToConsole(@$"{prefix}  - Komutçu anlık durumlarda diğer oyuncuların mikrofonunu açabilir. Onun haricinde Komutçu harici mikrofon basmak yasaktır.");
        player.PrintToConsole(@$"{prefix}  - Spam yapmak, flood yapmak yasaktır.");
        player.PrintToConsole(@$"{prefix}  - Sunucumuzda reklam yapmak yasaktır.");
        player.PrintToConsole(@$"{prefix}  - Sunucumuzda veya CS2’de herhangi 3. parti program kullanmak, hile kullanmak yasaktır.");
        player.PrintToConsole(@$"{prefix}  - Komutcu af çıkardığında can almamışsa veya CT’den bir oyuncu revlememişse isyan hakkınız yoktur.");
        player.PrintToConsole(@$"{prefix}  - Komutçu veya korumanın ölmesi veya canı gittiği zaman ve dışarda hala bir kaçağın bulunmasına rağmen af vermesi durumunda o el isyandan sayılır.");
        player.PrintToConsole(@$"{prefix}  - Komutçunun veya korumanın canı azalmadığı ama hala kaçağın bulunması durumunda af vermesi isyan sayılmaz.");
        player.PrintToConsole(@$"{prefix}  - Komutçu veya koruma af çıkarılmadan kendi canlarını dolduramazlar.");
        player.PrintToConsole(@$"{prefix}  - Sunucumuzda skin takibi yasaktır.");
        player.PrintToConsole(@$"{prefix} ");
        player.PrintToConsole(@$"{prefix} * Seviye sistemine kayıtlı oyuncuların");
        player.PrintToConsole(@$"{prefix}  - Steam Grubuna katılmaları zorunludur.");
        player.PrintToConsole(@$"{prefix}  - Gereksiz yetki kullanmak yasaktır.(Kendi mute ve gag’ınızı kaldırmak, komutçudan izinsiz veya habersiz bir şekilde herhangi bir admin komutunu kullanmak.)");
        player.PrintToConsole(@$"{prefix} ");
        player.PrintToConsole(@$"{prefix} * Komutçu Kuralları");
        player.PrintToConsole(@$"{prefix}  - +18 yaş zorunluluğu");
        player.PrintToConsole(@$"{prefix}  - Hücre içi don vermek yasaktır. Hücre kapısı açılıp kapandıktan sonra verilen don bozulur.");
        player.PrintToConsole(@$"{prefix}  - Komutçu herhangi bi’ oyuncudan kredi alarak, oyuncuya ek avantaj sağlayamaz.");
        player.PrintToConsole(@$"{prefix}  - Hard komut vermek yasaktır. GOD MODE kullanılıp eğlence için verilebilir.");
        player.PrintToConsole(@$"{prefix}  - Özel koruma olarak sadece MUHAFIZ tag’lı oyuncu alınabilir.");
        player.PrintToConsole(@$"{prefix}  - 3 raundda bir GOD MODE kullanabilir.");
        player.PrintToConsole(@$"{prefix}  - Komutçuların Steam Grubuna katılmaları zorunludur.");
        player.PrintToConsole(@$"{prefix}  - Hücre içinde en fazla 2 infaz verebilir. (20 Kişi ve Üstü durumlarda bu sayı artabilir.)");
        player.PrintToConsole(@$"{prefix}  - Korumalar 8-16-24-32-40-48 kişide 1 alınır.");
        player.PrintToConsole(@$"{prefix}  - Map değiş kal oylamasını ve map oylamasını komutcu yapar.");
        player.PrintToConsole(@$"{prefix}  - Map değiştirme görevi komutçuya aittir.");
        player.PrintToConsole(@$"{prefix}  - Komutçuların oyunda aktif oldukları sürece, DISCORD’da bulunmaları zorunludur.");
        player.PrintToConsole(@$"{prefix}  - El başında veya isyan sırasında hook veya grap kullanmak yasaktır.");
        player.PrintToConsole(@$"{prefix}  - Komutçu isyan varken af çıkaramaz ve hiçbir komutu kullanamaz.");
        player.PrintToConsole(@$"{prefix}  - Komutçu af çıkarmadan can dolduramaz veya veremez. (Kill çekip kendini revleyemez.)");
        player.PrintToConsole(@$"{prefix} ");
        player.PrintToConsole(@$"{prefix} * Admin/Yetkili Kuralları");
        player.PrintToConsole(@$"{prefix}  - Sunucu adminlerimiz: Sunucumuzda gerek saygı gerek dostluk ortamı açısından sizler örnek teşkil etmelisiniz ki oyuncularımız oyunu zevkle oyanamalı. ");
        player.PrintToConsole(@$"{prefix}         Bu yüzden siz adminlerimizin uyması gereken bazı kurallar var. Bu kuralları sizin benimsemenizi temenni ederiz.");
        player.PrintToConsole(@$"{prefix}  - Lüzumsuz ve gereksiz yere ban-kick-gag-mute atımı yasaktır.");
        player.PrintToConsole(@$"{prefix}  - Gereksiz hook,grap,rope ve diğer komutlar (beacon,drug vb.) komutlar isyanda veya komutcu izni olmadan oyunlarda yapılmamalıdır.");
        player.PrintToConsole(@$"{prefix}  - Map ve komutcu oylaması komutçu tarafından yapılmaktadır . Eğer komutcu yoksa standart haritalar oylamaya konulmalıdır.");
        player.PrintToConsole(@$"{prefix}  - Adminler kafasına göre ve rastgele map değiştiremez oylama yapmak zorundadır.");
        player.PrintToConsole(@$"{prefix}  - Adminler istediği gibi T&CT’lerle oynayamaz.");
        player.PrintToConsole(@$"{prefix}  - Adminlerin Steam Grubuna katılmaları zorunludur.");
        player.PrintToConsole(@$"{prefix}  - Adminlerin DISCORD sunucumuzda aktif oldukları sürece bulunmaları zorunludur.");
        player.PrintToConsole(@$"{prefix}  - Adminler komutçu izni olmadan kendini revleyemez.");
        player.PrintToConsole(@$"{prefix}  - Sunucuda yetkili dahi olsanız oyun sırasında en yetkili kişi Komutçudur.");
        player.PrintToConsole(@$"{prefix}  - FF başlamadan önce herhangi bir admin komutuyla normal şekilde ulaşılamayacak yerlere çıkmak yasaktır.");
        player.PrintToConsole(@$"{prefix}  - FF sırasında weapon, bring, noclip, hp, god, hook, grab ve diğer admin komutları yasaktır.");
        player.PrintToConsole(@$"{prefix}  - FF zamanında “her şeyin serbest olduğu” söylenilse dahi komut kullanmak yasaktır.");
        player.PrintToConsole(@$"{prefix}  - Saklambaç sırasında noclip, gel ve git komutlarını kullanmak yasaktır.");
        player.PrintToConsole(@$"{prefix} ");
        player.PrintToConsole(@$"{prefix} ⚠️ Hesap değişikliği;");
        player.PrintToConsole(@$"{prefix} Buradaki yetkileriniz tek STEAM ID üzerine kayıtlıdır. Hesap değişmeniz, VAC ban yemeniz veya başka bir hesaba geçiş yapmak isterseniz yapılmaz. ");
        player.PrintToConsole(@$"{prefix}             Krediniz, market eşyalarınız veya yetkiniz başka bir hesaba aktarılmaz.");
        player.PrintToConsole(@$"{prefix} ");
        player.PrintToConsole(@$"{prefix} ✔️ Sunucuda oynayan tüm oyuncular bütün kuralları okuyup, kabul etmiş sayılır. ");
        player.PrintToConsole(@$"{prefix}             Kurallara uymayan oyuncuların yetkisi düşürülür veya alınır. Bütün kurallar yukarıda ki bölümde listelidir ve ceza hususunda çekimserlik yapılmaz.");
        player.PrintToConsole(@$"{prefix}             Kurallar haricinde “kötü/art niyetli” yapılan tüm davranışlardan ceza alırsınız. Bu kuralları sizin de benimsemenizi temenni ederiz.");
        player.PrintToChat($" {prefix}Konsolunuzu kontrol ediniz.");
    }

    #endregion Kurallar
}