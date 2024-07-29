using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Repositories.Contracts;

namespace InfluencerManagerApp.Repositories;
public class CampaignRepository : IRepository<ICampaign>
{
    private readonly List<ICampaign> models = new();

    public IReadOnlyCollection<ICampaign> Models => models.AsReadOnly();

    public void AddModel(ICampaign model)
    {
        models.Add(model);
    }

    public ICampaign FindByName(string name)
    {
        return models.FirstOrDefault(m => m.Brand == name);
    }

    public bool RemoveModel(ICampaign model)
    {
        return models.Remove(model);
    }
}
