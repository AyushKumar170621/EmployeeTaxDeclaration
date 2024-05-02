using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EmployeeTaxDeclaration.Models
{
    public class TaxForm
    {
        [Key]
        public int Id { get; set; }

        [Required,NotNull]
        public int FinancialYear { get; set; }

        [Required,NotNull]
        [Display(Name = "1.Any other income (previous employment, income from house property, interest income etc.)")]
        public int OtherIncome { get; set; }

        
        [Display(Name = "a. Sukanya Samriddhi Account")]
        public int SukSamAcc {  get; set; }

        [Display(Name = "b. PPF/NSC/ULIP")]
        public int Ppf { get; set; }

        [Display(Name = "c. Life Insurance Premium")]
        public int LifeInsurance { get; set; }

        [Display(Name = "d. Tuition fee for child")]
        public int TuitionFee { get; set; }

        [Display(Name = "e. Bank Fixed Deposit (More than Five Years)")]
        public int FixedDeposit { get; set; }

        [Display(Name = "f. Principal amount of housing loan paid during the year.")]
        public int PrincipalAmount { get; set; }

        [Display(Name = "3. National Pension Scheme 80CCD(18) (Limit 50K).")]
        [Range(0,50000,ErrorMessage = "Does not exceed above 50k")]
        public int NPS { get; set; }

        [Display(Name = "4. Higher Education Loan interest u/s 80E.")]
        public int HighEduLoan { get; set; }

        [Display(Name = "5.Interest paid on Housing Loan u/s 24.")]
        public int HousingLoanInterest { get; set; }

        [Display(Name = "6. House Rent paid every month(For above amt Rs 8333/-PAN card of landlord have to provide.")]
        public int HouseRent { get; set; }

        [Display(Name = "7.Tax Deducted at Source by the previous employer or TDS deducted by any other person from your income.")]
        public int TDS { get; set; }

        [Display(Name = "8.Medi-claim insurance policy-premium (80 D).")]
        public int MediClaim { get; set; }

        [Display(Name = "9.Preventive Health check-up for Self & Family (80D) (Limit 5K).")]
        [Range(0,5000,ErrorMessage = "Does not exceed 5k.")]
        public int FamilyHealth { get; set; }

        public int ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; } = null;

        public bool Frezeed { get; set; } = false;
        [Display(Name = "LTA (Do you want LTA as reimbursement?)")]
        public bool LTA { get; set; } = false;
        [Display(Name = "Education Allowance (Do you want Edu Allowance as reimbursement?)")]
        public bool EduAllowance { get; set; }
        public string DeclarationStatus { get; set; } = "";

    }
}
