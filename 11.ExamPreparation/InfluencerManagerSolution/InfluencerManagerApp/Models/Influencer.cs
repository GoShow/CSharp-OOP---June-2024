using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Utilities.Messages;

namespace InfluencerManagerApp.Models;

public abstract class Influencer : IInfluencer
{
    private string username;
    private int followers;
    private double income;
    private readonly List<string> participations;

    protected Influencer(string username, int followers, double engagementRate)
    {
        Username = username;
        Followers = followers;
        EngagementRate = engagementRate;

        participations = new List<string>();
    }

    public string Username
    {
        get => username;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ExceptionMessages.UsernameIsRequired);
            }

            username = value;
        }
    }

    public int Followers
    {
        get => followers;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException(ExceptionMessages.FollowersCountNegative);
            }

            followers = value;
        }
    }

    public double EngagementRate { get; private set; }

    public double Income { get; private set; }

    public IReadOnlyCollection<string> Participations => participations.AsReadOnly();

    public virtual int CalculateCampaignPrice()
    {
        return (int)(Followers * EngagementRate);
    }

    public void EarnFee(double amount)
    {
        Income += amount;
    }

    public void EndParticipation(string brand)
    {
        participations.Remove(brand);
    }

    public void EnrollCampaign(string brand)
    {
        participations.Add(brand);
    }

    public override string ToString()
        => $"{Username} - Followers: {Followers}, Total Income: {Income}";
}
