namespace InfluencerManagerApp.Models;

internal class FashionInfluencer : Influencer
{
    private const double FIEngagementRate = 4.0;
    private const double Factor = 0.1;

    public FashionInfluencer(string username, int followers)
        : base(username, followers, FIEngagementRate)
    {
    }

    public override int CalculateCampaignPrice()
        => (int)(base.CalculateCampaignPrice() * Factor);
}
