using Microsoft.AspNetCore.Mvc;
using webkino.Services.Interfaces;

namespace webkino.Controllers
{
    public class StudioController : Controller
    {
        private readonly IStudioService _studioService;

        public StudioController(IStudioService studioService)
        {
            _studioService = studioService;
        }

        public IActionResult Index()
        {
            var studios = _studioService.GetAll();
            return View(studios);
        }
    }
}