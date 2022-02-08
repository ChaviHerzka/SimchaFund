using SimchaFund.Data;
namespace SimchaFund.web.Models
{
    public class ContributionsViewModel
    {
        public Simcha Simcha { get; set; }
        public string Name { get; set; }
        public int SimchaId { get; set; }
        public List<SimchaContributor> SimchaContributors { get; set; }
      
    }
}
