using SimchaFund.Data;
namespace SimchaFund.web.Models
{
    public class HistoryViewModel
    {
        public List<Transaction> Contributions { get; set; }
        public decimal Balance { get; set; }
        public string Name { get; set; }  

    }
}