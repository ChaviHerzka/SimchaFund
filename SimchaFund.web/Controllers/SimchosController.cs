using Microsoft.AspNetCore.Mvc;
using SimchaFund.web.Models;
using System.Diagnostics;
using System.Data.SqlClient;
using SimchaFund.Data;


namespace SimchaFund.web.Controllers
{
    public class SimchosController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=Simchas;Integrated Security=true;";

       [HttpPost]
        public IActionResult AddSimcha(Simcha simcha)
        {
            DbManager db = new DbManager(_connectionString);
            db.AddSimcha(simcha);
            TempData["Message"] = $"New Simcha Created! Id: {simcha.Id}";
            return Redirect("/home/index");
        }
        public IActionResult ViewContributions(int contributorId, int simchaId)
        {
            ContributionsViewModel vm = new ContributionsViewModel();
            DbManager db = new DbManager(_connectionString);
            List<Simcha> simchaList = db.GetAllSimchos();
            Simcha simcha = simchaList.FirstOrDefault(s => s.Id == simchaId);
            vm.Simcha = simcha;
            
            List<SimchaContributor> contributions = db.GetSimchaContributions( simchaId);
            vm.SimchaContributors = contributions;

            return View(vm);
        
        }
        [HttpPost]
        public IActionResult UpdateContributions(int simchaId, List<SimchaContributor> simchaContributor)
        {
            DbManager db = new DbManager(_connectionString);
            List<SimchaContributor> currents = simchaContributor.Where(t => t.Included).ToList();
            db.ClearAndReplace(simchaId, currents);
            TempData["Message"] = "Simcha Updated Succesfully";
            return Redirect("/home/index");
        }
    }
}
