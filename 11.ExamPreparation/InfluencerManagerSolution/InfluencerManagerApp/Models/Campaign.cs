using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Utilities.Messages;

namespace InfluencerManagerApp.Models;

public abstract class Campaign : ICampaign
{
    private string brand;
    private readonly List<string> contributors;

    protected Campaign(string brand, double budget)
    {
        Brand = brand;
        Budget = budget;

        contributors = new List<string>();
    }

    public string Brand
    {
        get => brand;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ExceptionMessages.BrandIsrequired);
            }

            brand = value;
        }
    }

    public double Budget { get; private set; }

    public IReadOnlyCollection<string> Contributors => contributors.AsReadOnly();

    public void Engage(IInfluencer influencer)
    {
        contributors.Add(influencer.Username);
        Budget -= influencer.CalculateCampaignPrice();
    }

    public void Gain(double amount)
    {
        Budget += amount;
    }

    public override string ToString()
        => $"{GetType().Name} - Brand: {Brand}, Budget: {Budget}, Contributors: {Contributors.Count}";
}
