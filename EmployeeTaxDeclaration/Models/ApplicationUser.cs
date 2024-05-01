using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace EmployeeTaxDeclaration.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Required, NotNull]
        [Display(Name = "Enter your first name.")]
        [StringLength(50, ErrorMessage = "First name length should not exceed 50.")]
        public String FirstName { get; set; } = String.Empty;

        [Required, NotNull]
        [Display(Name = "Enter your last name.")]
        [StringLength(50, ErrorMessage = "Last name length should not exceed 50.")]
        public String LastName { get; set; } = String.Empty;

        [Required, NotNull]
        [Display(Name = "Enter your pan number.")]
        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]$")]
        public required string PanNumber { get; set; }

        [Required, NotNull]
        [Display(Name = "Enter your social security number.")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$")]
        public required string SocialSecurityNumber { get; set; }

        [Required]
        [Display(Name = "Choose Profile.")]
        public String ProfilePic { get; set; } = "profile.png";

        [Required, NotNull]
        [Display(Name = "Enter your address.")]
        [StringLength(150,ErrorMessage = "Address does not exceed 150 character.")]
        public required string Address { get; set; }
        [Required,NotNull]
        [Display(Name = "Pin Code")]
        [RegularExpression(@"^\d{6}$")]
        public string?  ZipCode { get; set; }


        public DateTime CreatedAt { get; set; }

        public ICollection<TaxForm> Forms { get;  } = new List<TaxForm>();
    }
}
