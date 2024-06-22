/*	if (!Config.Additional.SkinEnabled) return;
var designerName = entity.DesignerName;
if (!weaponList.ContainsKey(designerName)) return;
bool isKnife = false;
var weapon = new CBasePlayerWeapon(entity.Handle);

if (designerName.Contains("knife") || designerName.Contains("bayonet"))
{
    isKnife = true;
}

Server.NextFrame(() =>
{
    try
    {
        if (!weapon.IsValid) return;
        if (weapon.OwnerEntity.Value == null) return;
        if (weapon.OwnerEntity.Index <= 0) return;
        int weaponOwner = (int)weapon.OwnerEntity.Index;
        var pawn = new CBasePlayerPawn(NativeAPI.GetEntityFromIndex(weaponOwner));
        if (!pawn.IsValid) return;

        var playerIndex = (int)pawn.Controller.Index;
        var player = Utilities.GetPlayerFromIndex(playerIndex);
        if (!Utility.IsPlayerValid(player)) return;

        ChangeWeaponAttributes(weapon, player, isKnife);
    }
    catch (Exception) { }
});
-----------------------------------------
foreach (var item in weaponDefaults.ToList())
{
    var weapon = new CBasePlayerWeapon(item.Key);
    Server.NextFrame(() =>
    {
        try
        {
            if (!weapon.IsValid) return;

            CCSWeaponBaseVData? _weapon = weapon.As<CCSWeaponBase>().VData;
            if (_weapon == null) return;
            if (_weapon.GearSlot != gear_slot_t.GEAR_SLOT_KNIFE &&
                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_GRENADES &&
                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_INVALID &&
                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_BOOSTS &&
                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_UTILITY &&
                _weapon.GearSlot != gear_slot_t.GEAR_SLOT_C4)
            {
                _weapon.MaxClip1 = item.Value.MaxClip1;
                _weapon.MaxClip2 = item.Value.MaxClip2;
                _weapon.DefaultClip1 = item.Value.DefaultClip1;
                _weapon.DefaultClip2 = item.Value.DefaultClip2;
            }
        }
        catch (Exception) { }
    });
}
-------------------------------------------
CBasePlayerWeapon newWeapon = new(player.GiveNamedItem(weaponByDefindex));

Server.NextFrame(() =>
{
    if (newWeapon == null) return;
    try
    {
        newWeapon.Clip1 = clip1;
        newWeapon.ReserveAmmo[0] = reservedAmmo;
    }
    catch (Exception)
    { }
});

*/