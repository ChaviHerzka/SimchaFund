using Microsoft.AspNetCore.Mvc;
using SimchaFund.web.Models;
using System.Diagnostics;
using System.Data.SqlClient;
using SimchaFund.Data;

namespace SimchaFund.web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=Simchas;Integrated Security=true;";
        public IActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            DbManager db = new DbManager(_connectionString);
            SimchosViewModel vm= new SimchosViewModel();                                                                                  
            List<Simcha> simchos = db.GetAllSimchos();
            
            vm.TotalContributors = db.GetNumberOfContributors();
            foreach (Simcha simcha in simchos)
            {
                simcha.ContCount = db.GetContributorsCountForSimcha(simcha.Id);
            }
            vm.Simchos = simchos;
            return View(vm);
        }

    }
}