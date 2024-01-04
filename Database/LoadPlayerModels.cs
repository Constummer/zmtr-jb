namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LoadPlayerModels()
    {
        foreach (var model in Config.Market.MarketModeller.Where(x => x.Enable == true))
        {
            PlayerModels.Add(model.Id, new PlayerModel()
            {
                Id = model.Id,
                TeamNo = model.TeamNo,
                Text = model.Text,
                Cost = model.Cost,
                Enable = model.Enable,
                PathToModel = model.PathToModel,
            });
        }
    }
}