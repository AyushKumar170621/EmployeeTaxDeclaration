using EmployeeTaxDeclaration.Data;
using EmployeeTaxDeclaration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using NuGet.Protocol.Plugins;

namespace EmployeeTaxDeclaration.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private bool isLoaded = false;
        private List<ApplicationUser> users;
        private Dictionary<string, int> userIdToInt = new Dictionary<string, int>();
        private Dictionary<int, string> IntUserID = new Dictionary<int,string>();

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index(string financialYear="",string employeeId="",string employeeName = "", string declarationStatus="",int currentPage=1)
        {
            //checking users for id matching
            if(!isLoaded )
            {
               await LoadUser();
            }
            //replacing null value with empty string
            financialYear = string.IsNullOrEmpty(financialYear)?"":financialYear;
            employeeId = string.IsNullOrEmpty(employeeId) ? "" : employeeId;
            employeeName = string.IsNullOrEmpty(employeeName) ? "" : employeeName;
            declarationStatus = string.IsNullOrEmpty(declarationStatus) ? "" : declarationStatus; 

            //converting data to required datatype
            int empId = 0, finyear = 0;
            int userCount = users.Count();
            string mappedId = "";
            //mapping provided id with original id
            if (employeeId != "")
                empId = Convert.ToInt32(employeeId);

            if (empId != 0 && IntUserID.TryGetValue(empId, out mappedId))
            {
                employeeId = mappedId;
            }
            else if (empId == 0)
            {
                employeeId = "";
            }
            else
            {
                employeeId = "random";
            }
            if (financialYear != "")
                finyear = Convert.ToInt32(financialYear);


            try
            {
                //filtering forms based on given query
                var forms = _db.TaxForms.Include(x => x.User).Where(tf => (employeeName == "" || ((tf.User.FirstName + " " + tf.User.LastName).StartsWith(employeeName))) && (employeeId == "" || tf.User.Id == employeeId) && (declarationStatus == "" || tf.DeclarationStatus == declarationStatus) && (financialYear == "" || tf.FinancialYear == finyear));

                //view modal
                var forData = new ViewModal();

                //setting parameters
                int totalRecords = forms.Count();
                int pageSize = 5;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                forms = forms.Skip((currentPage - 1) * pageSize).Take(pageSize);

                //setting up view modal for display
                forData.Form = forms;
                forData.CurrentPage = currentPage;
                forData.TotalPage = totalPages;
                forData.PageSize = pageSize;
                forData.declarationStatus = declarationStatus;
                forData.IdMap = userIdToInt;
                forData.employeeId = employeeId;
                forData.employeeName = employeeName;
                forData.finacialYear = financialYear;

                return View(forData);
            }
            catch(Exception e)
            {
                string ErrMsg = e.Message;
                return RedirectToAction("Error", new { ErrMsg = ErrMsg });
            }
            
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DisplayWithActionAsync(string financialYear = "", string employeeId = "", string employeeName = "", int currentPage = 1,string status_msg="")
        {
            //checking users for id matching
            if (!isLoaded)
            {
                await LoadUser();
            }
            //replacing null value with empty string
            financialYear = string.IsNullOrEmpty(financialYear) ? "" : financialYear;
            employeeId = string.IsNullOrEmpty(employeeId) ? "" : employeeId;
            employeeName = string.IsNullOrEmpty(employeeName) ? "" : employeeName;
            
            //converting data to required datatype
            int empId = 0, finyear = 0;
            int userCount = users.Count();
            string mappedId = "";
            //mapping provided id with original id
            if (employeeId != "")
                empId = Convert.ToInt32(employeeId);
            
            if (empId != 0 && IntUserID.TryGetValue(empId,out mappedId))
            {
                employeeId = mappedId;
            }
            else if(empId == 0)
            {
                employeeId = "";
            }
            else
            {
                employeeId = "random";
            }
            if (financialYear != "")
                finyear = Convert.ToInt32(financialYear);

            try
            {
                //filtering forms based on given query
                var forms = _db.TaxForms.Include(x => x.User).Where(tf => (employeeName == "" || ((tf.User.FirstName + " " + tf.User.LastName).StartsWith(employeeName))) && (employeeId == "" || tf.User.Id == employeeId) && (financialYear == "" || tf.FinancialYear == finyear));

                //view modal
                var forData = new ViewModal();

                //setting parameters
                int totalRecords = forms.Count();
                int pageSize = 5;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                forms = forms.Skip((currentPage - 1) * pageSize).Take(pageSize);

                //setting up view modal for display
                forData.Form = forms;
                forData.CurrentPage = currentPage;
                forData.TotalPage = totalPages;
                forData.IdMap = userIdToInt;
                forData.PageSize = pageSize;
                forData.employeeId = employeeId;
                forData.employeeName = employeeName;
                forData.finacialYear = financialYear;
                ViewBag.Msg = status_msg;
                return View(forData);
            }
            catch(Exception e)
            {
                string ErrMsg = e.Message;
                return RedirectToAction("Error", new { ErrMsg = ErrMsg });
            }
            
        }
        private async Task LoadUser()
        {
            users = _db.AppUsers.ToList();
            int n = users.Count;
            for(int i=n;i>0;i--)
            {
                userIdToInt[users[i-1].Id] = i;
                IntUserID[i] = users[i - 1].Id;
            }
            isLoaded = true;
        }

        [Authorize(Roles = "admin")]
        public IActionResult ShowDisplay(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest();
            }
            try
            {
                IEnumerable<TaxForm> tax = _db.TaxForms.Include(x => x.User).Where(tf => (tf.Id == id));
                if(tax == null)
                {
                    return NotFound();
                }
                return View(tax.First());
            }
            catch (Exception e)
            {
                string ErrMsg = e.Message;
                return RedirectToAction("Error", new { ErrMsg = ErrMsg });
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeFreeze(int ?id)
        {
            if(id == null || id == 0)
            {
                return BadRequest();
            }
            
            try
            {
                TaxForm tax = _db.TaxForms.Where(tf => tf.Id == id).FirstOrDefault();
                if(tax == null)
                {
                    return NotFound();
                }
                tax.Frezeed = false;
                tax.DeclarationStatus = "Draft";
                tax.unfreezeReason = "";
                _db.TaxForms.Update(tax);
                await _db.SaveChangesAsync();
                return RedirectToAction("DisplayWithAction",new { status_msg="Unfreezed Successfully"});
            }
            catch(Exception e)
            {
                string ErrMsg = e.Message;
                return RedirectToAction("Error", new { ErrMsg = ErrMsg });
            }
            
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeDeclaration(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest();
            }
            try
            {
                TaxForm tax = _db.TaxForms.Where(tf => tf.Id == id).FirstOrDefault();
                if(tax == null)
                {
                    return NotFound();
                }
                tax.DeclarationStatus = "Accepted";
                _db.TaxForms.Update(tax);
                await _db.SaveChangesAsync();
                return RedirectToAction("DisplayWithAction", new { status_msg = "Accepted Successfully" });
            }
            catch(Exception e)
            {
                string ErrMsg = e.Message;
                return RedirectToAction("Error",new { ErrMsg =  ErrMsg});
            }
            
        }
        public IActionResult Error(string? ErrMsg)
        {
            ViewBag.ErrMsg = ErrMsg;
            return View();
        }

    }
}
