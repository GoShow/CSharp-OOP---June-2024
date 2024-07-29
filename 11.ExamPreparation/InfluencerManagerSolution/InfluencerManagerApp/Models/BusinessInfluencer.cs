namespace InfluencerManagerApp.Models;

internal class BusinessInfluencer : Influencer
{
    private const double BIEngagementRate = 3.0;
    private const double Factor = 0.15;

    public BusinessInfluencer(string username, int followers)
        : base(username, followers, BIEngagementRate)
    {
    }

    public override int CalculateCampaignPrice()
        => (int)(base.CalculateCampaignPrice() * Factor);
}
