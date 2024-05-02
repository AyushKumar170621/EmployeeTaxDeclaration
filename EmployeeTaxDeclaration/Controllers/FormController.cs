using EmployeeTaxDeclaration.Data;
using EmployeeTaxDeclaration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualBasic;

namespace EmployeeTaxDeclaration.Controllers
{
    public class FormController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public FormController(ApplicationDbContext db,UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ShowFormAsync(string financial)
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            var userId = usr.Id;
            IEnumerable<TaxForm> taxForms = _db.TaxForms.Where(tf => tf.User.Id == userId);
            if (taxForms.Any())
            {
                foreach (TaxForm taxForm in taxForms)
                {
                    if (taxForm.FinancialYear == Convert.ToInt32(financial))
                    { 
                        if(taxForm.Frezeed)
                        {
                            return RedirectToAction("FreezedDisplay", taxForm);
                        }
                        return View(taxForm);
                    }
                }
            }
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ShowFormAsync(TaxForm taxForm,string financial)
        {
            if (!ModelState.IsValid)
            {
                ApplicationUser usr = await GetCurrentUserAsync();
                taxForm.User = usr;
                taxForm.FinancialYear = Convert.ToInt32(financial);
                taxForm.Frezeed = true;
                taxForm.DeclarationStatus = "Submited";
                _db.TaxForms.Add(taxForm);
                await _db.SaveChangesAsync();
                return Redirect("/");
            }
            return View();
        }
        public IActionResult FreezedDisplay(TaxForm taxForm)
        {
            return View(taxForm);
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
