namespace InfluencerManagerApp.Models;
public class ProductCampaign : Campaign
{
    private const double PCBUdget = 60_000;

    public ProductCampaign(string brand)
        : base(brand, PCBUdget)
    {
    }
}
