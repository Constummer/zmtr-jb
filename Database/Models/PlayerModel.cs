using CounterStrikeSharp.API.Modules.Utils;
using System.ComponentModel.DataAnnotations;

namespace JailbreakExtras;

public class PlayerModel
{
    public PlayerModel(int id)
    {
        Id = id;
    }

    public PlayerModel()
    {
    }

    public int Id { get; set; }
    public string Text { get; set; }
    public string PathToModel { get; set; }

    [Range(0, int.MaxValue)]
    public int Cost { get; set; } = 0;

    public bool Enable { get; set; }
    public CsTeam TeamNo { get; set; }
}