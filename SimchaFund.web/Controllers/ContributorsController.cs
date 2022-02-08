using SimchaFund.Data;
using Microsoft.AspNetCore.Mvc;
using SimchaFund.web.Models;

namespace SimchaFund.web.Controllers
{
    public class ContributorsController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=Simchas;Integrated Security=true;";
        public IActionResult Contributors()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            DbManager db = new DbManager(_connectionString);
            List<Contributor> contributors = db.GetContributors();
            ContributorsIndexViewModel vm = new ContributorsIndexViewModel();
            vm.Contributors = contributors;
            vm.Total = db.GetTotal();
            foreach (Contributor c in contributors)
            {
                c.Balance = db.GetBalance(c.Id);
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult NewContributor(Contributor contributor)
        {
            DbManager db = new DbManager(_connectionString);
            db.AddContributor(contributor);
            TempData["Message"] = "New Contributor Added";
            return Redirect("/Contributors/Contributors");
        }
        [HttpPost]
        public IActionResult AddDeposit(Transaction deposit)
        {
            DbManager db = new DbManager(_connectionString);
            db.AddDeposit(deposit);
            TempData["Message"] = "Deposit Successfully recorded";
            return Redirect("/Contributors/Contributors");
        }

        [HttpPost]
        public IActionResult Edit(Contributor contributor)
        {
            DbManager db = new DbManager(_connectionString);
            db.EditContributor(contributor);
            return Redirect("/Contributors/Contributors");
        }
        public IActionResult History(int id)
        {
            DbManager db = new DbManager(_connectionString);
            List<Transaction> t = db.GetContributorContributions(id);
            db.AddHistoryOfDeposits(t, id);
            t = t.OrderBy(t => t.Date).ToList();
            HistoryViewModel vm = new()
            {
                Balance = db.GetBalance(id),
                Contributions = t,
                Name = db.GetNameById(id),

            };
            return View(vm);
        }
    }

}
