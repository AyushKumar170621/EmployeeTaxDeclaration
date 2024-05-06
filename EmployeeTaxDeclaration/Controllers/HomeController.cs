using EmployeeTaxDeclaration.Data;
using EmployeeTaxDeclaration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Diagnostics;

namespace EmployeeTaxDeclaration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> LoggedUserFormsAsync(int currentPage = 1)
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            var forms = _db.TaxForms.Include(x => x.User).Where(tf => tf.User.Id == usr.Id);
            var forData = new ViewModal();
            int totalRecords = forms.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            forms = forms.Skip((currentPage - 1) * pageSize).Take(pageSize);
            forData.Form = forms;
            forData.CurrentPage = currentPage;
            forData.TotalPage = totalPages;
            forData.PageSize = pageSize;

            return View(forData);
        }

        [Authorize(Roles = "client")]
        public IActionResult ShowDisplay(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            IEnumerable<TaxForm> tax = _db.TaxForms.Include(x => x.User).Where(tf => (tf.Id == id));
            return View(tax.First());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
