using Microsoft.AspNetCore.Mvc;
using Beerhall.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Beerhall.Controllers
{
    public class BrewerController : Controller
    {
        private readonly IBrewerRepository _brewerRepository;
        public BrewerController(IBrewerRepository brewerRepository)
        {
            _brewerRepository = brewerRepository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Brewer> brewers = _brewerRepository.GetAll().OrderBy(b => b.Name).ToList();
            ViewData["TotalTurnover"] = brewers.Sum(b => b.Turnover);
            return View(brewers);
        }
    }
}
