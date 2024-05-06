using EmployeeTaxDeclaration.Data;
using EmployeeTaxDeclaration.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "client")]
        public IActionResult Index(string? status="")
        {
            ViewBag.StatusMessage = status;
            return View();
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> ShowFormAsync(string financial)
        {
            try
            {
                //getting logged user details
                ApplicationUser usr = await GetCurrentUserAsync();
                var userId = usr.Id;

                //setting up some display fields
                ViewBag.finYear = financial;
                ViewBag.EmpName = usr.FirstName + " " + usr.LastName;
                ViewBag.PanNo = usr.PanNumber;
                ViewBag.Addr = usr.Address;
                ViewBag.Ssn = usr.SocialSecurityNumber;
                ViewBag.Email = usr.Email;
                ViewBag.Phone = usr.PhoneNumber;

                //getting all year forms of logged user
                IEnumerable<TaxForm> taxForms = _db.TaxForms.Where(tf => tf.User.Id == userId);
                if (taxForms.Any())
                {
                    foreach (TaxForm taxForm in taxForms)
                    {
                        //selecting form for selected final year
                        if (taxForm.FinancialYear == Convert.ToInt32(financial))
                        {
                            //in case the form is already submitted 
                            if (taxForm.Frezeed)
                            {
                                return RedirectToAction("FreezedDisplay", taxForm);
                            }

                            //if the form is saved then in editable menu
                            if (taxForm.DeclarationStatus == "Draft")
                            {
                                return RedirectToAction("UpdateForm", taxForm);
                            }


                            return View(taxForm);
                        }
                    }
                }
                else
                {
                    return NotFound();
                }
                //blank view if form does not exist for that year
                return View();
            }
            catch(Exception e)
            {
                string msg = e.Message;
                return RedirectToAction("Error", new { msg = msg });
            }
            
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> ShowFormAsync(TaxForm taxForm,string financial,string isSubmit)
        {
            if (!ModelState.IsValid)
            {
                string status = "Sucessfully";
                try
                {
                    ApplicationUser usr = await GetCurrentUserAsync();
                    taxForm.User = usr;

                    taxForm.FinancialYear = Convert.ToInt32(financial);
                    if (isSubmit == "true")
                    {
                        taxForm.DeclarationStatus = "Submited";
                        taxForm.Frezeed = true;
                        status = "Submitted " + status;
                    }
                    else
                    {
                        taxForm.DeclarationStatus = "Draft";
                        status = "Saved " + status;

                    }

                    taxForm.DeclarationDate = DateTime.Now;
                    _db.TaxForms.Add(taxForm);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", new { status = status });
                }
                catch(Exception e)
                {
                    string msg = e.Message;
                    return RedirectToAction("Error", new { msg = msg });
                }
            }
            return View();
        }

        public IActionResult UpdateForm(TaxForm taxForm)
        {
            return View(taxForm);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> UpdateFormAsync(TaxForm taxForm, string isSubmit)
        {
            string status = "Sucessfully";
            if (!ModelState.IsValid)
            {
                try
                {
                    ApplicationUser usr = await GetCurrentUserAsync();
                    taxForm.User = usr;
                    if (isSubmit == "true")
                    {
                        taxForm.DeclarationStatus = "Submited";
                        taxForm.Frezeed = true;
                        status = "Submitted " + status;
                    }
                    else
                    {
                        taxForm.DeclarationStatus = "Draft";
                        status = "Saved " + status;

                    }
                    taxForm.DeclarationDate = DateTime.Now;
                    _db.TaxForms.Update(taxForm);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", new { status = status });
                }
                catch(Exception e)
                {
                    string msg = e.Message;
                    return RedirectToAction("Error", new { msg = msg });
                }
            }
            return View();
        }

        [Authorize(Roles = "client")]
        public IActionResult FreezedDisplay(TaxForm taxForm)
        {
            return View(taxForm);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> FreezedDisplay(string unfreezeReason,int formId)
        {
            try
            {
                var form = _db.TaxForms.FirstOrDefault(x => x.Id == formId);
                if(form == null)
                {
                    return NotFound();
                }
                string status = "Unfreeze Request Send";
                TaxForm taxForm = form as TaxForm;
                taxForm.unfreezeReason = unfreezeReason;
                _db.TaxForms.Update(taxForm);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", new { status = status });
            }
            catch(Exception e)
            {
                string msg = e.Message;
                return RedirectToAction("Error", new { msg = msg });
            }
            
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public IActionResult Error(string? msg)
        {
            ViewBag.msg = msg;
            return View();
        }

    }
}
