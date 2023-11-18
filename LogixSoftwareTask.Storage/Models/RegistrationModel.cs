using System.ComponentModel.DataAnnotations;

namespace LogixSoftwareTask.Storage.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be at least 3 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be at least 3 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
