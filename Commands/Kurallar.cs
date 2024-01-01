using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

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

        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}             --------------------------------------------");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}               ________  __       __  ________  _______  ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}              /        |/  \     /  |/        |/       \ ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}              $$$$$$$$/ $$  \   /$$ |$$$$$$$$/ $$$$$$$  |");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}                  /$$/  $$$  \ /$$$ |   $$ |   $$ |__$$ |");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}                 /$$/   $$$$  /$$$$ |   $$ |   $$    $$< ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}                /$$/    $$ $$ $$/$$ |   $$ |   $$$$$$$  |");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}               /$$/____ $$ |$$$/ $$ |   $$ |   $$ |  $$ |");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}              /$$      |$$ | $/  $$ |   $$ |   $$ |  $$ |");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}              $$$$$$$$/ $$/      $$/    $$/    $$/   $$/ ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}      ---------------------------------------------------------");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}                                 KURALLAR");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}      ---------------------------------------------------------");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  * Oyuncu Kuralları");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Sunucumuzda gerek saygı gerek dostluk ortamı açısından küfür, argo, ırkçılık, din veya siyaset yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçu anlık durumlarda diğer oyuncuların mikrofonunu açabilir. Onun haricinde Komutçu harici mikrofon basmak yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Spam yapmak, flood yapmak yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Sunucumuzda reklam yapmak yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Sunucumuzda veya CS2’de herhangi 3. parti program kullanmak, hile kullanmak yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutcu af çıkardığında can almamışsa veya CT’den bir oyuncu revlememişse isyan hakkınız yoktur.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçu veya korumanın ölmesi veya canı gittiği zaman ve dışarda hala bir kaçağın bulunmasına rağmen af vermesi durumunda o el isyandan sayılır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçunun veya korumanın canı azalmadığı ama hala kaçağın bulunması durumunda af vermesi isyan sayılmaz.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçu veya koruma af çıkarılmadan kendi canlarını dolduramazlar.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Sunucumuzda skin takibi yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} * Seviye sistemine kayıtlı oyuncuların");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Steam Grubuna katılmaları zorunludur.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Gereksiz yetki kullanmak yasaktır.(Kendi mute ve gag’ınızı kaldırmak, komutçudan izinsiz veya habersiz bir şekilde herhangi bir admin komutunu kullanmak.)");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} * Komutçu Kuralları");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - +18 yaş zorunluluğu");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Hücre içi don vermek yasaktır. Hücre kapısı açılıp kapandıktan sonra verilen don bozulur.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçu herhangi bi’ oyuncudan kredi alarak, oyuncuya ek avantaj sağlayamaz.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Hard komut vermek yasaktır. GOD MODE kullanılıp eğlence için verilebilir.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Özel koruma olarak sadece MUHAFIZ tag’lı oyuncu alınabilir.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - 3 raundda bir GOD MODE kullanabilir.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçuların Steam Grubuna katılmaları zorunludur.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Hücre içinde en fazla 2 infaz verebilir. (20 Kişi ve Üstü durumlarda bu sayı artabilir.)");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Korumalar 8-16-24-32-40-48 kişide 1 alınır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Map değiş kal oylamasını ve map oylamasını komutcu yapar.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Map değiştirme görevi komutçuya aittir.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçuların oyunda aktif oldukları sürece, DISCORD’da bulunmaları zorunludur.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - El başında veya isyan sırasında hook veya grap kullanmak yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçu isyan varken af çıkaramaz ve hiçbir komutu kullanamaz.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Komutçu af çıkarmadan can dolduramaz veya veremez. (Kill çekip kendini revleyemez.)");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} * Admin/Yetkili Kuralları");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Sunucu adminlerimiz: Sunucumuzda gerek saygı gerek dostluk ortamı açısından sizler örnek teşkil etmelisiniz ki oyuncularımız oyunu zevkle oyanamalı. ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}         Bu yüzden siz adminlerimizin uyması gereken bazı kurallar var. Bu kuralları sizin benimsemenizi temenni ederiz.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Lüzumsuz ve gereksiz yere ban-kick-gag-mute atımı yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Gereksiz hook,grap,rope ve diğer komutlar (beacon,drug vb.) komutlar isyanda veya komutcu izni olmadan oyunlarda yapılmamalıdır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Map ve komutcu oylaması komutçu tarafından yapılmaktadır . Eğer komutcu yoksa standart haritalar oylamaya konulmalıdır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Adminler kafasına göre ve rastgele map değiştiremez oylama yapmak zorundadır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Adminler istediği gibi T&CT’lerle oynayamaz.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Adminlerin Steam Grubuna katılmaları zorunludur.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Adminlerin DISCORD sunucumuzda aktif oldukları sürece bulunmaları zorunludur.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Adminler komutçu izni olmadan kendini revleyemez.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Sunucuda yetkili dahi olsanız oyun sırasında en yetkili kişi Komutçudur.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - FF başlamadan önce herhangi bir admin komutuyla normal şekilde ulaşılamayacak yerlere çıkmak yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - FF sırasında weapon, bring, noclip, hp, god, hook, grab ve diğer admin komutları yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - FF zamanında “her şeyin serbest olduğu” söylenilse dahi komut kullanmak yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}  - Saklambaç sırasında noclip, gel ve git komutlarını kullanmak yasaktır.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} ⚠️ Hesap değişikliği;");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} Buradaki yetkileriniz tek STEAM ID üzerine kayıtlıdır. Hesap değişmeniz, VAC ban yemeniz veya başka bir hesaba geçiş yapmak isterseniz yapılmaz. ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}             Krediniz, market eşyalarınız veya yetkiniz başka bir hesaba aktarılmaz.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White} ✔️ Sunucuda oynayan tüm oyuncular bütün kuralları okuyup, kabul etmiş sayılır. ");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}             Kurallara uymayan oyuncuların yetkisi düşürülür veya alınır. Bütün kurallar yukarıda ki bölümde listelidir ve ceza hususunda çekimserlik yapılmaz.");
        player.PrintToConsole(@$"{ChatColors.Green}[ZMTR]{ChatColors.White}             Kurallar haricinde “kötü/art niyetli” yapılan tüm davranışlardan ceza alırsınız. Bu kuralları sizin de benimsemenizi temenni ederiz.");
        player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Konsolunuzu kontrol ediniz.");
    }

    #endregion Kurallar
}