using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

//using CounterStrikeSharp.API.Modules.Menu;
using System.Text.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public List<VectorTemp> BulletImpactVectors { get; set; } = new List<VectorTemp>();

    [ConsoleCommand("ceventbulletimpactwithsave")]
    public void ceventbulletimpactwithsave(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        BulletImpactActive = false;
        var data = new
        {
            MapName = Server.MapName,
            Data = BulletImpactVectors
        };
        var serialized = JsonSerializer.Serialize(data,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        });
        var savePath = Path.Combine(ContentRootPath, $"pubg_{Server.MapName}.json");

        File.WriteAllText(savePath, serialized);
        BulletImpactVectors?.Clear();
    }

    [ConsoleCommand("ceventbulletimpact")]
    public void ceventbulletimpact(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        BulletImpactActive = !BulletImpactActive;
        BulletImpactVectors?.Clear();
    }

    private bool BulletImpactActive = false;

    private void DebugBulletImpact(EventBulletImpact @event, GameEventInfo info)
    {
        if (BulletImpactActive)
        {
            if (@event == null) return;
            if (ValidateCallerPlayer(@event.Userid) == false) return;
            if (!AdminManager.PlayerHasPermissions(@event.Userid, "@css/root")) return;
            BulletImpactVectors.Add(new(@event?.X, @event?.Y, @event?.Z));
            var msg = $"{@event?.X},{@event?.Y},{@event?.Z} - {BulletImpactVectors.Count}";
            PrintToRootChat(msg);
            Server.PrintToConsole(msg);
        }
    }
}