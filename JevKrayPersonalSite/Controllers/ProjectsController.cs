using JevKrayPersonalSite.DAL;
using JevKrayPersonalSite.Models;
using JevKrayPersonalSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JevKrayPersonalSite.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly JevkSiteDbContext _dbContext;

        public ProjectsController(ILogger<ProjectsController> logger, JevkSiteDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Projects()
        {
            var projects = await _dbContext.Projects.ToListAsync();
            var projectPictures = await _dbContext.ProjectPictures.ToListAsync();

            var viewModel = new ProjectViewModel
            {
                Projects = projects,
                Pictures = projectPictures,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ProjectPreview(int Id)
        {
            var project = await _dbContext.Projects
                .Include(p => p.ProjectPictures)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (project == null)
            {
                return Error();
            }

            return View(project);
        }

        public async Task<string> GetProjectImage(int number, int id)
        {
            var project = await _dbContext.Projects
                .Include(p => p.ProjectPictures)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project.ProjectPictures.FirstOrDefault(p => p.Number == number).ImageLink;
        }

        public IActionResult GazorpGameStore()
        {
            return View();
        }

        public IActionResult Ferma()
        {
            return View();
        }

        public IActionResult WPumper()
        {
            return View();
        }

        public IActionResult SteamChecker()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
