using Microsoft.AspNetCore.Mvc;
using Beerhall.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using Beerhall.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Beerhall.Controllers
{
    public class BrewerController : Controller
    {
        private readonly IBrewerRepository _brewerRepository;
        private readonly ILocationRepository _locationRepository;
        public BrewerController(IBrewerRepository brewerRepository, ILocationRepository locationRepository)
        {
            _brewerRepository = brewerRepository;
            _locationRepository = locationRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Brewer> brewers = _brewerRepository.GetAll().OrderBy(b => b.Name).ToList();
            ViewData["TotalTurnover"] = brewers.Sum(b => b.Turnover);
            return View(brewers);
        }

        public IActionResult Edit(int id)
        {
            Brewer brewer = _brewerRepository.GetBy(id);
            ViewData["Locations"] = new SelectList(
                _locationRepository.GetAll().OrderBy(l => l.Name),
                nameof(Location.PostalCode),
                nameof(Location.Name));
            return View(new BrewerEditViewModel(brewer));
        }

        [HttpPost]
        public IActionResult Edit(BrewerEditViewModel brewerEditViewModel, int id)
        {
            Brewer brewer = _brewerRepository.GetBy(id);
            brewer.Name = brewerEditViewModel.Name;
            brewer.Street = brewerEditViewModel.Street;
            brewer.Location = brewerEditViewModel.PostalCode == null ? null : _locationRepository.GetBy(brewerEditViewModel.PostalCode);
            brewer.Turnover = brewerEditViewModel.Turnover;
            _brewerRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
