namespace InfluencerManagerApp.Models;
public class ServiceCampaign : Campaign
{
    private const double SCBUdget = 30_000;

    public ServiceCampaign(string brand)
        : base(brand, SCBUdget)
    {
    }
}
