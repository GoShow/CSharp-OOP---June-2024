using InfluencerManagerApp.Core.Contracts;
using InfluencerManagerApp.Models;
using InfluencerManagerApp.Models.Contracts;
using InfluencerManagerApp.Repositories;
using InfluencerManagerApp.Repositories.Contracts;
using InfluencerManagerApp.Utilities.Messages;
using System.Text;

namespace InfluencerManagerApp.Core;

public class Controller : IController
{
    private readonly IRepository<IInfluencer> influencerRepository = new InfluencerRepository();
    private readonly IRepository<ICampaign> campaignRepository = new CampaignRepository();

    public string ApplicationReport()
    {
        StringBuilder sb = new StringBuilder();

        IOrderedEnumerable<IInfluencer> orderedInfluencers = influencerRepository.Models
            .OrderByDescending(i => i.Income)
            .ThenByDescending(i => i.Followers);

        foreach (IInfluencer influencer in orderedInfluencers)
        {
            sb.AppendLine(influencer.ToString());

            if (influencer.Participations.Any())
            {
                sb.AppendLine("Active Campaigns:");

                IOrderedEnumerable<ICampaign> orderedCampaigns = campaignRepository.Models
                    .Where(c => c.Contributors.Contains(influencer.Username))
                    .OrderBy(c => c.Brand);

                foreach (ICampaign campaign in orderedCampaigns)
                {
                    sb.AppendLine($"--{campaign.ToString()}");
                }
            }
        }

        return sb.ToString().TrimEnd();
    }

    public string AttractInfluencer(string brand, string username)
    {
        IInfluencer influencer = influencerRepository.FindByName(username);

        if (influencer is null)
        {
            return string.Format(OutputMessages.InfluencerNotFound, nameof(InfluencerRepository), username);
        }

        ICampaign campaign = campaignRepository.FindByName(brand);

        if (campaign is null)
        {
            return string.Format(OutputMessages.CampaignNotFound, brand);
        }

        if (campaign.Contributors.Contains(username))
        {
            return string.Format(OutputMessages.InfluencerAlreadyEngaged, username, brand);
        }

        bool isEligible = false;

        if (campaign.GetType().Name == nameof(ProductCampaign))
        {
            isEligible =
                influencer.GetType().Name == nameof(BusinessInfluencer) ||
                influencer.GetType().Name == nameof(FashionInfluencer);
        }
        else if (campaign.GetType().Name == nameof(ServiceCampaign))
        {
            isEligible =
                influencer.GetType().Name == nameof(BusinessInfluencer) ||
                influencer.GetType().Name == nameof(BloggerInfluencer);
        }

        if (!isEligible)
        {
            return string.Format(OutputMessages.InfluencerNotEligibleForCampaign, username, brand);
        }

        int campaignPrice = influencer.CalculateCampaignPrice();

        if (campaign.Budget < campaignPrice)
        {
            return string.Format(OutputMessages.UnsufficientBudget, brand, username);
        }

        influencer.EarnFee(campaignPrice);
        influencer.EnrollCampaign(brand);
        campaign.Engage(influencer);

        return string.Format(OutputMessages.InfluencerAttractedSuccessfully, username, brand);
    }

    public string BeginCampaign(string typeName, string brand)
    {
        ICampaign campaign;

        switch (typeName)
        {
            case nameof(ProductCampaign):
                campaign = new ProductCampaign(brand);
                break;
            case nameof(ServiceCampaign):
                campaign = new ServiceCampaign(brand);
                break;
            default:
                return string.Format(OutputMessages.CampaignTypeIsNotValid, typeName);
        }

        ICampaign existingCampaign = campaignRepository.FindByName(brand);

        if (existingCampaign is not null)
        {
            return string.Format(OutputMessages.CampaignDuplicated, brand);
        }

        campaignRepository.AddModel(campaign);

        return string.Format(OutputMessages.CampaignStartedSuccessfully, brand, typeName);
    }

    public string CloseCampaign(string brand)
    {
        ICampaign campaign = campaignRepository.FindByName(brand);

        if (campaign is null)
        {
            return string.Format(OutputMessages.InvalidCampaignToClose);
        }

        if (campaign.Budget <= 10000)
        {
            return string.Format(OutputMessages.CampaignCannotBeClosed, brand);
        }

        foreach (string name in campaign.Contributors)
        {
            IInfluencer influencer = influencerRepository.FindByName(name);
            influencer.EarnFee(2000);
            influencer.EndParticipation(campaign.Brand);
        }

        campaignRepository.RemoveModel(campaign);

        return string.Format(OutputMessages.CampaignClosedSuccessfully, brand);
    }

    public string ConcludeAppContract(string username)
    {
        IInfluencer influencer = influencerRepository.FindByName(username);

        if (influencer is null)
        {
            return string.Format(OutputMessages.InfluencerNotSigned, username);
        }

        if (influencer.Participations.Any())
        {
            return string.Format(OutputMessages.InfluencerHasActiveParticipations, username);
        }

        influencerRepository.RemoveModel(influencer);

        return string.Format(OutputMessages.ContractConcludedSuccessfully, username);
    }

    public string FundCampaign(string brand, double amount)
    {
        ICampaign campaign = campaignRepository.FindByName(brand);

        if (campaign is null)
        {
            return string.Format(OutputMessages.InvalidCampaignToFund);
        }

        if (amount <= 0)
        {
            return string.Format(OutputMessages.NotPositiveFundingAmount);
        }

        campaign.Gain(amount);

        return string.Format(OutputMessages.CampaignFundedSuccessfully, brand, amount);
    }

    public string RegisterInfluencer(string typeName, string username, int followers)
    {
        IInfluencer influencer;

        switch (typeName)
        {
            case nameof(BloggerInfluencer):
                influencer = new BloggerInfluencer(username, followers);
                break;
            case nameof(BusinessInfluencer):
                influencer = new BusinessInfluencer(username, followers);
                break;
            case nameof(FashionInfluencer):
                influencer = new FashionInfluencer(username, followers);
                break;
            default:
                return string.Format(OutputMessages.InfluencerInvalidType, typeName);
        }

        IInfluencer existingInfluencer = influencerRepository.FindByName(username);

        if (existingInfluencer is not null)
        {
            return string.Format(OutputMessages.UsernameIsRegistered, username, nameof(InfluencerRepository));
        }

        influencerRepository.AddModel(influencer);

        return string.Format(OutputMessages.InfluencerRegisteredSuccessfully, username);
    }
}
