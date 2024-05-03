using EmployeeTaxDeclaration.Data;
using EmployeeTaxDeclaration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxDeclaration.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }   

        public IActionResult Index(string financialYear="",string employeeId="",string employeeName="", string declarationStatus="")
        {
            financialYear = string.IsNullOrEmpty(financialYear)?"":financialYear;
            employeeId = string.IsNullOrEmpty(employeeId) ? "" : employeeId;
            employeeName = string.IsNullOrEmpty(employeeId) ? "" : employeeName;
            declarationStatus = string.IsNullOrEmpty(declarationStatus) ? "" : declarationStatus;
            int finyear;
            if(financialYear != "")
                finyear = Convert.ToInt32(financialYear);
            var forData = new ViewModal();
            var forms = _db.TaxForms.Include(x => x.User).Where(tf => (employeeName == "" || tf.User.FirstName == employeeName) && (employeeId=="" || tf.User.Id == employeeId) && (declarationStatus == "" || tf.DeclarationStatus==declarationStatus));
            forData.Form = forms;
            return View(forData);
        }

        public IActionResult ShowDisplay(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            IEnumerable<TaxForm> tax = _db.TaxForms.Include(x => x.User).Where(tf => (tf.Id == id));
            return View(tax.First());
        }
    }
}
