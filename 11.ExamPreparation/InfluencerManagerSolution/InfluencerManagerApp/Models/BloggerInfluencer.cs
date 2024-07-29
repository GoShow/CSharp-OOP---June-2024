namespace InfluencerManagerApp.Models;

internal class BloggerInfluencer : Influencer
{
    private const double BIEngagementRate = 2.0;
    private const double Factor = 0.2;

    public BloggerInfluencer(string username, int followers)
        : base(username, followers, BIEngagementRate)
    {
    }

    public override int CalculateCampaignPrice()
        => (int)(base.CalculateCampaignPrice() * Factor);
}
