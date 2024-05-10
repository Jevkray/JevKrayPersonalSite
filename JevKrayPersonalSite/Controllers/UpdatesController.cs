using JevKrayPersonalSite.DAL;
using JevKrayPersonalSite.Models;
using JevKrayPersonalSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JevKrayPersonalSite.Controllers
{
    public class UpdatesController : Controller
    {
        private const int pageSize = 10;

        private readonly ILogger<UpdatesController> _logger;
        private readonly JevkSiteDbContext _dbContext;

        public UpdatesController(JevkSiteDbContext dbContext, ILogger<UpdatesController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IActionResult> Updates(int page = 1)
        {
            var totalCount = await _dbContext.Commits.CountAsync();
            var commits = await _dbContext.Commits
                .OrderByDescending(c => c.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new UpdatesViewModel
            {
                Commits = commits,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };

            return View(viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
