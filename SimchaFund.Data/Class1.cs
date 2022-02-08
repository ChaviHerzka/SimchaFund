namespace SimchaFund.Data
{
    public class Simcha
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int TotalContributors { get;  set; }
        public int ContCount { get;  set; }
        
    }
    public class Contributor 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool AlwaysInclude { get; set; }
        public int Balance { get; set; }
       
    }

    public class SimchaContributor
    { 
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public bool AlwaysInclude { get; set; }
        public int ContributorId { get; set; }
        public bool Included { get; set; }
        public decimal Amount { get; set; }
        public int Counter { get;set; }

    
    }

    public class Transaction 
    {
        public int ContributorId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
        public bool Included { get; set; }
        public string Action { get; set; }

    }
}