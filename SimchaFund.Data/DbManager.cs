using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SimchaFund.Data
{
    public class DbManager
    {
        string _connectionString;
        public DbManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Simcha> GetAllSimchos()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Simchos";
                List<Simcha> simchos = new List<Simcha>();
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var s = (int)reader["Id"];
                    simchos.Add(new Simcha
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Date = (DateTime)reader["Date"],
                        TotalContributors = GetTotalContributionForSimcha(s),

                    });


                }
                return simchos;
            }
        }
        public int GetAmountContributed(int contributorId, int simchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"select ISNULL(sum(Amount),0) from Contributions
                                        where SimchaId = @simchaId and ContributorId = @contributorId";
                command.Parameters.AddWithValue("@contributorId", contributorId);
                command.Parameters.AddWithValue("@simchaId", simchaId);
                connection.Open();
                command.ExecuteNonQuery();
                return (int)(decimal)command.ExecuteScalar();

            }
        }
        public int GetContributorsCountForSimcha(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"select count(distinct ContributorId) from Contributions where SimchaId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            return (int)command.ExecuteScalar();
        }
        public List<SimchaContributor> GetSimchaContributions(int simchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"select * from contributors";
                connection.Open();
                var reader = command.ExecuteReader();
                List<SimchaContributor> simchaContributors = new List<SimchaContributor>();
                int counter = 0;
                while (reader.Read())
                {
                    var Id = (int)reader["Id"];
                    var contributedAmount = GetAmountContributed(Id, simchaId);
                    simchaContributors.Add(new SimchaContributor
                    {
                        ContributorId = Id,
                        Amount = contributedAmount,
                        Counter = counter,
                        AlwaysInclude = (bool)reader["AlwaysInclude"],
                        Balance = GetBalance((int)reader["Id"]),
                        Name = (string)reader["FirstName"] + " " + (string)reader["LastName"],
                        Included = contributedAmount > 0

                    });
                    counter++;
                };
                return simchaContributors;
            }

        }
        
       
        public List<Transaction> GetContributorContributions(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"Select * from Contributions c join Simchos s on c.SimchaId = s.Id where c.ContributorId = @contributorId";
                command.Parameters.AddWithValue("@contributorId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                List<Transaction> transacts = new List<Transaction>();
                while (reader.Read())
                {
                    transacts.Add(new Transaction
                    {
                        Action = $"Contribution for {(string)reader["Name"]}",
                        Date = (DateTime)reader["Date"],
                        ContributorId = (int)reader["ContributorId"],
                        Amount = (decimal)reader["Amount"],
                    });
                };
                return transacts;
            }
        }
        
        public void AddSimcha(Simcha simcha)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"Insert into Simchos values(@name, @date)";
                command.Parameters.AddWithValue("@name", simcha.Name);
                command.Parameters.AddWithValue("@date", simcha.Date);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public string GetSimchaName(int simchaid) 
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"select Name from simchos where Id = @id";
                command.Parameters.AddWithValue("@id", simchaid);
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                return (string)reader["Name"];

            }
        }
        public int GetNumberOfContributors()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"select count(*) from Contributors";
                connection.Open();
                return (int)command.ExecuteScalar();

            }
        }
        public int GetTotalContributionForSimcha(int simchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"select ISNULL(sum(Amount), 0) from Contributions where simchaId = @Id";
                command.Parameters.AddWithValue("@Id", simchaId);
                connection.Open();
                return (int)(decimal)command.ExecuteScalar();
            }
        }
        public int GetTotalContributions()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select ISNULL(sum(Amount), 0) from Contributions";
                connection.Open();
                return (int)(decimal)command.ExecuteScalar();
            }
        }

        public void AddContributor(Contributor contributor)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Contributors (FirstName, LastName, Cell, AlwaysInclude, CreatedDate) " +
                             " Values (@firstname, @lastname, @cell, @alwaysInclude, @createdDate)";
            cmd.Parameters.AddWithValue("@firstname", contributor.FirstName);
            cmd.Parameters.AddWithValue("@lastname", contributor.LastName);
            cmd.Parameters.AddWithValue("@cell", contributor.Cell);
            cmd.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
            cmd.Parameters.AddWithValue("@createdDate", contributor.CreatedDate);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
       
        public List<Contributor> GetContributors()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select * from Contributors";
                List<Contributor> contrib = new List<Contributor>();
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    contrib.Add(new Contributor
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Cell = (string)reader["Cell"],
                        CreatedDate = (DateTime)reader["CreatedDate"],
                        AlwaysInclude = (bool)reader["AlwaysInclude"]
                    });

                }
                return contrib;
            };


        }
        public void AddDeposit(Transaction deposit)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"Insert into Deposits(Amount, Date, ContributorId)
                                    Values(@amount, @date, @contributorId)";
                cmd.Parameters.AddWithValue("@amount", deposit.Amount);
                cmd.Parameters.AddWithValue("@date", deposit.Date);
                cmd.Parameters.AddWithValue("@ContributorId", deposit.ContributorId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ClearAndReplace(int simchaId, List<SimchaContributor> simchaContributors)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Contributions where SimchaId = @id";
                cmd.Parameters.AddWithValue("@id", simchaId);
                connection.Open();
                cmd.ExecuteNonQuery();
                foreach (SimchaContributor s in simchaContributors)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"Insert into Contributions(SimchaId, ContributorId, Amount, Date)
                                        values(@simchaId, @contributorId, @amount, @date)";
                    cmd.Parameters.AddWithValue("@simchaId", simchaId);
                    cmd.Parameters.AddWithValue("@contributorId", s.ContributorId);
                    cmd.Parameters.AddWithValue("@amount", s.Amount);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AddHistoryOfDeposits(List<Transaction> t, int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Deposits WHERE ContributorId=@id";
            command.Parameters.AddWithValue("@id", contributorId);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                t.Add(new Transaction
                {
                    Action = "Deposit",
                    Date = (DateTime)reader["Date"],
                    Amount = (decimal)reader["Amount"]
                });
            }
        }
        public string GetNameById(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT FirstName, LastName FROM Contributors WHERE Id=@id ";
            command.Parameters.AddWithValue("@id", contributorId);
            connection.Open();
            var reader = command.ExecuteReader();
            reader.Read();

            return (string)reader["FirstName"];

        }
        public int GetTotalDeposit()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Select ISNULL(SUM(Amount), 0) from Deposits";
            connection.Open();
            return (int)(decimal)cmd.ExecuteScalar();

        }

        public decimal GetTotal()
        {
            return GetTotalDeposit() - GetTotalContributions();
        }
        public int GetBalance(int contribId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            {
                cmd.CommandText = @"SELECT(SELECT ISNULL(SUM(d.Amount), '0') 
                                    From Contributors c 
                                    JOIN Deposits d 
                                    ON c.Id = d.ContributorId WHERE c.Id = @id) 
                                    -(SELECT ISNULL (SUM(cs.Amount), '0') 
                                     From Contributors c 
                                    JOIN Contributions cs ON c.Id = cs.ContributorId WHERE c.Id = @id)";
                cmd.Parameters.AddWithValue("@id", contribId);
                connection.Open();
                return (int)(decimal)cmd.ExecuteScalar();
            }
        }
        public void EditContributor(Contributor contributor)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            {
                cmd.CommandText = @"update contributors 
                                    set FirstName = @firstname, LastName = @lastname, Cell = @cell, CreatedDate = @date, AlwaysInclude = @alwaysinclude where Id = @id";
                cmd.Parameters.AddWithValue("@firstname", contributor.FirstName);
                cmd.Parameters.AddWithValue("@lastname", contributor.LastName);
                cmd.Parameters.AddWithValue("@cell", contributor.Cell);
                cmd.Parameters.AddWithValue("@date", contributor.CreatedDate);
                cmd.Parameters.AddWithValue("@alwaysinclude", contributor.AlwaysInclude);
                cmd.Parameters.AddWithValue("@id", contributor.Id);
                connection.Open();
                cmd.ExecuteNonQuery();

            }
        }
    }


}

